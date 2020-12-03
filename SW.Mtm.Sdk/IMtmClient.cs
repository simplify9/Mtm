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
        Task<TenantCreateResult> CreateTenant(TenantCreate registerAccount);
        Task<ApiResult<TenantCreateResult>> CreateTenantAsApiResult(TenantCreate registerAccount);
        //Task<TenantCreateResult> CreateAdditionalTenant(TenantCreate tenantCreate);
        //Task<ApiResult<TenantCreateResult>> CreateAdditionalTenantAsApiResult(TenantCreate tenantCreate);


        Task AcceptInvitation(string key, InvitationAccept invitationAccept);
        Task<ApiResult> AcceptInvitationAsApiResult(string key, InvitationAccept invitationAccept);

        Task<TenantInviteResult> Invite(int key, TenantInvite tenantInvite);
        Task<ApiResult<TenantInviteResult>> InviteAsApiResult(int key, TenantInvite tenantInvite);

        Task<ApiResult<List<InvitationSearchResult>>> SearchInvitationsAsApiResult(InvitationSearch invitationSearch);
        Task<ApiResult> CancelInvitationAsApiResult(InvitationCancel invitationCancel);
        Task<ApiResult<AccountLoginResult>> SwitchTenantAsApiResult(AccountSwitchTenant accountSwitchTenant);
        Task<ApiResult> RemoveAccountAsApiResult(int key, TenantRemoveAccount tenantRemoveAccount);
    }
}
