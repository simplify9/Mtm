using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SW.PrimitiveTypes;
using SW.HttpExtensions;
using System.Security.Claims;
using SW.Mtm.Model;

namespace SW.Mtm.Sdk
{
    public class MtmClientMock : IMtmClient
    {
        private readonly MtmClientOptions mtmClientOptions;

        public MtmClientMock(MtmClientOptions mtmClientOptions)
        {
            this.mtmClientOptions = mtmClientOptions;
        }



        async public Task<AccountLoginResult> Login(AccountLogin request)
        {
            var loginResult = new AccountLoginResult();

            if (request.RefreshToken != null)
            {

                //var refreshToken = await dbContext
                //   .Set<RefreshToken>()
                //   .Where(u => u.Id == request.RefreshToken)
                //   .SingleOrDefaultAsync();

                //if (refreshToken == null)
                //    throw new SWException("User not found or invalid password.");

                //account = await dbContext
                //   .Set<Account>().FindAsync(refreshToken.AccountId);

                //loginResult.Jwt = CreateJwt(account, refreshToken.LoginMethod);
                //loginResult.RefreshToken = CreateRefreshToken(account, refreshToken.LoginMethod);

                //dbContext.Remove(refreshToken);

            }
            else if (request.OtpToken != null)
            {
                //var otpToken = await dbContext
                //   .Set<OtpToken>()
                //   .Where(u => u.Id == request.OtpToken)
                //   .SingleOrDefaultAsync();

                //if (otpToken == null)
                //    throw new SWException("User not found or invalid password.");



                //switch (otpToken.Type)
                //{
                //    case OtpType.Otp:
                //        if (request.Password == null || !SecurePasswordHasher.Verify(request.Password, otpToken.Password))
                //            throw new SWException("User not found or invalid password.");

                //        break;
                //    case OtpType.Totp:
                //        break;
                //    default:
                //        throw new SWException("User not found or invalid password.");

                //}

                //account = await dbContext
                //   .Set<Account>().FindAsync(otpToken.AccountId);

                //loginResult.Jwt = CreateJwt(account, otpToken.LoginMethod);
                //loginResult.RefreshToken = CreateRefreshToken(account, otpToken.LoginMethod);

                //dbContext.Remove(otpToken);
            }
            else if (request.ApiKey != null)
            {
                if (request.ApiKey == mtmClientOptions.MockData["ApiKey"])
                {
                    loginResult.Jwt = CreateJwt(LoginMethod.ApiKey);
                }
                else
                {
                    throw new SWException("User not found or invalid password.");
                }
            }
            else if (request.Email != null)
            {

                if (request.Email == mtmClientOptions.MockData["Email"] && request.Password == mtmClientOptions.MockData["Password"])
                {
                    loginResult.Jwt = CreateJwt(LoginMethod.EmailAndPassword);
                    loginResult.RefreshToken = mtmClientOptions.MockData["RefreshToken"];
                }
                else
                {
                    throw new SWException("User not found or invalid password.");
                }
                //account = await dbContext
                //   .Set<Account>()
                //   .Where(u => u.Email == request.Email && u.EmailProvider == request.EmailProvider && (u.LoginMethods & LoginMethod.EmailAndPassword) != 0)
                //   .SingleOrDefaultAsync();

                //if (account == null)
                //    throw new SWException("User not found or invalid password.");

                //if (request.EmailProvider == EmailProvider.None && request.Password == null || request.EmailProvider == EmailProvider.None && !SecurePasswordHasher.Verify(request.Password, account.Password))
                //    throw new SWException("User not found or invalid password.");


                //switch (account.SecondFactorMethod)
                //{
                //    case OtpType.None:
                //        loginResult.Jwt = CreateJwt(account, LoginMethod.EmailAndPassword);
                //        loginResult.RefreshToken = CreateRefreshToken(account, LoginMethod.EmailAndPassword);
                //        break;
                //    case OtpType.Otp:
                //    case OtpType.Totp:
                //        var otpToken = CreateOtpToken(account, LoginMethod.EmailAndPassword, account.SecondFactorMethod);
                //        loginResult.OtpType = account.SecondFactorMethod;
                //        loginResult.OtpToken = otpToken.Key;
                //        loginResult.Password = otpToken.Value;
                //        break;
                //    default:
                //        throw new NotImplementedException();

                //}

            }
            else if (request.Phone != null)
            {
                //account = await dbContext
                //   .Set<Account>()
                //   .Where(u => u.Phone == request.Phone && (u.LoginMethods & LoginMethod.PhoneAndOtp) != 0)
                //   .SingleOrDefaultAsync();

                //if (account == null)
                //    throw new SWException("User not found or invalid password.");

                //var otpToken = CreateOtpToken(account, LoginMethod.PhoneAndOtp, OtpType.Otp);
                //loginResult.OtpType = OtpType.Otp;
                //loginResult.OtpToken = otpToken.Key;
                //loginResult.Password = otpToken.Value;
            }

            //await dbContext.SaveChangesAsync();

            return loginResult;
        }

        private string CreateJwt(LoginMethod loginMethod)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, mtmClientOptions.MockData["AccountId"]),
                new Claim(ClaimTypes.GivenName, mtmClientOptions.MockData["DisplayName"]),
                new Claim("EmailVerified", true.ToString()),
                new Claim("PhoneVerified", true.ToString()),
            };

            switch (loginMethod)
            {
                case LoginMethod.EmailAndPassword:
                    claims.Add(new Claim(ClaimTypes.Name, mtmClientOptions.MockData["Email"]));
                    break;
                case LoginMethod.PhoneAndOtp:
                    claims.Add(new Claim(ClaimTypes.Name, mtmClientOptions.MockData["Phone"]));
                    break;
                case LoginMethod.ApiKey:
                    claims.Add(new Claim(ClaimTypes.Name, mtmClientOptions.MockData["AccountId"]));
                    break;
            }

            claims.Add(new Claim(ClaimTypes.Email, mtmClientOptions.MockData["Email"]));
            claims.Add(new Claim(ClaimTypes.MobilePhone, mtmClientOptions.MockData["Phone"]));
            if (mtmClientOptions.MockData.ContainsKey("TenantId")) claims.Add(new Claim("TenantId", mtmClientOptions.MockData["TenantId"]));

            return mtmClientOptions.Token.WriteJwt(new ClaimsIdentity(claims, "Mtm"));
        }

        public Task<AccountRegisterResult> Register(AccountRegister registerAccount)
        {
            throw new NotImplementedException();
        }

        public Task<TenantCreateResult> CreateTenant(TenantCreate registerAccount)
        {
            throw new NotImplementedException();
        }

        public Task<TenantCreateResult> CreateAdditionalTenant(TenantCreate registerAccount)
        {
            throw new NotImplementedException();
        }

   

        public async Task<AccountChangePasswordResult> ChangePassword(string accountId, AccountChangePassword changePasswordAccount)
        {
            return null ;
        }
    }
}
