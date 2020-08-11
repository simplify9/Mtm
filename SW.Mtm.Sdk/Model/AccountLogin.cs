using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Sdk.Model
{
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
}
