﻿using Microsoft.EntityFrameworkCore.Internal;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SW.Mtm.Resources.Tenants
{
    [Protect]
    [HandlerName("invite")]
    class Invite : ICommandHandler<int, TenantInvite,object>
    {
        private readonly MtmDbContext dbContext;

        public Invite(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(int key, TenantInvite request)
        {
            if (!(await dbContext.IsRequesterLandlord() || await dbContext.IsRequesterTenantOwner(key)))
                throw new SWUnauthorizedException();


            Invitation invitation;

            if (request.AccountId != null)
            {
                if (dbContext.Set<Invitation>().Any(i => i.AccountId == request.AccountId && i.TenantId == key))
                    throw new SWException("Invitaion already sent.");

                var account = await dbContext.FindAsync<Account>(request.AccountId);
                if (account == null)
                    throw new SWNotFoundException(request.AccountId);
                if (account.TenantMemberships.Any(i => i.TenantId == key))
                    throw new SWException("Account already member.");

                invitation = Invitation.ByAccountId(key, request.AccountId);

            }

            else if (request.Email != null)
            {
                if( dbContext.Set<Invitation>().Any(i => i.Email.ToLower() == request.Email.ToLower() && i.TenantId == key))
                    throw new SWException("Invitaion already sent.");

                var account = await dbContext.Set<Account>().FirstOrDefaultAsync(i => i.Email.ToLower() == request.Email.ToLower());
                if (account != null && account.TenantMemberships.Any(i => i.TenantId == key))
                    throw new SWException("Account already member.");
                
                invitation = Invitation.ByEmail(key, request.Email);
            }

            else if (request.Phone != null)
            {
                if (dbContext.Set<Invitation>().Any(i => i.Phone == request.Phone && i.TenantId == key))
                    throw new SWException("Invitaion already sent.");

                var account = await dbContext.Set<Account>().FirstOrDefaultAsync(i => i.Phone == request.Phone);
                if (account != null && account.TenantMemberships.Any(i => i.TenantId == key))
                    throw new SWException("Account already member.");
                invitation = Invitation.ByPhone(key, request.Phone);

            }

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
