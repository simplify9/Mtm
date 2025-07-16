﻿using Microsoft.EntityFrameworkCore;
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
    [HandlerName("Cancel")]
    [Protect]
    class Cancel : ICommandHandler<InvitationCancel,object>
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
            if (!tenantId.HasValue)
                throw new SWException("Tenant is empty.");

             if (!await dbContext.IsRequesterTenantOwner() && !await dbContext.IsRequesterLandlord())
                throw new SWUnauthorizedException();

            if (!string.IsNullOrEmpty(request.Email))
            {
                var inv= await dbContext.Set<Invitation>()
                    .Where(i => i.Email == request.Email && i.TenantId == tenantId.Value)
                    .SingleOrDefaultAsync();
                
                if (inv == null)
                    throw new SWException("Invitation not found.");

                dbContext.Remove(inv);
                await dbContext.SaveChangesAsync();
            }
            else
                throw new SWException("Email is empty.");

            return null;
            
        }
    }
}
