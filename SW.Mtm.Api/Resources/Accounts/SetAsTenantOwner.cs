using Microsoft.EntityFrameworkCore;
using SW.HttpExtensions;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("setastenantowner")]
    [Protect]
    class SetAsTenantOwner : ICommandHandler<AccountSetAsTenantOwner>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;
        public SetAsTenantOwner(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }
        public async Task<object> Handle(AccountSetAsTenantOwner request)
        {
            if (requestContext.GetNameIdentifier() == Account.SystemId)
                throw new SWUnauthorizedException();

            var account = await dbContext
               .Set<Account>()
               .Include(m => m.TenantMemberships)
               .Where(u => u.Id == request.AccountId && u.TenantMemberships.Any(m => m.TenantId == request.TenantId))
               .SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(request.AccountId.ToString());

            var membership = account.TenantMemberships.FirstOrDefault(i => i.TenantId == request.TenantId);

            membership.ChangeType(MembershipType.Owner);

            await dbContext.SaveChangesAsync();
           
            return null;
        }

      
    }
}
