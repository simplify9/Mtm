using Microsoft.Extensions.Configuration;
using SW.HttpExtensions;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SW.Mtm.Sdk
{
    public class MtmClient : ApiClientBase<MtmClientOptions>, IMtmClient
    {
        public MtmClient(HttpClient httpClient, RequestContext requestContext, MtmClientOptions mtmClientOptions) : base(httpClient, requestContext, mtmClientOptions)
        {
        }
        async public Task CreateTenant(TenantCreate tenantCreate)
        {
            await Builder
               .Key()
               .Path("tenants/create")
               .PostAsync(tenantCreate, true);
        }

        async public Task CreateAdditionalTenant(TenantCreate tenantCreate)
        {
            await Builder
               .Jwt()
               .Path("tenants/create")
               .PostAsync(tenantCreate, true);
        }

        async public Task<AccountLoginResult> Login(AccountLogin loginAccount)
        {
            return await Builder
                .Key()
                .Path("accounts/login")
                .As<AccountLoginResult>(true)
                .PostAsync(loginAccount);
        }

        public Task<AccountRegisterResult> Register(AccountRegister registerAccount)
        {
            throw new NotImplementedException();
        }

    }
}
