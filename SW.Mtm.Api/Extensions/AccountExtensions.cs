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
                new Claim("EmailVerified", account.EmailVerified.ToString(), ClaimValueTypes.Boolean),
                new Claim("PhoneVerified", account.PhoneVerified.ToString(), ClaimValueTypes.Boolean),
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
            
            if (account.ProfileData != null)
                foreach (var item in account.ProfileData)
                    claims.Add(new Claim(item.Name, item.Value, item.Type));

            
            if (account.TenantId != null)
            {
                var membership = account.TenantMemberships.FirstOrDefault(m => m.TenantId == account.TenantId.Value);

                if (membership != null)
                {
                    claims.Add(new Claim("TenantId", account.TenantId.ToString(), ClaimValueTypes.Integer32));
                    claims.Add(new Claim("IsOwner", (membership.Type == MembershipType.Owner).ToString(), ClaimValueTypes.Boolean));

                    if (membership.ProfileData != null)
                        foreach (var item in membership.ProfileData)
                            claims.Add(new Claim(item.Name, item.Value, item.Type));

                }

            }

            return new ClaimsIdentity(claims, "Mtm");
        }

        public static string CreateJwt(this Account account, LoginMethod loginMethod, JwtTokenParameters jwtTokenParameters)
        {
            return jwtTokenParameters.WriteJwt(CreateClaimsIdentity(account, loginMethod));
        }
    }
}
