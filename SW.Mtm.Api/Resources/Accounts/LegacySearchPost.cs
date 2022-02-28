using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("LegacySearchPost")]
    public class LegacySearchPost : ICommandHandler<SearchAccounts>
    {
        private readonly MtmDbContext _dbContext;

        public LegacySearchPost(MtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<object> Handle(SearchAccounts request)
        {
            var accounts = await _dbContext.Set<Account>()
                .Where(a => request.EmailContains == null || a.Email.Contains(request.EmailContains))
                .Where(a => request.PhoneContains == null || a.Phone.Contains(request.PhoneContains))
                .Where(a => request.Ids == null || request.Ids.Length == 0 || request.Ids.Any(i => i == a.Id))
                .ToListAsync();

            if (accounts == null)
                throw new SWNotFoundException();

            if (!await _dbContext.IsRequesterLandlord() &&
                !await _dbContext.IsRequesterTenantOwner())
                throw new SWUnauthorizedException();

            return accounts.Select(account => new AccountGet
            {
                Id = account.Id,
                Email = account.Email,
                Disabled = account.Disabled,
                Phone = account.Phone,
                Roles = account.Roles,
                CreatedOn = account.CreatedOn,
                DisplayName = account.DisplayName,
                EmailProvider = account.EmailProvider,
                LoginMethods = account.LoginMethods,
                ProfileData = account.ProfileData?.ToList(),
                SecondFactorMethod = account.SecondFactorMethod
            }).ToList();
        }
    }
}