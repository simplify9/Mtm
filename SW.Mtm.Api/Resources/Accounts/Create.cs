using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    
    [Protect(RequireRole = true)]
    class Create : ICommandHandler<AccountCreate>
    {
        private readonly MtmDbContext dbContext;

        public Create(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(AccountCreate request)
        {
            string apiKey = null;
            Account account;

            if (request.Email != null)
            {
                if (request.EmailProvider == EmailProvider.None)
                    if(request.SecondFactorMethod == OtpType.None)
                        account = new Account(request.DisplayName, request.Email, SecurePasswordHasher.Hash(request.Password));
                    else
                        account = new Account(request.DisplayName, request.Email, SecurePasswordHasher.Hash(request.Password), request.SecondFactorMethod);

                else
                    if (request.SecondFactorMethod == OtpType.None)
                        account = new Account(request.DisplayName, request.Email, request.EmailProvider);
                    else
                        account = new Account(request.DisplayName, request.Email, request.EmailProvider, request.SecondFactorMethod);

            }
            else if (request.Phone != null)
            {
                account = new Account(request.DisplayName, request.Phone);

            }
            else if (request.CredentialName != null)
            {
                apiKey = Guid.NewGuid().ToString("N");
                account = new Account(request.DisplayName, new ApiCredential(request.CredentialName, apiKey));
            }
            else
            {
                throw new NotImplementedException();
            }
            
            // if (request.ProfileData != null)
            //     account.ProfileData = request.ProfileData.ToList();

            dbContext.Add(account);
            await dbContext.SaveChangesAsync();

            return new AccountCreateResult
            {
                Id = account.Id,
                Key = apiKey
            };
        }

        private class Validate : AbstractValidator<AccountCreate>
        {
            public Validate(MtmDbContext mtmDbContext)
            {
                RuleFor(p => p.DisplayName).NotEmpty();

                RuleFor(p => p.Email).Null().When(p => p.Phone != null || p.CredentialName != null) ;
                RuleFor(p => p.Phone).Null().When(p => p.Email != null || p.CredentialName != null);
                RuleFor(p => p.CredentialName).Null().When(p => p.Phone != null || p.Email != null);

                RuleFor(p => p.Email).NotEmpty().When(p => p.Phone == null && p.CredentialName == null);
                RuleFor(p => p.Phone).NotEmpty().When(p => p.Email == null && p.CredentialName == null);
                RuleFor(p => p.CredentialName).NotEmpty().When(p => p.Phone == null && p.Email == null);


                RuleFor(p => p.Email).EmailAddress();

                RuleFor(p => p.Email).CustomAsync(async (value, context, cancellationToken) =>
                {
                    if (await mtmDbContext.Set<Account>().AnyAsync(p => p.Email == value && p.Email != null))
                        context.AddFailure("Account exists.");
                });

                RuleFor(p => p.Phone).CustomAsync(async (value, context, cancellationToken) =>
                {
                    if (await mtmDbContext.Set<Account>().AnyAsync(p => p.Phone == value && p.Phone != null))
                        context.AddFailure("Account exists.");
                });


                RuleFor(p => p.Password).Null().When(p =>
                {
                    return p.Phone != null || p.CredentialName != null;
                });

                RuleFor(p => p.Password).NotEmpty().MinimumLength(5).When(p =>
                {
                    return p.EmailProvider == EmailProvider.None && p.Email != null;
                });
                
                RuleForEach(p => p.ProfileData).
                    ChildRules(items =>
                    {
                        items.RuleFor(i => i.Name).NotEmpty();
                        items.RuleFor(i => i.Name).NotEqual(ClaimTypes.Role, StringComparer.OrdinalIgnoreCase);
                        items.RuleFor(i => i.Value).NotEmpty();
                    }).
                    When(p => p.ProfileData != null);
            }
        }

    }
}
