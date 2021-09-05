using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("otp")]
    [Protect(RequireRole = true)]
    public class GenerateOtp : ICommandHandler<AccountCreate>
    {
        private readonly MtmDbContext dbContext;

        public GenerateOtp(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<object> Handle(AccountCreate request)
        {
            if (!await dbContext.IsRequesterLandlord() &&
                !await dbContext.IsRequesterTenantOwner())
                throw new SWUnauthorizedException();

            var account = await dbContext.Set<Account>()
                .Include(a => a.TenantMemberships)
                .FirstOrDefaultAsync(i => i.Id == request.Phone);

            var result = new AccountLoginResult();

            if (account == null)
            {
                account = new Account(request.DisplayName, request.Phone);
                dbContext.Add(account);

                if (await dbContext.IsRequesterLandlord() && request.TenantId != null)
                {
                    account.AddTenantMembership(new TenantMembership(request.TenantId.Value, MembershipType.User)
                    {
                        ProfileData = request.ProfileData
                    });
                }

                result.IsCreated = true;

                await dbContext.SaveChangesAsync();
            }

            if ((account.LoginMethods & LoginMethod.PhoneAndOtp) == 0)
                throw new SWUnauthorizedException();


            var otpToken = CreateOtpToken(account, LoginMethod.PhoneAndOtp, OtpType.Otp, request.MockOtp ?? false);

            await dbContext.SaveChangesAsync();


            result.OtpType = OtpType.Otp;
            result.OtpToken = otpToken.Key;
            result.Password = otpToken.Value;
            
            return result;
        }

        private KeyValuePair<string, string> CreateOtpToken(Account account, LoginMethod loginMethod, OtpType otpType,
            bool mock = false)
        {
            OtpToken otpToken;
            string password = null;

            switch (otpType)
            {
                case OtpType.Otp:
                    password = mock ? "1234" : RandomNumberGenerator.GetInt32(1000, 9999).ToString();
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
    }
}