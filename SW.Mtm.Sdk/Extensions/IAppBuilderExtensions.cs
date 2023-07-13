using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SW.HttpExtensions;
using SW.Mtm.Model;
using SW.PrimitiveTypes;
using System.Linq;

namespace SW.Mtm.Sdk
{
    public static class IAppBuilderExtensions
    {
        public static IApplicationBuilder UseApiKeyAsRequestContext(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (httpContext, next) =>
            {
                if (httpContext.Request.Headers.TryGetValue("apikey", out var value))
                {
                    var requestContext = httpContext.RequestServices.GetRequiredService<RequestContext>();
                    var loginAccount = new AccountLogin
                    {
                        ApiKey = value
                    };

                    var mtmClient = httpContext.RequestServices.GetRequiredService<IMtmClient>();
                    var result = await mtmClient.Login(loginAccount);
                    var jwtTokenParameters = httpContext.RequestServices.GetRequiredService<JwtTokenParameters>();
                    var claimsPrincipal = jwtTokenParameters.ReadJwt(result.Jwt);

                    httpContext.Request.Headers.TryGetValue(RequestContext.CorrelationIdHeaderName, out var cid);

                    requestContext.Set(claimsPrincipal, null, cid.FirstOrDefault());
                }
                await next();
            });

            return applicationBuilder;
        }
    }
}
