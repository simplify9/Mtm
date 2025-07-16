using System.Threading.Tasks;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts
{
    [Protect]
    [HandlerName(nameof(Activate))]
    public class Activate:ICommandHandler<string,ActivateDeactivateOptions>
    {
        private readonly MtmDbContext _dbContext;


        public Activate(MtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<object> Handle(string key,ActivateDeactivateOptions request)
        {
            var account = await _dbContext.FindAsync<Account>(key);
            if (account == null)
                throw new SWNotFoundException($"Cant find account with key {key}");
            if (!account.Disabled)
                throw new SWException("Account is already active");
        
            account.Deactivate();

            await _dbContext.SaveChangesAsync();

            return null;
        }
    }
}