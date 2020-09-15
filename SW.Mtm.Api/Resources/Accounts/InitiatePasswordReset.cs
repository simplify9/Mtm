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

    [Protect(RequireRole = true)]
    [HandlerName("initiatepasswordreset")]
    class InitiatePasswordReset : ICommandHandler<string, AccountInitiatePasswordReset>
    {
        private readonly MtmDbContext dbContext;

        public InitiatePasswordReset(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(string accountIdOrEmail, AccountInitiatePasswordReset request)
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

            var passwordResetToken = new Domain.PasswordResetToken(account.Id);
            dbContext.Add(passwordResetToken);
            await dbContext.SaveChangesAsync();

            return new AccountInitiatePasswordResetResult
            {
                AccountId = account.Id,
                Token = passwordResetToken.Id
            };
        }
    }
}
