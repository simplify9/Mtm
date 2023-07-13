using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
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

        async public Task<object> Handle(string accountIdOrEmail, AccountResetPassword request)
        {

            var account = await dbContext.FindAsync<Account>(accountIdOrEmail);
            if (account == null)
                account = await dbContext.Set<Account>().Where(i => i.Email == accountIdOrEmail).SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(accountIdOrEmail);

            if (account.EmailProvider != EmailProvider.None)
                throw new SWValidationException("EmailProvider", "Not allowed for external email providers.");

            if ((account.LoginMethods & LoginMethod.EmailAndPassword) != LoginMethod.EmailAndPassword)
                throw new SWValidationException("LoginMethod", "Invalid login method.");

            var token = await dbContext.FindAsync<PasswordResetToken>(request.Token);
            if (token == null)
                throw new SWUnauthorizedException();


            account.SetPassword(SecurePasswordHasher.Hash(request.NewPassword));

            dbContext.Remove(token);

            await dbContext.SaveChangesAsync();

            return null;
        }

        class Validate : AbstractValidator<AccountResetPassword>
        {
            public Validate()
            {
                RuleFor(i => i.Token).NotEmpty();
                RuleFor(i => i.NewPassword).NotEmpty();
            }
        }
    }
}
