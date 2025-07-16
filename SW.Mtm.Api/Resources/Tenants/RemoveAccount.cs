using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Tenants
{
    [Protect]
    [HandlerName("removeaccount")]
    class RemoveAccount : ICommandHandler<int, TenantRemoveAccount,object>
    {
        private readonly MtmDbContext mtmDbContext;

        public RemoveAccount(MtmDbContext mtmDbContext)
        {
            this.mtmDbContext = mtmDbContext;
        }

        async public Task<object> Handle(int key, TenantRemoveAccount request)
        {
            if (!await mtmDbContext.IsRequesterTenantOwner(key) && !await mtmDbContext.IsRequesterLandlord())
                throw new SWUnauthorizedException();

            var account = await mtmDbContext.FindAsync<Account>(request.AccountId);

            if (account == null)
                throw new SWNotFoundException();

            if (!account.RemoveTenantMembership(key))
                throw new SWException("Membership not updated.");

            await mtmDbContext.SaveChangesAsync();

            return null;
        }
    }
}
