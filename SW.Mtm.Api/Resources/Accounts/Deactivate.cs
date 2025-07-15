
using System.Threading.Tasks;
using SW.Mtm.Domain;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts;


[Protect]
[HandlerName(nameof(Deactivate))]
public class Deactivate:ICommandHandler<string>
{
    private readonly MtmDbContext _dbContext;


    public Deactivate(MtmDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<object> Handle(string key)
    {
        var account = await _dbContext.FindAsync<Account>(key);
        if (account == null)
            throw new SWNotFoundException($"Cant find account with key {key}");
        if (account.Disabled)
            throw new SWException("Account is already deactivate");
        
        account.Activate();

        await _dbContext.SaveChangesAsync();

        return null;
    }
}

