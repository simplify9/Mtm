using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SW.HttpExtensions;

namespace SW.Mtm.Sdk.UnitTests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            //services.AddAuthentication().
            //    AddJwtBearer(options =>
            //    {
            //        options.RequireHttpsMetadata = false;
            //        options.SaveToken = true;

            //        options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidIssuer = "cqapi",
            //            ValidAudience = "cqapi",

            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6547647654764764767657658658758765876532542"))
            //        };
            //    });

            services.AddApiClient<IMtmClient, MtmClient, MtmClientOptions>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapControllers();
            //});
        }
    }
}
