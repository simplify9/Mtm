using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [Protect]
    [HandlerName("setprofiledata")]
    class Update : ICommandHandler<string, AccountUpdate>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Update(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }

        async public Task<object> Handle(string accountIdOrEmail, AccountUpdate request)
        {

            var account = await dbContext.FindAsync<Account>(accountIdOrEmail);
            if (account == null)
                account = await dbContext.Set<Account>().Where(i => i.Email == accountIdOrEmail).SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(accountIdOrEmail);

            if (request.TenantId == null)
            {
                if (requestContext.GetNameIdentifier() != accountIdOrEmail && requestContext.GetEmail() != accountIdOrEmail && !await dbContext.IsRequesterLandlord())
                    throw new SWUnauthorizedException();
            }
            else
            {
                if (requestContext.GetNameIdentifier() != accountIdOrEmail && requestContext.GetEmail() != accountIdOrEmail && !await dbContext.IsRequesterLandlord() && !await dbContext.IsRequesterTenantOwner(request.TenantId.Value))
                    throw new SWUnauthorizedException();
            }

            if (request.TenantId == null)
                account.ProfileData = request.ProfileData.ToList();
            else
            {
                var membership = account.TenantMemberships.SingleOrDefault(m => m.TenantId == request.TenantId.Value);
                if (membership == null)
                    throw new SWNotFoundException(request.TenantId.Value.ToString());
                else
                    membership.ProfileData = request.ProfileData.ToList();
            }

            await dbContext.SaveChangesAsync();

            return null;

        }

        private class Validate : AbstractValidator<AccountUpdate>
        {
            public Validate()
            {
                RuleFor(p => p.ProfileData).NotNull();
            }
        }
    }
}
