using Microsoft.EntityFrameworkCore.Internal;
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
            {
                var account = await dbContext.FindAsync<Account>(request.AccountId);
                if (account == null)
                    throw new SWNotFoundException(request.AccountId);
                if (account.TenantMemberships.Any(i => i.TenantId == key))
                    throw new SWException("Account already member.");

                invitation = Invitation.ByAccountId(key, request.AccountId);

            }

            else if (request.Email != null)
            {
                var account = await dbContext.Set<Account>().FirstOrDefaultAsync(i => i.Email == request.Email);
                if (account != null && account.TenantMemberships.Any(i => i.TenantId == key))
                    throw new SWException("Account already member.");
                
                invitation = Invitation.ByEmail(key, request.Email);
            }

            else if (request.Phone != null)
            {
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
