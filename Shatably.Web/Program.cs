using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shatably.Data.Context;

namespace Shatably.Web
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    var host = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();
        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var db = scope.ServiceProvider.GetRequiredService<ShatblyDbContext>();
        //        db.Database.Migrate();

        //        host.Run();
        //    }
        //}
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    }
}
