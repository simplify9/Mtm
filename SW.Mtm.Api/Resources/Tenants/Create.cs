using FluentValidation;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Tenants
{
    [Protect]
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
                //if (request.OwnerEmail != null)
                //{
                if (request.OwnerEmailProvider == EmailProvider.None)
                    if (request.OwnerSecondFactorMethod == OtpType.None)
                        account = new Account(request.OwnerDisplayName, request.OwnerEmail.ToLower(), SecurePasswordHasher.Hash(request.OwnerPassword));
                    else
                        account = new Account(request.OwnerDisplayName, request.OwnerEmail.ToLower(), SecurePasswordHasher.Hash(request.OwnerPassword), request.OwnerSecondFactorMethod);

                //account = new Account(request.OwnerDisplayName, request.OwnerEmail.ToLower(), SecurePasswordHasher.Hash(request.OwnerPassword));
                else
                     if (request.OwnerSecondFactorMethod == OtpType.None)
                    account = new Account(request.OwnerDisplayName, request.OwnerEmail.ToLower(), request.OwnerEmailProvider);
                else
                    account = new Account(request.OwnerDisplayName, request.OwnerEmail.ToLower(), request.OwnerEmailProvider, request.OwnerSecondFactorMethod);
                //account = new Account(request.OwnerDisplayName, request.OwnerEmail.ToLower(), request.OwnerEmailProvider);
                
                //}
                //else if (request.OwnerPhone != null)
                //{
                //    account = new Account(request.DisplayName, request.OwnerPhone);

                //}
                //else
                //{
                //    throw new NotImplementedException();
                //}

                dbContext.Add(account);
            }
            else
            {
                account = await dbContext.Set<Account>().FindAsync(requestContext.GetNameIdentifier());
                //account.TenantMemberships.cou 
            }

            account.AddTenantMembership(new TenantMembership(tenant.Id, MembershipType.Owner));

            await dbContext.SaveChangesAsync();

            return new TenantCreateResult
            {
                TenantId = tenant.Id,
                AccountId = account.Id
            };
        }

        private class Validate : AbstractValidator<TenantCreate>
        {
            public Validate(RequestContext requestContext)
            {

                RuleFor(p => p.DisplayName).NotEmpty();
                When(request => requestContext.GetNameIdentifier() == Account.SystemId, () =>
                {
                    RuleFor(p => p.OwnerDisplayName).NotEmpty();
                    RuleFor(p => p.OwnerEmail).NotEmpty();
                    RuleFor(p => p.OwnerPassword).NotEmpty().When(i => i.OwnerEmailProvider == EmailProvider.None);
                    RuleFor(p => p.OwnerPassword).Empty().When(i => i.OwnerEmailProvider != EmailProvider.None);
                });

            }
        }
    }
}
