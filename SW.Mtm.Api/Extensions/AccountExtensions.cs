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
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id),
                new Claim(ClaimTypes.GivenName, account.DisplayName),
                new Claim("email_verified", account.EmailVerified.ToString(), ClaimValueTypes.Boolean),
                new Claim("phone_verified", account.PhoneVerified.ToString(), ClaimValueTypes.Boolean),
                new Claim("login_methods", ((int)account.LoginMethods).ToString(), ClaimValueTypes.Integer),
                new Claim("second_factor_method", ((int)account.SecondFactorMethod).ToString(), ClaimValueTypes.Integer),

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

            if (account.Roles != null)
                claims.AddRange(account.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            if (account.ProfileData != null)
                claims.AddRange(account.ProfileData.Select(p => new Claim(p.Name, p.Value, p.Type)));


            if (account.TenantId != null)
            {
                var membership = account.TenantMemberships.FirstOrDefault(m => m.TenantId == account.TenantId.Value);

                if (membership != null)
                {
                    claims.Add(new Claim("TenantId", account.TenantId.ToString(), ClaimValueTypes.Integer32));
                    claims.Add(new Claim("tenant_owner", (membership.Type == MembershipType.Owner).ToString(), ClaimValueTypes.Boolean));

                    if (membership.ProfileData != null)
                        claims.AddRange(membership.ProfileData.Select(p => new Claim(p.Name, p.Value, p.Type)));

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
