using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SW.EfCoreExtensions;
using SW.Logger;

namespace SW.Mtm.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).
                UseSwLogger().
                Build().
                MigrateDatabase<MtmDbContext>().
                Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
