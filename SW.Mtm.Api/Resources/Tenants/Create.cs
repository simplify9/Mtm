using SW.Mtm.Api.Domain;
using SW.Mtm.Sdk.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Api.Resources.Tenants
{
    [HandlerName("create")]
    [Protect(RequireRole = false)]
    class Create : ICommandHandler<TenantCreate>
    {
        private readonly RequestContext requestContext;
        private readonly MtmDbContext dbContext;

        public Create(RequestContext requestContext, MtmDbContext dbContext)
        {
            this.requestContext = requestContext;
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(TenantCreate request)
        {
            var tenant = new Tenant(request.DisplayName);
            await dbContext.AddAsync(tenant);

            Account account;

            if (requestContext.GetNameIdentifier() == Account.SystemId)
            {
                if (request.OwnerEmail != null)
                {
                    if (request.OwnerEmailProvider == EmailProvider.None)
                        account = new Account(request.OwnerDisplayName, request.OwnerEmail, SecurePasswordHasher.Hash(request.OwnerPassword));
                    else
                        account = new Account(request.OwnerDisplayName, request.OwnerEmail, request.OwnerEmailProvider);
                }
                //else if (request.OwnerPhone != null)
                //{
                //    account = new Account(request.DisplayName, request.OwnerPhone);

                //}
                else
                {
                    throw new NotImplementedException();
                }

                dbContext.Add(account);
            }
            else
            {
                account = await dbContext.Set<Account>().FindAsync(requestContext.GetNameIdentifier());
            }

            account.AddTenantMembership(new TenantMembership(tenant.Id, MembershipType.Owner));
            
            await dbContext.SaveChangesAsync();

            return null;
        }
    }
}
