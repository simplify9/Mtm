using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using SW.CqApi;
using SW.EfCoreExtensions;
using SW.HttpExtensions;
using SW.Logger;
using SW.PrimitiveTypes;

namespace SW.Mtm.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var mtmOptions = new MtmOptions();
            Configuration.GetSection(MtmOptions.ConfigurationSection).Bind(mtmOptions);
            services.AddSingleton(mtmOptions);

            services.AddControllers();
            services.AddHealthChecks();

            services.AddCqApi(configure =>
                {
                    configure.RolePrefix = "Mtm";
                },
                typeof(MtmDbContext).Assembly);

            services.AddScoped<RequestContext>();
            services.AddJwtTokenParameters();


            if (mtmOptions.DatabaseType.ToLower() == RelationalDbType.PgSql.ToString().ToLower())
            {
                services.AddDbContext<MtmDbContext, PgSql.MtmDbContext>(c =>
                {
                    c.EnableSensitiveDataLogging(true);
                    c.UseSnakeCaseNamingConvention();
                    c.UseNpgsql(Configuration.GetConnectionString(MtmDbContext.ConnectionString), b =>
                    {
                        b.MigrationsHistoryTable("__ef_migrations_history", PgSql.MtmDbContext.Schema);
                        b.MigrationsAssembly(typeof(PgSql.DbType).Assembly.FullName);
                        b.UseAdminDatabase("defaultdb");
                    });

                });
            }
            else
            {
                services.AddDbContext<MtmDbContext>(c =>
                {
                    c.EnableSensitiveDataLogging(true);

                    if (mtmOptions.DatabaseType.ToLower() == RelationalDbType.MySql.ToString().ToLower())
                    {
                        c.UseMySql(Configuration.GetConnectionString(MtmDbContext.ConnectionString), b =>
                        {
                            b.MigrationsAssembly(typeof(MySql.DbType).Assembly.FullName);
                            b.CommandTimeout(90);
                            b.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                        });
                    }
                    else if (mtmOptions.DatabaseType.ToLower() == RelationalDbType.MsSql.ToString().ToLower())
                    {
                        //c.UseSqlServer(Configuration.GetConnectionString(InfolinkDbContext.ConnectionString), b =>
                        //{
                        //    b.MigrationsAssembly(typeof(MsSql.DbType).Assembly.FullName);
                        //});
                        throw new NotImplementedException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                });
            }


            services.AddAuthentication().
                AddJwtBearer(configureOptions =>
                {
                    configureOptions.RequireHttpsMetadata = false;
                    configureOptions.SaveToken = true;
                    configureOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Token:Issuer"],
                        ValidAudience = Configuration["Token:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase("/_mtm");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiKeyAsRequestContext();
            app.UseHttpAsRequestContext();
            app.UseRequestContextLogEnricher();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
