using Microsoft.Extensions.Configuration;
using SW.HttpExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Sdk
{
    public class MtmClientOptions : ApiClientOptionsBase
    {
        public override string ConfigurationSection => "MtmClient";
    }
}
