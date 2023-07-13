using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Mtm.Model;

namespace SW.Mtm.Resources.Accounts
{
    class Get : IGetHandler<string>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Get(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }

        public async Task<object> Handle(string accountIdOrEmail, bool lookup = false)
        {
            if (requestContext.GetNameIdentifier() != accountIdOrEmail &&
                requestContext.GetEmail() != accountIdOrEmail &&
                !await dbContext.IsRequesterLandlord() &&
                !await dbContext.IsRequesterTenantOwner())
                throw new SWUnauthorizedException();


            var accountQuery =  dbContext.Set<Account>()
                .Include(a => a.TenantMemberships);
            
            var account = await accountQuery.FirstOrDefaultAsync(i => i.Id == accountIdOrEmail) ??
                          await accountQuery.Where(i => i.Email == accountIdOrEmail).SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(accountIdOrEmail);

            var response = new AccountGet
            {
                Id = account.Id,
                Email = account.Email,
                Disabled = account.Disabled,
                Phone = account.Phone,
                Roles = account.Roles,
                CreatedOn = account.CreatedOn,
                DisplayName = account.DisplayName,
                EmailProvider = account.EmailProvider,
                LoginMethods = account.LoginMethods,
                ProfileData = account.ProfileData,
                SecondFactorMethod = account.SecondFactorMethod,
                
            };

            if (requestContext.GetNameIdentifier() == accountIdOrEmail ||
                requestContext.GetEmail() == accountIdOrEmail ||
                await dbContext.IsRequesterLandlord())
            {
                response.TenantIdsMemberships = account.TenantMemberships.Select(t => t.TenantId).ToList();
            }

            return response;
            
        }
    }
}
