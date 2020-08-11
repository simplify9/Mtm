using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Sdk.Model
{
    public class AccountRegister
    {
        public string Email { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public string Phone { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string CredentialName { get; set; }
    }
}
