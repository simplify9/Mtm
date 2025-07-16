using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("setAsLandlord")]
    [Protect]
    public class SetAsLandlord : ICommandHandler<AccountSetAsLandlord,object>
    {
        
        private readonly MtmDbContext dbContext;
        public SetAsLandlord(MtmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<object> Handle(AccountSetAsLandlord request)
        {
            if (!await dbContext.IsRequesterLandlord())
                throw new SWUnauthorizedException();

            var account = await dbContext
               .Set<Account>()
               .Where(u => u.Id == request.AccountId)
               .SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(request.AccountId);
            account.SetLandlord(true);
            await dbContext.SaveChangesAsync();
           
            return null;
        }
    }
}