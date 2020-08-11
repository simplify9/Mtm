using SW.Mtm.Sdk.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Api.Resources.Accounts
{
    [HandlerName("switchtenant")]
    [Protect]
    class SwitchTenant : ICommandHandler<AccountSwitch>
    {
        public Task<object> Handle(AccountSwitch request)
        {
            throw new NotImplementedException();
        }
    }
}
