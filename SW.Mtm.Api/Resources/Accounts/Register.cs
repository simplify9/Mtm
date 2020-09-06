using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    [HandlerName("register")]
    [Protect(RequireRole = true)]
    class Register : ICommandHandler<AccountRegister>
    {
        private readonly MtmDbContext dbContext;

        public Register(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(AccountRegister request)
        {
            string apiKey = null;
            Account account;

            if (request.Email != null)
            {
                if (request.EmailProvider == EmailProvider.None)
                    account = new Account(request.DisplayName, request.Email, SecurePasswordHasher.Hash(request.Password));
                else
                    account = new Account(request.DisplayName, request.Email, request.EmailProvider);
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

            dbContext.Add(account);
            await dbContext.SaveChangesAsync();

            return new AccountRegisterResult
            {
                Id = account.Id,
                Key = apiKey
            };
        }

        private class Validate : AbstractValidator<AccountRegister>
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




            }
        }

    }
}
