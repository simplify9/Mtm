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

    public class AccountLogin
    {
        public string Email { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public string Phone { get; set; }
        public string ApiKey { get; set; }
        public string OtpToken { get; set; }
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
    }

    public class AccountRegister
    {
        public string Email { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public string Phone { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string CredentialName { get; set; }
    }

    public class AccountRegisterResult
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }

    public class AccountSwitchTenant
    {
        public int AccountId { get; set; }
        public int NewTenant { get; set; }
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

}
