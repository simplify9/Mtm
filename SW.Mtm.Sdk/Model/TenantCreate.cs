using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Sdk.Model
{
    public class TenantCreate
    {
        public string DisplayName { get; set; }
        public string OwnerEmail { get; set; }
        public EmailProvider OwnerEmailProvider { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerPassword { get; set; }
    }
}
