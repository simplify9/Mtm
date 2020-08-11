using SW.Mtm.Api.Domain;
using SW.Mtm.Sdk.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Api.Resources.Invitations
{
    [Protect]
    [HandlerName("accept")]
    class Accept : ICommandHandler<string, InvitationAccept>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Accept(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }

        async public Task<object> Handle(string key, InvitationAccept request)
        {
            var invitation = await dbContext.FindAsync<Invitation>(key);

            if (invitation == null)
                throw new SWException("Invitation not found.");

            if (requestContext.GetEmail() != null && requestContext.GetEmail().Equals(invitation.Email, StringComparison.OrdinalIgnoreCase) ||
            requestContext.GetMobilePhone() != null && requestContext.GetMobilePhone().Equals(invitation.Phone, StringComparison.OrdinalIgnoreCase) ||
            requestContext.GetNameIdentifier() != null && requestContext.GetNameIdentifier() == invitation.AccountId)
            {
                var account = await dbContext.FindAsync<Account>(requestContext.GetNameIdentifier());

                if (account == null)
                    throw new SWException("Account not found.");

                if (!account.AddTenantMembership(new TenantMembership(invitation.TenantId, MembershipType.User)))
                    throw new SWException("Membership not updated.");

                await dbContext.SaveChangesAsync();
                return null;
            }
            else
                throw new SWException("Invitation mismatch.");
        }
    }
}
