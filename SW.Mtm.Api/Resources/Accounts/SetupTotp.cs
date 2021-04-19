using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OtpNet;
using SW.Mtm.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("setupTotp")]
    [Protect]
    class SetupTotp : ICommandHandler
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;
        private readonly MtmOptions mtmOptions;



        public SetupTotp(MtmDbContext dbContext, RequestContext requestContext, MtmOptions mtmOptions)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
            this.mtmOptions = mtmOptions;
        }

        public async Task<object> Handle()
        {
            var accountId = requestContext.GetNameIdentifier();
            var secret = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secret);

            var account = await dbContext
               .Set<Account>()
               .Where(i => i.Id == accountId)
               .SingleOrDefaultAsync();

            account.SetupSecondFactor(OtpType.Totp, base32Secret);

            await dbContext.SaveChangesAsync();

            var issuer = mtmOptions.TotpIssuer;
   
            var qrCodeUrl = $"otpauth://totp/{issuer}:{account.Email}?secret={secret}";

            return new AccountSetupTotpResult
            {
                QrCodeUrl = qrCodeUrl,
                SecretKey = base32Secret
            };

        }
       


    }
}
