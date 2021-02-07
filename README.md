| **Package**       | **Version** |
| :----------------:|:----------------------:|
| [`SimplyWorks.MTM`](https://www.nuget.org/packages/SimplyWorks.Mtm.Sdk/)|![Nuget](https://img.shields.io/nuget/v/SimplyWorks.Mtm.Sdk?style=for-the-badge)|

![License](https://img.shields.io/badge/license-MIT-blue.svg)

## Introduction 
_MTM_ is a multi-tenant management system that handles setting up users, authenticating them, and authorizing them for program access. 

As the owner of a tenant, _MTM_ allows for you to invite an employee, making them users under your tenancy. 
To authenticate a user, _MTM_ returns a [JSON Web Token (JWT)](https://jwt.io), identifying the user's permissions. 

## MTM SDK
The [MTM Client](https://github.com/simplify9/Mtm/blob/master/SW.Mtm.Sdk/MtmClient.cs) outlines the steps the user takes to enable _MTM_ onto the program. 
 
 #### Creating the tenant 
 ```cpp
 async public Task<TenantCreateResult> CreateTenant(TenantCreate tenantCreate)
        {
            return await Builder
               .JwtOrKey()
               .Path("tenants")
               .As<TenantCreateResult>(true)
               .PostAsync(tenantCreate);
        }
```
```cpp 
  public async Task<ApiResult<TenantCreateResult>> CreateTenantAsApiResult(TenantCreate tenantCreate)
        {
            return await Builder
               .JwtOrKey()
               .Path("tenants")
               .AsApiResult<TenantCreateResult>()
               .PostAsync(tenantCreate);
        }        
```
#### Accepting the invitation 
```cpp
        public async Task AcceptInvitation(string key, InvitationAccept invitationAccept)
        {
            await Builder
                .Jwt()
                .Path($"invitations/{key}/accept")
                .PostAsync(invitationAccept);
        }
```
```cpp
 public async Task<ApiResult> AcceptInvitationAsApiResult(string key, InvitationAccept invitationAccept)
        {
            return await Builder
                .Jwt()
                .Path($"invitations/{key}/accept")
                .AsApiResult()
                .PostAsync(invitationAccept);
        }
```
#### Inviting user to tenancy 
```cpp
 public async Task<TenantInviteResult> Invite(int key, TenantInvite tenantInvite)
        {
            return await Builder
                .Jwt()
                .Path($"tenants/{key}/invite")
                .As<TenantInviteResult>(true)
                .PostAsync(tenantInvite);
        }
```      
```cpp
public async Task<ApiResult<TenantInviteResult>> InviteAsApiResult(int key, TenantInvite tenantInvite)
        {
            return await Builder
                .Jwt()
                .Path($"tenants/{key}/invite")
                .AsApiResult<TenantInviteResult>()
                .PostAsync(tenantInvite);
        }
```
#### LOGIN prompted
```cpp
 async public Task<AccountLoginResult> Login(AccountLogin loginAccount)
        {
            return await Builder
                .JwtOrKey()
                .Path("accounts/login")
                .As<AccountLoginResult>(true)
                .PostAsync(loginAccount);
        }
```       
#### LOGIN inputted on user's end
```cpp
public async Task<ApiResult<AccountLoginResult>> LoginAsApiResult(AccountLogin loginAccount)
        {
            return await Builder
                .JwtOrKey()
                .Path("accounts/login")
                .AsApiResult<AccountLoginResult>()
                .PostAsync(loginAccount);
        }
```
#### MTM Then creates an account for the user
```cpp
public async Task<AccountCreateResult> CreateAccount(AccountCreate registerAccount)
        {
            return await Builder
                .Key()
                .Path("accounts")
                .As<AccountCreateResult>(true)
                .PostAsync(registerAccount);
        }
```
```cpp
 public async Task<ApiResult<AccountCreateResult>> CreateAccountAsApiResult(AccountCreate registerAccount)
        {
            return await Builder
                .Key()
                .Path("accounts")
                .AsApiResult<AccountCreateResult>()
                .PostAsync(registerAccount);
        }
```
#### Changing password on user's end:
```cpp
async public Task ChangePassword(AccountChangePassword changePasswordAccount)
        {
            await Builder
               .Jwt()
               .Path($"accounts/changepassword")
               .PostAsync(changePasswordAccount, true);
        }
```
```cpp
public async Task<ApiResult> ChangePasswordAsApiResult(AccountChangePassword accountChangePassword)
        {
            return await Builder
               .Jwt()
               .Path($"accounts/changepassword")
               .AsApiResult()
               .PostAsync(accountChangePassword);
        }
```
#### Resetting the password:
```cpp
async public Task ResetPassword(string accountIdOrEmail, AccountResetPassword accountResetPassword)
        {
            await Builder
               .Key()
               .Path($"accounts/{accountIdOrEmail}/resetpassword")
               .PostAsync(accountResetPassword, true);
        }
        public async Task<ApiResult> ResetPasswordAsApiResult(string accountIdOrEmail, AccountResetPassword accountResetPassword)
        {
            return await Builder
               .Key()
               .Path($"accounts/{accountIdOrEmail}/resetpassword")
               .AsApiResult()
               .PostAsync(accountResetPassword);
        }
```
#### MTM Initiating Password Reset 
```cpp
sync public Task<AccountInitiatePasswordResetResult> InitiatePasswordReset(string accountIdOrEmail)
        {
            return await Builder
               .Key()
               .Path($"accounts/{accountIdOrEmail}/initiatepasswordreset")
               .As<AccountInitiatePasswordResetResult>(true)
               .PostAsync(new  AccountInitiatePasswordReset());
        }
```
```cpp

        public async Task<ApiResult<AccountInitiatePasswordResetResult>> InitiatePasswordResetAsApiResult(string accountIdOrEmail)
        {
            return await Builder
               .Key()
               .Path($"accounts/{accountIdOrEmail}/initiatepasswordreset")
               .AsApiResult<AccountInitiatePasswordResetResult>()
               .PostAsync(new AccountInitiatePasswordReset());
        }
```
#### Seeking sent invitation to user
```cpp
 public async Task<ApiResult<List<InvitationSearchResult>>> SearchInvitationsAsApiResult(InvitationSearch invitationSearch)
        {
            return await Builder
               .Jwt()
               .Path($"invitations?email={invitationSearch.Email}")
               .AsApiResult<List<InvitationSearchResult>>()
               .GetAsync();
        }
```
#### Retrieving invitation
```cpp
        public async Task<ApiResult<InvitationGet>> GetInvitationAsApiResult(string key)
        {
            return await Builder
               .Key()
               .Path($"invitations/{key}")
               .AsApiResult<InvitationGet>()
               .GetAsync();
        }
```
#### Canceling invite 
```cpp
 public async Task<ApiResult> CancelInvitationAsApiResult(InvitationCancel invitationCancel)
        {
            return await Builder
               .Jwt()
               .Path($"invitations/cancel")
               .AsApiResult()
               .PostAsync(invitationCancel);
        }
```
#### Switching Tenants 
```cpp
public async Task<ApiResult<AccountLoginResult>> SwitchTenantAsApiResult(AccountSwitchTenant accountSwitchTenant)
        {
            return await Builder
               .Jwt()
               .Path($"accounts/switchtenant")
               .AsApiResult<AccountLoginResult>()
               .PostAsync(accountSwitchTenant);
        }
```
#### Removing account 
```cpp
 public async Task<ApiResult> RemoveAccountAsApiResult(int key, TenantRemoveAccount tenantRemoveAccount)
        {
            return await Builder
                .Jwt()
                .Path($"tenants/{key}/removeaccount")
                .AsApiResult()
                .PostAsync(tenantRemoveAccount);
        }
```
#### Adding an account 
```cpp
public async Task<ApiResult> TenantAddAccountAsApiResult(int key, TenantAddAccount tenantAddAccount)
        {
            return await Builder
                .Jwt()
                .Path($"tenants/{key}/addaccount")
                .AsApiResult()
                .PostAsync(tenantAddAccount);
        }
```
#### Setting up profile data
```cpp
 public async Task<ApiResult> SetProfileDataAsApiResult(string accountIdOrEmail, AccountSetProfileData accountSetProfileData)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/{accountIdOrEmail}/setprofiledata")
                .AsApiResult()
                .PostAsync(accountSetProfileData);
        }
```
```cpp
public async Task<ApiResult<AccountGet>> GetAccountAsApiResult(string accountIdOrEmail)
        {
            return await Builder
                .Jwt()
                .Path($"accounts/{accountIdOrEmail}")
                .AsApiResult<AccountGet>()
                .GetAsync();
        }
```
#### Searching Accounts 
```cpp
public async Task<ApiResult<List<AccountGet>>> SearchAccountsAsApiResult(SearchAccounts request)
        {
            return await Builder
                .Jwt()
                .Path($"accounts")
                .AsApiResult<List<AccountGet>>()
                .GetAsync(request);
        }

        public async Task<ApiResult<AddLoginMethodResult>> AddLoginMethodAsApiResult(string accountIdOrEmail,AddLoginMethodModel request)
        {
            return await Builder
                .JwtOrKey()
                .Path($"accounts/{accountIdOrEmail}/addlogin")
                .AsApiResult<AddLoginMethodResult>()
                .PostAsync(request);
        }

    }
```

## Getting support 👷
If you encounter any bugs, don't hesitate to submit an [issue](https://github.com/simplify9/Mtm/issues). We'll get back to you promptly! 
