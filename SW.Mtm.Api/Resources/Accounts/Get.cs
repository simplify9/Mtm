using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    class Get : IGetHandler<string>
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Get(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }

        async public Task<object> Handle(string accountIdOrEmail, bool lookup = false)
        {
            var account = await dbContext.FindAsync<Account>(accountIdOrEmail);
            if (account == null)
                account = await dbContext.Set<Account>().Where(i => i.Email == accountIdOrEmail).SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(accountIdOrEmail);

            if (requestContext.GetNameIdentifier() != accountIdOrEmail && requestContext.GetEmail() != accountIdOrEmail && !await dbContext.IsRequesterLandlord())
                throw new SWUnauthorizedException();



            throw new NotImplementedException();
        }
    }
}
