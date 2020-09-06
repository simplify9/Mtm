using SW.Mtm.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Sdk
{
    public interface IMtmClient
    {
        Task<AccountLoginResult> Login(AccountLogin loginAccount);
        Task<AccountRegisterResult> Register(AccountRegister registerAccount);
        Task CreateTenant(TenantCreate registerAccount);
        Task CreateAdditionalTenant(TenantCreate tenantCreate);

    }
}
