using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW.Mtm.Model;

namespace SW.Mtm.Resources.Invitations
{
    class Get : IGetHandler<string>
    {
        private readonly MtmDbContext dbContext;

        public Get(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<object> Handle(string key, bool lookup = false)
        {
            var invitation = await dbContext.FindAsync<Invitation>(key);

            if (invitation == null)
                throw new SWNotFoundException(key);

            return new InvitationGet
            {
                TenantId = invitation.TenantId,
                AccountId = invitation.AccountId,
                CreatedBy = invitation.CreatedBy,
                CreatedOn = invitation.CreatedOn,
                Email = invitation.Email,
                Id = invitation.Id,
                ModifiedBy = invitation.ModifiedBy,
                ModifiedOn = invitation.ModifiedOn,
                Phone = invitation.Phone
            };
            
        }
    }
}
