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
    [HandlerName("addaccount")]
    class AddAccount : ICommandHandler<int, TenantAddAccount>
    {
        private readonly MtmDbContext mtmDbContext;

        public AddAccount(MtmDbContext mtmDbContext)
        {
            this.mtmDbContext = mtmDbContext;
        }

        async public Task<object> Handle(int key, TenantAddAccount request)
        {
            if (!await mtmDbContext.IsTenantOwner(key) && !await mtmDbContext.IsLandlord())
                throw new SWUnauthorizedException();

            var account = await mtmDbContext.FindAsync<Account>(request.AccountId);

            if (account == null)
                throw new SWNotFoundException();

            if (!account.AddTenantMembership(new TenantMembership(key, request.MembershipType)))
                throw new SWException("Membership not updated.");

            await mtmDbContext.SaveChangesAsync();

            return null;
        }
    }
}
