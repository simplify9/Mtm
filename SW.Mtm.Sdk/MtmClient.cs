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
        async public Task<TenantCreateResult> CreateTenant(TenantCreate tenantCreate)
        {
            return await Builder
               .Key()
               .Path("tenants/create")
               .As<TenantCreateResult>(true)
               .PostAsync(tenantCreate);
        }

        async public Task<TenantCreateResult> CreateAdditionalTenant(TenantCreate tenantCreate)
        {
            return await Builder
               .Jwt()
               .Path("tenants/create")
               .As<TenantCreateResult>(true)
               .PostAsync(tenantCreate);
        }

        async public Task<ApiResult<AccountLoginResult>> LoginAsApiResult(AccountLogin loginAccount)
        {
            return await Builder
                .Key()
                .Path("accounts/login")
                .AsApiResult<AccountLoginResult>()
                .PostAsync(loginAccount);
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

        async public Task ChangePassword(AccountChangePassword changePasswordAccount)
        {
            await Builder
               .Jwt()
               .Path($"accounts/changepassword")
               .PostAsync(changePasswordAccount, true);
        }

        async public Task ResetPassword(string accountIdOrEmail, AccountResetPassword accountResetPassword)
        {
            await Builder
               .Key()
               .Path($"accounts/{accountIdOrEmail}/resetpassword")
               .PostAsync(accountResetPassword, true);
        }

        async public Task<AccountInitiatePasswordResetResult> InitiatePasswordReset(string accountIdOrEmail)
        {
            return await Builder
               .Key()
               .Path($"accounts/{accountIdOrEmail}/initiatepasswordreset")
               .As<AccountInitiatePasswordResetResult>(true)
               .PostAsync(new AccountInitiatePasswordReset());
        }
    }
}
