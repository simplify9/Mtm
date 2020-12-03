using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtpNet;
using SW.HttpExtensions;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("externallogin")]
    [Protect(RequireRole = true)]
    class ExternalLogin : ICommandHandler<AccountExternalLogin>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;
        private readonly JwtTokenParameters jwtTokenParameters;

        public ExternalLogin(MtmDbContext dbContext, RequestContext requestContext, JwtTokenParameters jwtTokenParameters)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
            this.jwtTokenParameters = jwtTokenParameters;
        }

        async public Task<object> Handle(AccountExternalLogin request)
        {

            Account account = null;
            var loginResult = new AccountLoginResult();

            account = await dbContext
               .Set<Account>()
               .Where(u => u.Email == requestContext.GetEmail() && u.EmailProvider == request.EmailProvider && (u.LoginMethods & LoginMethod.EmailAndPassword) != 0 && !u.Disabled)
               .SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(requestContext.GetEmail());

            //if (request.EmailProvider == EmailProvider.None && request.Password == null || request.EmailProvider == EmailProvider.None && !SecurePasswordHasher.Verify(request.Password, account.Password))
            //    throw new SWException("Invalid password.");


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

        class Validator : AbstractValidator<AccountExternalLogin>
        {
            public Validator()
            {
            }

        }
    }
}
