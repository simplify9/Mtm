using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.PrimitiveTypes;
using System.Linq;
using System.Threading.Tasks;
using SW.Mtm.Model;

namespace SW.Mtm.Resources.Accounts
{
    class Get : IGetHandler<string,object>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Get(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }

        public async Task<object> Handle(string accountIdOrEmailOrPhone)
        {
            if (requestContext.GetNameIdentifier() != accountIdOrEmailOrPhone &&
                requestContext.GetEmail() != accountIdOrEmailOrPhone &&
                requestContext.GetMobilePhone() != accountIdOrEmailOrPhone &&
                !await dbContext.IsRequesterLandlord() &&
                !await dbContext.IsRequesterTenantOwner())
                throw new SWUnauthorizedException();
            
            var accountQuery =  dbContext.Set<Account>()
                .Include(a => a.TenantMemberships);
            
            var account = await accountQuery.FirstOrDefaultAsync(i => i.Id == accountIdOrEmailOrPhone) ??
                          await accountQuery.FirstOrDefaultAsync(i => i.Phone == accountIdOrEmailOrPhone) ??
                          await accountQuery.Where(i => i.Email == accountIdOrEmailOrPhone).SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(accountIdOrEmailOrPhone);

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
            
            if (requestContext.GetNameIdentifier() == accountIdOrEmailOrPhone ||
                requestContext.GetEmail() == accountIdOrEmailOrPhone ||
                requestContext.GetMobilePhone() == accountIdOrEmailOrPhone ||
                await dbContext.IsRequesterLandlord())
            {
                response.TenantIdsMemberships = account.TenantMemberships.Select(t => t.TenantId).ToList();
            }
            return response;
        }
    }
}
