using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Invitations
{
    [Protect]
    class Cancel : ICommandHandler<InvitationCancel>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Cancel(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }

        async public Task<object> Handle(InvitationCancel request)
        {
            var tenantId = requestContext.GetTenantId();
            //null check

            //make sure is owenr
            var owner = await dbContext.IsTenantOwner();


            throw new NotImplementedException();
        }
    }
}
