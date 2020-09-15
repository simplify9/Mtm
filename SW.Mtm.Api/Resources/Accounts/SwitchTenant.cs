using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("switchtenant")]
    [Protect]
    class SwitchTenant : ICommandHandler<AccountSwitchTenant>
    {
        public Task<object> Handle(AccountSwitchTenant request)
        {
            throw new NotImplementedException();
        }
    }
}
