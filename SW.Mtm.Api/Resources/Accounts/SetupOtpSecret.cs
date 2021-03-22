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
    [HandlerName("setupOtpSecret")]
    [Protect]
    class SetupOtpSecret : ICommandHandler<AccountSetupOtpRequest>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;
        private readonly IConfiguration configuration;



        public SetupOtpSecret(MtmDbContext dbContext, RequestContext requestContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
            this.configuration = configuration;
        }

        public async Task<object> Handle(AccountSetupOtpRequest request)
        {
            var accountId = requestContext.GetNameIdentifier();
            var secret = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secret);

            var account = await dbContext
               .Set<Account>()
               .Where(i => i.Id == accountId)
               .SingleOrDefaultAsync();

            account.SetupSecondFactor(request.Type, base32Secret);

            await dbContext.SaveChangesAsync();

            var issuer = configuration["Totp:Issuer"];
   
            var totpDigits = 6;
            var totpPeriod = 30;
            var hashMethod = OtpHashMode.Sha256;

            var qrCodeUrl = $"otpauth://totp/{issuer}:{account.Email}?secret={base32Secret}&issuer={issuer}&algorithm={hashMethod}&digits={totpDigits}&period={totpPeriod}";

            return new AccountSetupTotpResult
            {
                QrCodeUrl = qrCodeUrl,
                SecretKey = base32Secret
            };

        }
        class Validator : AbstractValidator<AccountSetupOtpRequest>
        {
            public Validator()
            {
                RuleFor(p=>p.Type).NotEmpty();

            }

        }


    }
}
