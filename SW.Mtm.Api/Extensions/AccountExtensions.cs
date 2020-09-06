using SW.HttpExtensions;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SW.Mtm
{
    internal static class AccountExtensions
    {
        public static ClaimsIdentity CreateClaimsIdentity(this Account account, LoginMethod loginMethod)
        {
            var roleClaims = account.Roles.Select(r => new Claim(ClaimTypes.Role, r));
            var claims = new List<Claim>(roleClaims)
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id),
                new Claim(ClaimTypes.GivenName, account.DisplayName),
                new Claim("EmailVerified", account.EmailVerified.ToString()),
                new Claim("PhoneVerified", account.PhoneVerified.ToString()),
            };

            switch (loginMethod)
            {
                case LoginMethod.EmailAndPassword:
                    claims.Add(new Claim(ClaimTypes.Name, account.Email));
                    break;
                case LoginMethod.PhoneAndOtp:
                    claims.Add(new Claim(ClaimTypes.Name, account.Phone));
                    break;
                case LoginMethod.ApiKey:
                    claims.Add(new Claim(ClaimTypes.Name, account.Id));
                    break;
            }

            if (account.Email != null) claims.Add(new Claim(ClaimTypes.Email, account.Email));
            if (account.Phone != null) claims.Add(new Claim(ClaimTypes.MobilePhone, account.Phone));
            if (account.TenantId != null) claims.Add(new Claim("TenantId", account.TenantId.ToString()));

            return new ClaimsIdentity(claims, "Mtm");
        }

        public static string CreateJwt(this Account account, LoginMethod loginMethod, JwtTokenParameters jwtTokenParameters)
        {
            return jwtTokenParameters.WriteJwt(CreateClaimsIdentity(account, loginMethod));
        }
    }
}
