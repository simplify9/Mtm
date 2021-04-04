using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Model
{
    public class AccountChangePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AccountExternalLogin
    {
        public EmailProvider EmailProvider { get; set; }
    }

    public class AccountLogin
    {
        public string Email { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public string Phone { get; set; }
        public string ApiKey { get; set; }
        public string OtpToken { get; set; }
        public string Code { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }

    }

    public class AccountLoginResult
    {
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }
        public string OtpToken { get; set; }
        public OtpType OtpType { get; set; }
        public string Password { get; set; }
        public string QrCodeUrl { get; set; }
        public string SecretKey { get; set; }
    }

    public class AccountCreate
    {
        public string Email { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public string Phone { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string CredentialName { get; set; }
        public ICollection<ProfileDataItem> ProfileData { get; set; }
        public OtpType SecondFactorMethod { get; set; }
    }

    public class AccountCreateResult
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }

    public class AccountSwitchTenant
    {
        public int NewTenant { get; set; }
    } 
    public class AccountSetAsTenantOwner
    {
        public string AccountId { get; set; }
        public int TenantId { get; set; }
    }

    public class AccountInitiatePasswordReset
    {

    } 


    public class AccountInitiatePasswordResetResult
    {
        public string AccountId { get; set; }
        public string Token { get; set; }
    }

    public class AccountResetPassword
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class AccountSetProfileData
    {
        //public string DisplayName { get; set; }
        public int? TenantId { get; set; }
        public ICollection<ProfileDataItem> ProfileData { get; set; }
    }

    public class AccountGet
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DisplayName { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public LoginMethod LoginMethods { get; set; }
        public OtpType SecondFactorMethod { get; set; }
        //public string SecondFactorKey { get;  set; }
        //
        public string[] Roles { get; set; }
        public bool Disabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public ICollection<int> TenantIdsMemberships { get; set; }
        public ICollection<ProfileDataItem> ProfileData { get; set; }
        

    }

    public class SearchAccounts
    {
        public string EmailContains { get; set; }
        public string PhoneContains { get; set; }
        public string[] Ids { get; set; }
    }

    public class AddLoginMethodModel
    {
        public LoginMethod LoginMethod { get; set; }
        public string Email { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string CredentialName { get; set; }
    }
    public class AddLoginMethodResult
    {
        public string Key { get; set; }
    }

    public class RemoveLoginMethodModel
    {
        public LoginMethod LoginMethod { get; set; }
    }

    public class AccountSetupOtpRequest
    {
        public OtpType Type { get; set; }
    }
    public class AccountSetupTotpResult
    {
        public string SecretKey { get; set; }
        public string QrCodeUrl { get; set; }
    }

    public class AccountValidateTotpRequest
    {
        public string Code { get; set; }
        public string OtpToken { get; set; }

    }
    public class AccountValidateTotpResult
    {
        public bool IsValid { get; set; }
    }
    public class AccountEnableSecondFactor
    {
        public string Code { get; set; }

    }
}
