using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Invitations
{

    [Protect]
    class Search : IQueryHandler<InvitationSearch>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Search(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }

        public async  Task<object> Handle(InvitationSearch request)
        {
            var tenantId = requestContext.GetTenantId();
            //null check

            //make sure is owenr
            var owner = await dbContext.IsTenantOwner() ;




            throw new NotImplementedException();
        }
    }
}
