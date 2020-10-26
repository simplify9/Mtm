using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<object> Handle(InvitationSearch request)
        {
            var tenantId = requestContext.GetTenantId();
            if (!tenantId.HasValue)
                throw new SWException("Tenant is empty.");

            if (!await dbContext.IsTenantOwner())
                throw new SWUnauthorizedException();

            if (!string.IsNullOrEmpty(request.Email))
                return await dbContext.Set<Invitation>()
                    .Where(i => i.Email == request.Email && i.TenantId == tenantId.Value)
                    .Select(i => new InvitationSearchResult
                    {
                        TenantId = i.TenantId,
                        AccountId = i.AccountId,
                        CreatedBy = i.CreatedBy,
                        CreatedOn = i.CreatedOn,
                        Email = i.Email,
                        Id = i.Id,
                        ModifiedBy = i.ModifiedBy,
                        ModifiedOn = i.ModifiedOn,
                        Phone = i.Phone
                    })
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
            else
                return await dbContext.Set<Invitation>()
                    .Where(i => i.TenantId == tenantId.Value)
                    .Select(i => new InvitationSearchResult
                    {
                        TenantId = i.TenantId,
                        AccountId = i.AccountId,
                        CreatedBy = i.CreatedBy,
                        CreatedOn = i.CreatedOn,
                        Email = i.Email,
                        Id = i.Id,
                        ModifiedBy = i.ModifiedBy,
                        ModifiedOn = i.ModifiedOn,
                        Phone = i.Phone
                    })
                    .AsNoTracking()
                    .ToListAsync();

            
        }
    }
}
