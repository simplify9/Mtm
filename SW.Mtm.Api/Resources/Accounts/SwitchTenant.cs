﻿using Microsoft.EntityFrameworkCore;
using SW.HttpExtensions;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Mtm.Resources.Accounts
{
    [HandlerName("switchtenant")]
    [Protect]
    class SwitchTenant : ICommandHandler<AccountSwitchTenant,object>
    {
        private readonly MtmDbContext dbContext;
        private readonly JwtTokenParameters jwtTokenParameters;
        private readonly RequestContext requestContext;
        private readonly MtmOptions mtmOptions;
        public SwitchTenant(MtmDbContext dbContext, JwtTokenParameters jwtTokenParameters, RequestContext requestContext, MtmOptions mtmOptions)
        {
            this.dbContext = dbContext;
            this.jwtTokenParameters = jwtTokenParameters;
            this.requestContext = requestContext;
            this.mtmOptions = mtmOptions;
        }
        public async Task<object> Handle(AccountSwitchTenant request)
        {
            //test account id c5cd075be5ca47a8a63107954bae44ac
            //c5cd075be5ca47a8a63107954bae44ac
            //tenants 87 and 88

            //var tenantId = requestContext.GetTenantId();
            //if (!tenantId.HasValue)
            //    throw new SWException("Tenant is empty.");
            var jwtExpiryTimeSpan = TimeSpan.FromMinutes(mtmOptions.JwtExpiryMinutes);
            var accountId = requestContext.GetNameIdentifier();
            if (string.IsNullOrEmpty(accountId))
                throw new SWException("Account is empty.");

            var account = await dbContext.Set<Account>().FindAsync(accountId);

            if (!account.SetTenantId(request.NewTenant))
                throw new SWException($"Account doesnt belong to tenant {request.NewTenant}.");
            
            var loginResult = new AccountLoginResult
            {
                Jwt = account.CreateJwt(account.LoginMethods, jwtTokenParameters, jwtExpiryTimeSpan),
                RefreshToken = CreateRefreshToken(account, account.LoginMethods)
            };

            await dbContext.SaveChangesAsync();

            return loginResult;
        }

        private string CreateRefreshToken(Account account, LoginMethod loginMethod)
        {
            var refreshToken = new RefreshToken(account.Id, loginMethod);
            dbContext.Add(refreshToken);
            return refreshToken.Id;
        }
    }
}
