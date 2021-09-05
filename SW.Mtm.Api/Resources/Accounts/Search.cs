using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SW.EfCoreExtensions;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;

namespace SW.Mtm.Resources.Accounts
{
    public class Search: ISearchyHandler
    {
        private readonly MtmDbContext dbContext;
        private readonly RequestContext requestContext;

        public Search(MtmDbContext dbContext, RequestContext requestContext)
        {
            this.dbContext = dbContext;
            this.requestContext = requestContext;
        }
        // //ToDo turn it to searchy
        // public async Task<object> Handle(SearchAccounts request)
        // {
        //     var accounts = await dbContext.Set<Account>()
        //         .Where(a => request.EmailContains == null || a.Email.Contains(request.EmailContains))
        //         .Where(a => request.PhoneContains == null || a.Phone.Contains(request.PhoneContains))
        //         .Where(a => request.Ids == null || request.Ids.Length == 0 || request.Ids.Any(i => i == a.Id))
        //         .ToListAsync();
        //
        //     if (accounts == null)
        //         throw new SWNotFoundException();
        //
        //     if (!await dbContext.IsRequesterLandlord() &&
        //         !await dbContext.IsRequesterTenantOwner())
        //         throw new SWUnauthorizedException();
        //
        //     return accounts.Select(account => new AccountGet
        //     {
        //         Id = account.Id,
        //         Email = account.Email,
        //         Disabled = account.Disabled,
        //         Phone = account.Phone,
        //         Roles = account.Roles,
        //         CreatedOn = account.CreatedOn,
        //         DisplayName = account.DisplayName,
        //         EmailProvider = account.EmailProvider,
        //         LoginMethods = account.LoginMethods,
        //         ProfileData = account.ProfileData?.ToList(),
        //         SecondFactorMethod = account.SecondFactorMethod
        //     }).ToList();
        //
        // }

        public async Task<object> Handle(SearchyRequest searchyRequest, bool lookup = false, string searchPhrase = null)
        {
            var query = from account in dbContext.Set<Account>()
                select new AccountGet
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
                    ProfileData = account.ProfileData,
                    SecondFactorMethod = account.SecondFactorMethod
                };

            return new SearchyResponse<AccountGet>
            {
                TotalCount = await query.Search(searchyRequest.Conditions).CountAsync(),
                Result = await query.Search(searchyRequest.Conditions, searchyRequest.Sorts, searchyRequest.PageSize, searchyRequest.PageIndex).ToListAsync()
            };
        }
    }
}