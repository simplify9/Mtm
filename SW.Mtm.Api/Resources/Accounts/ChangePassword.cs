using FluentValidation;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [Protect]
    class ChangePassword : ICommandHandler<string, AccountChangePassword>
    {
        private readonly RequestContext requestContext;
        private readonly MtmDbContext dbContext;

        public ChangePassword(RequestContext requestContext, MtmDbContext dbContext)
        {
            this.requestContext = requestContext;
            this.dbContext = dbContext;
        }

        async public Task<object> Handle(string key, AccountChangePassword request)
        {

            var accountId = requestContext.GetNameIdentifier();

            if (accountId != key)
                throw new SWUnauthorizedException();




            //
            var account = await dbContext.FindAsync<Account>(key);
            if (account == null)
                throw new SWNotFoundException(key);

            if (account.EmailProvider != EmailProvider.None)
                throw new SWValidationException("EmailProvider", "Not allowed for external email providers.");

            if (account.LoginMethods & LoginMethod.EmailAndPassword)



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
