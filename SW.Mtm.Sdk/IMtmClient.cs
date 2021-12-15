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
        Task<AccountCreateResult> CreateAccount(AccountCreate registerAccount);
        Task<ApiResult<AccountCreateResult>> CreateAccountAsApiResult(AccountCreate registerAccount);
        Task<ApiResult<AccountLoginResult>> GenerateOtpAsApiResult(GenerateOtpModel generateAccount);
        Task<TenantCreateResult> CreateTenant(TenantCreate registerAccount);

        Task<ApiResult<TenantCreateResult>> CreateTenantAsApiResult(TenantCreate registerAccount);
        //Task<TenantCreateResult> CreateAdditionalTenant(TenantCreate tenantCreate);
        //Task<ApiResult<TenantCreateResult>> CreateAdditionalTenantAsApiResult(TenantCreate tenantCreate);


        Task AcceptInvitation(string key, InvitationAccept invitationAccept);
        Task<ApiResult> AcceptInvitationAsApiResult(string key, InvitationAccept invitationAccept);

        Task<TenantInviteResult> Invite(int key, TenantInvite tenantInvite);
        Task<ApiResult<TenantInviteResult>> InviteAsApiResult(int key, TenantInvite tenantInvite);
        Task<ApiResult<InvitationGet>> GetInvitationAsApiResult(string key);
        Task<ApiResult<List<InvitationSearchResult>>> SearchInvitationsAsApiResult(InvitationSearch invitationSearch);
        Task<ApiResult> CancelInvitationAsApiResult(InvitationCancel invitationCancel);
        Task<ApiResult> SetAsTenantOwner(AccountSetAsTenantOwner request);
        Task<ApiResult<AccountLoginResult>> SwitchTenantAsApiResult(AccountSwitchTenant accountSwitchTenant);
        Task<ApiResult> RemoveAccountAsApiResult(int key, TenantRemoveAccount tenantRemoveAccount);
        Task<ApiResult> TenantAddAccountAsApiResult(int key, TenantAddAccount tenantAddAccount);
        Task<ApiResult> SetProfileDataAsApiResult(string accountIdOrEmail, AccountSetProfileData accountSetProfileData);
        Task<ApiResult> UpdateProfileAsApiResult(string accountId, UpdateAccountModel model);

        Task<ApiResult<AccountGet>> GetAccountAsApiResult(string accountIdOrEmail);
        Task<ApiResult<SearchyResponse<AccountGet>>> SearchAccountsAsApiResult(string searchUrl);
        Task<ApiResult<List<AccountGet>>> LegacySearchAccountsAsApiResult(SearchAccounts request);

        Task<ApiResult<AccountSetupTotpResult>> SetupOtpSecret(AccountSetupOtpRequest request);

        Task<ApiResult<AddLoginMethodResult>> AddLoginMethodAsApiResult(string accountIdOrEmail,
            AddLoginMethodModel request);

        Task<ApiResult> RemoveLoginMethodAsApiResult(string accountIdOrEmail, RemoveLoginMethodModel request);
    }
}