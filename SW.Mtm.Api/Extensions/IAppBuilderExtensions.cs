using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SW.Mtm.Domain;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SW.Mtm
{
    public static class IAppBuilderExtensions
    {

        public static IApplicationBuilder UseApiKeyRequestContext(this IApplicationBuilder applicationBuilder)
        {

            applicationBuilder.Use(async (httpContext, next) =>
            {
                if (httpContext.Request.Headers.TryGetValue("apikey", out var value))
                {
                    var requestContext = httpContext.RequestServices.GetRequiredService<RequestContext>();
                    var mtmDbContext = httpContext.RequestServices.GetRequiredService<MtmDbContext>();
                    var account = await mtmDbContext.Set<Account>().Where(a => a.ApiCredentials.Any(cred => cred.Key == value.First())).SingleOrDefaultAsync();
                    var user = new ClaimsPrincipal(account.CreateClaimsIdentity(LoginMethod.ApiKey));

                    httpContext.Request.Headers.TryGetValue(RequestContext.CorrelationIdHeaderName, out var cid);

                    requestContext.Set(user, null, cid.FirstOrDefault());
                }

                await next();

            });

            return applicationBuilder;

        }

    }
}
