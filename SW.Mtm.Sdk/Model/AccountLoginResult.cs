using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SW.Mtm.Model
{
    public class AccountLoginResult
    {
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }
        public string OtpToken { get; set; }
        public OtpType OtpType { get; set; }
        public string  Password { get; set; }
    }
}
