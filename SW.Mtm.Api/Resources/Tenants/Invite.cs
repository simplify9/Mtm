using SW.Mtm.Api.Domain;
using SW.Mtm.Sdk.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Api.Resources.Tenants
{
    [Protect]
    [HandlerName("invite")]
    class Invite : ICommandHandler<int, TenantInvite>
    {
        private readonly MtmDbContext dbContext;

        public Invite(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(int key, TenantInvite request)
        {
            if (!(await dbContext.IsLandlord() || await dbContext.IsTenantOwner(key)))
                throw new SWException("Unauthorized.");


            Invitation invitation;

            if (request.AccountId != null)
                invitation = Invitation.ByAccountId(key, request.AccountId);

            else if (request.Email != null)
                invitation = Invitation.ByEmail(key, request.Email);

            else if (request.Phone != null)
                invitation = Invitation.ByPhone(key, request.Phone);

            else
                throw new SWException("Missing data.");

            dbContext.Add(invitation);
            await dbContext.SaveChangesAsync();

            return new TenantInviteResult
            {
                Id = invitation.Id
            };
        }
    }
}
