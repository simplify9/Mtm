﻿using FluentValidation;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [Protect]
    [HandlerName("changepassword")]
    class ChangePassword : ICommandHandler<AccountChangePassword,object>
    {
        private readonly RequestContext requestContext;
        private readonly MtmDbContext dbContext;

        public ChangePassword(RequestContext requestContext, MtmDbContext dbContext)
        {
            this.requestContext = requestContext;
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(AccountChangePassword request)
        {

            var accountId = requestContext.GetNameIdentifier();

            var account = await dbContext.FindAsync<Account>(accountId);
            if (account == null)
                throw new SWNotFoundException(accountId);
            if (account.Disabled)
                throw new SWValidationException("DisabledAccount", "Not allowed for disabled account");

            if (account.EmailProvider != EmailProvider.None)
                throw new SWValidationException("EmailProvider", "Not allowed for external email providers.");
            
            if ((account.LoginMethods & LoginMethod.EmailAndPassword) != LoginMethod.EmailAndPassword)
                throw new SWValidationException("LoginMethod", "Invalid login method.");

            var passwordCheck = SecurePasswordHasher.Verify(request.CurrentPassword, account.Password);

            if (!passwordCheck)
                throw new SWUnauthorizedException();

            account.SetPassword(SecurePasswordHasher.Hash(request.NewPassword));

            await dbContext.SaveChangesAsync();

            return null;

        }

        class Validate : AbstractValidator<AccountChangePassword>
        {
            public Validate()
            {
                RuleFor(i => i.CurrentPassword).NotEmpty();
                RuleFor(i => i.NewPassword).NotEmpty();
            }
        }
    }
}
