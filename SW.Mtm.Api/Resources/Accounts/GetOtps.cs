using System;
using Microsoft.EntityFrameworkCore;
using SW.Mtm.Domain;
using SW.PrimitiveTypes;
using System.Linq;
using System.Threading.Tasks;
using SW.Mtm.Model;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("GetOtps")]
    class GetOtps : IQueryHandler<GetOtpsModel>
    {
        private readonly MtmDbContext _dbContext;

        public GetOtps(MtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<object> Handle(GetOtpsModel request)
        {
            var accountQuery = _dbContext.Set<Account>();

            var account = await accountQuery.FirstOrDefaultAsync(i => i.Phone == request.Phone);

            if (account == null)
                throw new SWNotFoundException(request.Phone);

            var from = DateTime.UtcNow.AddMinutes(-request.NumberOfMinutes);
            var otps = await _dbContext.Set<OtpToken>()
                .Where(x => x.AccountId == account.Id && x.CreatedOn > from)
                .ToListAsync();

            return otps.Select(x => new GetOtpsResponseModel
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
            });
        }
    }
}
