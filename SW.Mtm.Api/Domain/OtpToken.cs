using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Domain
{
    public class OtpToken : BaseEntity<string>, IHasCreationTime
    {
        private OtpToken()
        {
        }

        public OtpToken(string accountId, LoginMethod loginMethod) : this(accountId, loginMethod, null, OtpType.Totp)
        {
        }

        public OtpToken(string accountId, LoginMethod loginMethod, string password) : this(accountId, loginMethod, password, OtpType.Otp)
        {
        }

        private OtpToken(string accountId, LoginMethod loginMethod, string password, OtpType otpType)
        {
            Id = Guid.NewGuid().ToString("N");
            AccountId = accountId;
            LoginMethod = loginMethod;
            Password = password;
            Type = otpType;
        }

        public string AccountId { get; private set; }
        public string Password { get; private set; }
        public OtpType Type { get; private set; }
        public LoginMethod LoginMethod { get; set; }
        public DateTime CreatedOn { get ; set ; }
        
    }
}
