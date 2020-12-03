using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using SW.HttpExtensions;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("login")]
    [Protect]
    class Login : ICommandHandler<AccountLogin>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;
        private readonly JwtTokenParameters jwtTokenParameters;

        public Login(MtmDbContext dbContext, RequestContext requestContext,  JwtTokenParameters jwtTokenParameters)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
            this.jwtTokenParameters = jwtTokenParameters;
        }

        async public Task<object> Handle(AccountLogin request)
        {

            Account account = null;
            var loginResult = new AccountLoginResult();

            if (request.RefreshToken != null)
            {

                var refreshToken = await dbContext
                   .Set<RefreshToken>()
                   .Where(u => u.Id == request.RefreshToken)
                   .SingleOrDefaultAsync();

                if (refreshToken == null)
                    throw new SWUnauthorizedException();

                //account = await dbContext
                //   .Set<Account>().FindAsync(refreshToken.AccountId);
                account = await dbContext
                    .Set<Account>().Where(a => a.Id == refreshToken.AccountId && !a.Disabled)
                    .SingleOrDefaultAsync();

                if (account == null)
                    throw new SWUnauthorizedException();

                loginResult.Jwt = account.CreateJwt(refreshToken.LoginMethod, jwtTokenParameters);
                loginResult.RefreshToken = CreateRefreshToken(account, refreshToken.LoginMethod);

                dbContext.Remove(refreshToken);

            }
            else if (request.OtpToken != null)
            {
                var otpToken = await dbContext
                   .Set<OtpToken>()
                   .Where(u => u.Id == request.OtpToken)
                   .SingleOrDefaultAsync();

                if (otpToken == null)
                    throw new SWException("User not found or invalid password.");



                switch (otpToken.Type)
                {
                    case OtpType.Otp:
                        if (request.Password == null || !SecurePasswordHasher.Verify(request.Password, otpToken.Password))
                            throw new SWException("User not found or invalid password.");

                        break;
                    case OtpType.Totp:
                        break;
                    default:
                        throw new SWException("User not found or invalid password.");

                }

                //account = await dbContext
                //   .Set<Account>().FindAsync(otpToken.AccountId);
                account = await dbContext
                    .Set<Account>().Where(a => a.Id == otpToken.AccountId && !a.Disabled)
                    .SingleOrDefaultAsync();
                loginResult.Jwt = account.CreateJwt(otpToken.LoginMethod, jwtTokenParameters);
                loginResult.RefreshToken = CreateRefreshToken(account, otpToken.LoginMethod);

                dbContext.Remove(otpToken);
            }
            else if (request.ApiKey != null)
            {
                account = await dbContext
                   .Set<Account>()
                   .Where(u => u.ApiCredentials.Any(cred => cred.Key == request.ApiKey) && (u.LoginMethods & LoginMethod.ApiKey) != 0 && !u.Disabled)
                   .SingleOrDefaultAsync();

                if (account == null)
                    throw new SWNotFoundException(request.ApiKey);

                loginResult.Jwt = account.CreateJwt(LoginMethod.ApiKey, jwtTokenParameters);

            }
            else if (request.Email != null)
            {
                account = await dbContext
                   .Set<Account>()
                   .Where(u => u.Email == request.Email && u.EmailProvider == request.EmailProvider && (u.LoginMethods & LoginMethod.EmailAndPassword) != 0 && !u.Disabled)
                   .SingleOrDefaultAsync();

                if (account == null)
                    throw new SWNotFoundException(request.Email);

                if (request.EmailProvider == EmailProvider.None && request.Password == null || request.EmailProvider == EmailProvider.None && !SecurePasswordHasher.Verify(request.Password, account.Password))
                    throw new SWException("Invalid password.");


                switch (account.SecondFactorMethod)
                {
                    case OtpType.None:
                        loginResult.Jwt = account.CreateJwt(LoginMethod.EmailAndPassword, jwtTokenParameters);
                        loginResult.RefreshToken = CreateRefreshToken(account, LoginMethod.EmailAndPassword);
                        break;
                    case OtpType.Otp:
                    case OtpType.Totp:
                        var otpToken = CreateOtpToken(account, LoginMethod.EmailAndPassword, account.SecondFactorMethod);
                        loginResult.OtpType = account.SecondFactorMethod;
                        loginResult.OtpToken = otpToken.Key;
                        loginResult.Password = otpToken.Value;
                        break;
                    default:
                        throw new NotImplementedException();

                }

            }
            else if (request.Phone != null)
            {
                account = await dbContext
                   .Set<Account>()
                   .Where(u => u.Phone == request.Phone && !u.Disabled)
                   .SingleOrDefaultAsync();

                if (account == null)
                    throw new SWNotFoundException(request.Phone);

                if ((account.LoginMethods & LoginMethod.PhoneAndOtp) == 0)
                    throw new SWUnauthorizedException();

                var otpToken = CreateOtpToken(account, LoginMethod.PhoneAndOtp, OtpType.Otp);
                loginResult.OtpType = OtpType.Otp;
                loginResult.OtpToken = otpToken.Key;
                loginResult.Password = otpToken.Value;
            }
            else
            {
                account = await dbContext
                   .Set<Account>()
                   .Where(u => u.Email == requestContext.GetEmail() && u.EmailProvider == request.EmailProvider && (u.LoginMethods & LoginMethod.EmailAndPassword) != 0 && !u.Disabled)
                   .SingleOrDefaultAsync();

                if (account == null)
                    throw new SWNotFoundException(requestContext.GetEmail());

                switch (account.SecondFactorMethod)
                {
                    case OtpType.None:
                        loginResult.Jwt = account.CreateJwt(LoginMethod.EmailAndPassword, jwtTokenParameters);
                        loginResult.RefreshToken = CreateRefreshToken(account, LoginMethod.EmailAndPassword);
                        break;
                    case OtpType.Otp:
                    case OtpType.Totp:
                        var otpToken = CreateOtpToken(account, LoginMethod.EmailAndPassword, account.SecondFactorMethod);
                        loginResult.OtpType = account.SecondFactorMethod;
                        loginResult.OtpToken = otpToken.Key;
                        loginResult.Password = otpToken.Value;
                        break;
                    default:
                        throw new NotImplementedException();

                }
            }

            await dbContext.SaveChangesAsync();

            return loginResult;

        }

        private KeyValuePair<string, string> CreateOtpToken(Account account, LoginMethod loginMethod, OtpType otpType)
        {
            OtpToken otpToken;
            string password = null;

            switch (otpType)
            {
                case OtpType.Otp:
                    password = RandomNumberGenerator.GetInt32(1000, 9999).ToString();
                    otpToken = new OtpToken(account.Id, loginMethod, SecurePasswordHasher.Hash(password));
                    break;
                case OtpType.Totp:
                    otpToken = new OtpToken(account.Id, loginMethod);
                    break;
                default:
                    throw new NotImplementedException();
            }

            dbContext.Add(otpToken);

            return KeyValuePair.Create(otpToken.Id, password);
        }

        private string CreateRefreshToken(Account account, LoginMethod loginMethod)
        {
            var refreshToken = new RefreshToken(account.Id, loginMethod);
            dbContext.Add(refreshToken);
            return refreshToken.Id;
        }
        class Validator : AbstractValidator<AccountLogin>
        {
            public Validator(RequestContext requestContext)
            {
                When(request => requestContext.GetNameIdentifier() == Account.SystemId, () =>
                {
                    RuleFor(p => p.Email).NotEmpty().When(p => p.RefreshToken == null && p.Phone == null && p.ApiKey == null && p.OtpToken == null);
                    RuleFor(p => p.ApiKey).NotEmpty().When(p => p.RefreshToken == null && p.Phone == null && p.Email == null && p.OtpToken == null);
                    RuleFor(p => p.Phone).NotEmpty().When(p => p.RefreshToken == null && p.Email == null && p.ApiKey == null && p.OtpToken == null);
                    RuleFor(p => p.OtpToken).NotEmpty().When(p => p.RefreshToken == null && p.Phone == null && p.ApiKey == null && p.Email == null);
                    RuleFor(p => p.RefreshToken).NotEmpty().When(p => p.OtpToken == null && p.Phone == null && p.ApiKey == null && p.Email == null);
                    RuleFor(p => p.Email).EmailAddress();
                    RuleFor(p => p.Password).NotEmpty().When(p => p.EmailProvider == EmailProvider.None && p.Email != null || p.OtpToken != null);
                    RuleFor(p => p.Password).Empty().When(p => p.EmailProvider != EmailProvider.None && p.Email != null || p.ApiKey != null || p.Phone != null || p.RefreshToken != null);

                }).Otherwise(() =>
                {
                    RuleFor(p => p.EmailProvider).NotEqual(EmailProvider.None);
                });
            }

        }
    }
}
