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
    [HandlerName("addlogin")]
    public class AddLoginMethod: ICommandHandler<string, AddLoginMethodModel,object>
    {
        private readonly RequestContext requestContext;
        private readonly MtmDbContext dbContext;

        public AddLoginMethod(RequestContext requestContext, MtmDbContext dbContext)
        {
            this.requestContext = requestContext;
            this.dbContext = dbContext;
        }
        
        public async Task<object> Handle(string accountIdOrEmail,AddLoginMethodModel request)
        {
            var account = await dbContext.FindAsync<Account>(accountIdOrEmail) ??
                          await dbContext.Set<Account>().Where(i => i.Email == accountIdOrEmail).SingleOrDefaultAsync();

            if (account == null)
                throw new SWNotFoundException(accountIdOrEmail);

            if (requestContext.GetNameIdentifier() != accountIdOrEmail &&
                requestContext.GetEmail() != accountIdOrEmail && !await dbContext.IsRequesterLandlord()
                && !await dbContext.IsRequesterTenantOwner())
                throw new SWUnauthorizedException();
            
            string apiKey = null;
            
            if (request.Email != null)
            {
                if (request.EmailProvider == EmailProvider.None)
                    account.AddEmailLoginMethod(request.Email, SecurePasswordHasher.Hash(request.Password));
                else
                    account.AddEmailLoginMethod(request.Email, request.EmailProvider);
            }
            else if (request.Phone != null)
            {
                account.AddPhoneLoginMethod(request.Phone);

            }
            else if (request.CredentialName != null)
            {
                apiKey = Guid.NewGuid().ToString("N");
                account.AddApiKeyLoginMethod(new ApiCredential(request.CredentialName, apiKey));
               
            }
            else
            {
                throw new NotImplementedException();
            }

            await dbContext.SaveChangesAsync();

            return new AddLoginMethodResult()
            {
                Key = apiKey
            };




        }

        private class Validate : AbstractValidator<AddLoginMethodModel>
        {
            public Validate()
            {
            }
        }
    }
}