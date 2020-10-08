using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Sdk
{
    public interface IMtmClient
    {
        Task<AccountLoginResult> Login(AccountLogin loginAccount);
        Task<ApiResult<AccountLoginResult>> LoginAsApiResult(AccountLogin loginAccount);
        Task ChangePassword(AccountChangePassword accountChangePassword);
        Task<ApiResult> ChangePasswordAsApiResult(AccountChangePassword accountChangePassword);
        Task<AccountInitiatePasswordResetResult> InitiatePasswordReset(string accountIdOrEmail);
        Task<ApiResult<AccountInitiatePasswordResetResult>> InitiatePasswordResetAsApiResult(string accountIdOrEmail);
        Task ResetPassword(string accountIdOrEmail, AccountResetPassword accountResetPassword);
        Task<ApiResult> ResetPasswordAsApiResult(string accountIdOrEmail, AccountResetPassword accountResetPassword);
        Task<AccountRegisterResult> Register(AccountRegister registerAccount);
        Task<ApiResult<AccountRegisterResult>> RegisterAsApiResult(AccountRegister registerAccount);
        Task<TenantCreateResult> CreateTenant(TenantCreate registerAccount);
        Task<ApiResult<TenantCreateResult>> CreateTenantAsApiResult(TenantCreate registerAccount);
        Task<TenantCreateResult> CreateAdditionalTenant(TenantCreate tenantCreate);
        Task<ApiResult<TenantCreateResult>> CreateAdditionalTenantAsApiResult(TenantCreate tenantCreate);

    }
}
