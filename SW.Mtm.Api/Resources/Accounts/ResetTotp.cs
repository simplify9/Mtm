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
    [HandlerName("resetTotp")]
    [Protect]
    class ResetTotp : ICommandHandler<string>
    {
        private readonly MtmDbContext dbContext;




        public ResetTotp(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
         
        }

        public async Task<object> Handle(string key)
        {
            if (!await dbContext.IsRequesterLandlord())
                throw new SWUnauthorizedException();
            
            var accountId = key;

            var account = await dbContext
               .Set<Account>()
               .Where(i => i.Id == accountId)
               .FirstOrDefaultAsync();
            
            if (account == null)
                throw new SWNotFoundException();

            account.ResetSecondFactor();

            await dbContext.SaveChangesAsync();

            return null;
        }
       


    }
}
