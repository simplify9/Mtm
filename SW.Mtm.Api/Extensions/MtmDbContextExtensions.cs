using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm
{
    static class MtmDbContextExtensions
    {
        public static Task<bool> IsRequesterTenantOwner(this MtmDbContext dbContext)
        {
            var tenentId = dbContext.RequestContext.GetTenantId();

            if (tenentId.HasValue)
                return dbContext.IsRequesterTenantOwner(tenentId.Value);

            return Task.FromResult(false);
        }
        async public static Task<bool> IsRequesterTenantOwner(this MtmDbContext dbContext, int tenantId)
        {
            return await dbContext.Set<Account>().AnyAsync(a => a.Id == dbContext.RequestContext.GetNameIdentifier() && a.TenantMemberships.Any(m => m.TenantId == tenantId && m.Type == MembershipType.Owner));
        }
        async public static Task<bool> IsRequesterLandlord(this MtmDbContext dbContext)
        {
            return await dbContext.Set<Account>().AnyAsync(a => a.Id == dbContext.RequestContext.GetNameIdentifier() && a.Landlord);
        }

    }
}
