using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Mtm.Model
{

    [Flags]
    public enum LoginMethod
    {
        None = 0,
        ApiKey = 1,
        EmailAndPassword = 2,
        PhoneAndOtp = 4,
    }
}
