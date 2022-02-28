using SW.HttpExtensions;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SW.Mtm.Sdk
{
    public class MtmClient : ApiClientBase<MtmClientOptions>, IMtmClient
    {
        public MtmClient(HttpClient httpClient, RequestContext requestContext, MtmClientOptions mtmClientOptions) :
            base(httpClient, requestContext, mtmClientOptions)
        {
        }

        public async Task<ApiResult<AccountLoginResult>> GenerateOtpAsApiResult(GenerateOtpModel generateAccount)
        {
            return await Builder
                .JwtOrKey()
                .Path("accounts/otp")
                .AsApiResult<AccountLoginResult>()
                .PostAsync(generateAccount);
        }

        public async Task<TenantCreateResult> CreateTenant(TenantCreate tenantCreate)
        {
            return await Builder
                .JwtOrKey()
                .Path("tenants")
                .As<TenantCreateResult>(true)
                .PostAsync(tenantCreate);
        }

        public async Task<ApiResult<TenantCreateResult>> CreateTenantAsApiResult(TenantCreate tenantCreate)
        {
            return await Builder
                .JwtOrKey()
                .Path("tenants")
                .AsApiResult<TenantCreateResult>()
                .PostAsync(tenantCreate);
        }

        public async Task AcceptInvitation(string key, InvitationAccept invitationAccept)
        {
            await Builder
                .Jwt()
                .Path($"invitations/{key}/accept")
                .PostAsync(invitationAccept);
        }

        public async Task<ApiResult> AcceptInvitationAsApiResult(string key, InvitationAccept invitationAccept)
        {
            return await Builder
                .Jwt()
                .Path($"invitations/{key}/accept")
                .AsApiResult()
                .PostAsync(invitationAccept);
        }

        public async Task<TenantInviteResult> Invite(int key, TenantInvite tenantInvite)
        {
            return await Builder
                .Jwt()
                .Path($"tenants/{key}/invite")
                .As<TenantInviteResult>(true)
                .PostAsync(tenantInvite);
        }

        public async Task<ApiResult<TenantInviteResult>> InviteAsApiResult(int key, TenantInvite tenantInvite)
        {
            return await Builder
                .Jwt()
                .Path($"tenants/{key}/invite")
                .AsApiResult<TenantInviteResult>()
                .PostAsync(tenantInvite);
        }

        public async Task<AccountLoginResult> Login(AccountLogin loginAccount)
        {
            return await Builder
                .JwtOrKey()
                .Path("accounts/login")
                .As<AccountLoginResult>(true)
                .PostAsync(loginAccount);
        }

        public async Task<ApiResult<AccountLoginResult>> LoginAsApiResult(AccountLogin loginAccount)
        {
            return await Builder
                .JwtOrKey()
                .Path("accounts/login")
                .AsApiResult<AccountLoginResult>()
                .PostAsync(loginAccount);
        }

        public async Task<AccountCreateResult> CreateAccount(AccountCreate registerAccount)
        {
            return await Builder
                .Key()
                .Path("accounts")
                .As<AccountCreateResult>(true)
                .PostAsync(registerAccount);
        }

        public async Task<ApiResult<AccountCreateResult>> CreateAccountAsApiResult(AccountCreate registerAccount)
        {
            return await Builder
                .Key()
                .Path("accounts")
                .AsApiResult<AccountCreateResult>()
                .PostAsync(registerAccount);
        }

        public async Task ChangePassword(AccountChangePassword changePasswordAccount)
        {
            await Builder
                .Jwt()
                .Path($"accounts/changepassword")
                .PostAsync(changePasswordAccount, true);
        }

        public async Task<ApiResult> ChangePasswordAsApiResult(AccountChangePassword accountChangePassword)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/changepassword")
                .AsApiResult()
                .PostAsync(accountChangePassword);
        }

        public async Task ResetPassword(string accountIdOrEmail, AccountResetPassword accountResetPassword)
        {
            await Builder
                .Key()
                .Path($"accounts/{accountIdOrEmail}/resetpassword")
                .PostAsync(accountResetPassword, true);
        }

        public async Task<ApiResult> ResetPasswordAsApiResult(string accountIdOrEmail,
            AccountResetPassword accountResetPassword)
        {
            return await Builder
                .Key()
                .Path($"accounts/{accountIdOrEmail}/resetpassword")
                .AsApiResult()
                .PostAsync(accountResetPassword);
        }

        public async Task<AccountInitiatePasswordResetResult> InitiatePasswordReset(string accountIdOrEmail)
        {
            return await Builder
                .Key()
                .Path($"accounts/{accountIdOrEmail}/initiatepasswordreset")
                .As<AccountInitiatePasswordResetResult>(true)
                .PostAsync(new AccountInitiatePasswordReset());
        }

        public async Task<ApiResult<AccountInitiatePasswordResetResult>> InitiatePasswordResetAsApiResult(
            string accountIdOrEmail)
        {
            return await Builder
                .Key()
                .Path($"accounts/{accountIdOrEmail}/initiatepasswordreset")
                .AsApiResult<AccountInitiatePasswordResetResult>()
                .PostAsync(new AccountInitiatePasswordReset());
        }

        public async Task<ApiResult<List<InvitationSearchResult>>> SearchInvitationsAsApiResult(
            InvitationSearch invitationSearch)
        {
            return await Builder
                .Jwt()
                .Path($"invitations?email={invitationSearch.Email}")
                .AsApiResult<List<InvitationSearchResult>>()
                .GetAsync();
        }

        public async Task<ApiResult> SetAsTenantOwner(AccountSetAsTenantOwner request)
        {
            return await Builder
                .Key()
                .Path($"accounts/setastenantowner")
                .AsApiResult()
                .PostAsync(request);
        }

        public async Task<ApiResult> SetAsLandlord(AccountSetAsLandlord request)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/setaslandlord")
                .AsApiResult()
                .PostAsync(request);
        }

        public async Task<ApiResult<InvitationGet>> GetInvitationAsApiResult(string key)
        {
            return await Builder
                .Key()
                .Path($"invitations/{key}")
                .AsApiResult<InvitationGet>()
                .GetAsync();
        }

        public async Task<ApiResult> CancelInvitationAsApiResult(InvitationCancel invitationCancel)
        {
            return await Builder
                .Jwt()
                .Path($"invitations/cancel")
                .AsApiResult()
                .PostAsync(invitationCancel);
        }

        public async Task<ApiResult<AccountLoginResult>> SwitchTenantAsApiResult(
            AccountSwitchTenant accountSwitchTenant)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/switchtenant")
                .AsApiResult<AccountLoginResult>()
                .PostAsync(accountSwitchTenant);
        }

        public async Task<ApiResult> RemoveAccountAsApiResult(int key, TenantRemoveAccount tenantRemoveAccount)
        {
            return await Builder
                .Jwt()
                .Path($"tenants/{key}/removeaccount")
                .AsApiResult()
                .PostAsync(tenantRemoveAccount);
        }

        public async Task<ApiResult> TenantAddAccountAsApiResult(int key, TenantAddAccount tenantAddAccount)
        {
            return await Builder
                .JwtOrKey()
                .Path($"tenants/{key}/addaccount")
                .AsApiResult()
                .PostAsync(tenantAddAccount);
        }

        public async Task<ApiResult> SetProfileDataAsApiResult(string accountIdOrEmail,
            AccountSetProfileData accountSetProfileData)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/{accountIdOrEmail}/setprofiledata")
                .AsApiResult()
                .PostAsync(accountSetProfileData);
        }

        public async Task<ApiResult> UpdateProfileAsApiResult(string accountId, UpdateAccountModel model)
        {
            return await Builder
                .JwtOrKey()
                .Path($"accounts/{accountId}/update")
                .AsApiResult()
                .PostAsync(model);
        }

        public async Task<ApiResult> ResetTotpAsApiResult(string accountId, ResetTotopAccountModel model)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/{accountId}/resetTotp")
                .AsApiResult()
                .PostAsync(model);
        }

        public async Task<ApiResult<AccountGet>> GetAccountAsApiResult(string accountIdOrEmail)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/{accountIdOrEmail}")
                .AsApiResult<AccountGet>()
                .GetAsync();
        }

        public async Task<ApiResult<SearchyResponse<AccountGet>>> SearchAccountsAsApiResult(string searchUrl)
        {
            return await Builder
                .Jwt()
                .Path($"accounts?{searchUrl}")
                .AsApiResult<SearchyResponse<AccountGet>>()
                .GetAsync();
        }


        public async Task<ApiResult<List<AccountGet>>> LegacySearchAccountsAsApiResult(SearchAccounts request)
        {
            return await Builder
                .Jwt()
                .Path("accounts/LegacySearch")
                .AsApiResult<List<AccountGet>>()
                .GetAsync(request);
        }

        public async Task<ApiResult<List<AccountGet>>> LegacySearchAccountsPostAsApiResult(SearchAccounts request)
        {
            return await Builder
                .Jwt()
                .Path("accounts/LegacySearchPost")
                .AsApiResult<List<AccountGet>>()
                .GetAsync(request);
        }

        public async Task<ApiResult<AddLoginMethodResult>> AddLoginMethodAsApiResult(string accountIdOrEmail,
            AddLoginMethodModel request)
        {
            return await Builder
                .JwtOrKey()
                .Path($"accounts/{accountIdOrEmail}/addlogin")
                .AsApiResult<AddLoginMethodResult>()
                .PostAsync(request);
        }

        public async Task<ApiResult<AccountSetupTotpResult>> SetupOtpSecret(AccountSetupOtpRequest request)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/setupotpsecret")
                .AsApiResult<AccountSetupTotpResult>()
                .PostAsync(request);
        }

        public async Task<ApiResult> RemoveLoginMethodAsApiResult(string accountIdOrEmail,
            RemoveLoginMethodModel request)
        {
            return await Builder
                .JwtOrKey()
                .Path($"accounts/{accountIdOrEmail}/removelogin")
                .AsApiResult<ApiResult>()
                .PostAsync(request);
        }
    }
}