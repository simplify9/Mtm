using FluentValidation;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{

    [Protect(RequireRole = true)]
    [HandlerName("resetpassword")]
    class ResetPassword : ICommandHandler<string, AccountResetPassword>
    {
        private readonly MtmDbContext dbContext;

        public ResetPassword(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(string key, AccountResetPassword request)
        {

            var account = await dbContext.FindAsync<Account>(key);

            if (account == null)
                throw new SWNotFoundException(key);

            if (account.EmailProvider != EmailProvider.None)
                throw new SWValidationException("EmailProvider", "Not allowed for external email providers.");

            if ((account.LoginMethods & LoginMethod.EmailAndPassword) != LoginMethod.EmailAndPassword)
                throw new SWValidationException("LoginMethod", "Invalid login method.");

            account.SetPassword(SecurePasswordHasher.Hash(request.NewPassword));

            await dbContext.SaveChangesAsync();

            return null;
        }

        class Validate : AbstractValidator<AccountResetPassword>
        {
            public Validate()
            {
                RuleFor(i => i.NewPassword).NotEmpty();
            }
        }
    }
}
