using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts
{
    [Protect]
    [HandlerName("update")]
    public class Update : ICommandHandler<string, UpdateAccountModel>
    {
        private readonly MtmDbContext _dbContext;
        private readonly RequestContext _requestContext;


        public Update(MtmDbContext dbContext, RequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task<object> Handle(string key, UpdateAccountModel request)
        {
            var account = await _dbContext.FindAsync<Account>(key);

            if (account == null)
                throw new SWNotFoundException($"Cant find account with key {key}");

            if ((account.TenantId == null || !await _dbContext.IsRequesterTenantOwner(account.TenantId.Value)) &&
                !await _dbContext.IsRequesterLandlord())
            {
                throw new SWUnauthorizedException();
            }

            account.Update(request.Email);

            await _dbContext.SaveChangesAsync();

            return null;
        }

        private class Validate : AbstractValidator<UpdateAccountModel>
        {
            public Validate()
            {
                RuleFor(p => p.Email).NotEmpty();
            }
        }
    }
}