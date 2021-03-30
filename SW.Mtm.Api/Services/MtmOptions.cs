using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm
{
    public class MtmOptions
    {
        public const string ConfigurationSection = "Mtm";

        public MtmOptions()
        {
            DatabaseType = "MySql";

        }

        public string DatabaseType { get; set; }
        
        public string TotpIssuer { get; set; }
    }
}
