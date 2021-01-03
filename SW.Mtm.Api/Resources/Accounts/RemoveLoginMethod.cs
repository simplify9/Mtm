using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts
{
    [Protect]
    [HandlerName("removelogin")]
    public class RemoveLoginMethod: ICommandHandler<string,RemoveLoginMethodModel>
    {
        private readonly RequestContext requestContext;
        private readonly MtmDbContext dbContext;

        public RemoveLoginMethod(RequestContext requestContext, MtmDbContext dbContext)
        {
            this.requestContext = requestContext;
            this.dbContext = dbContext;
        }
        
        public async Task<object> Handle(string accountIdOrEmail, RemoveLoginMethodModel request)
        {
            var account = await dbContext.FindAsync<Account>(accountIdOrEmail);
            if (account == null)
                account = await dbContext.Set<Account>().Where(i => i.Email == accountIdOrEmail).SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(accountIdOrEmail);

            throw new NotImplementedException();
        }
        
        private class Validate : AbstractValidator<RemoveLoginMethodModel>
        {
            public Validate()
            {
            }
        }
    }
}