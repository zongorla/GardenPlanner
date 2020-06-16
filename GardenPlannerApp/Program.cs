using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace GardenPlannerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((HostBuilderContext context,LoggerConfiguration configuration) =>
                {
                    configuration.WriteTo.Console(new RenderedCompactJsonFormatter())
                    .WriteTo.File(new RenderedCompactJsonFormatter(), "log-gardenapp.txt", rollOnFileSizeLimit: true, fileSizeLimitBytes: 10000000)
                    .WriteTo.Seq("http://zongi-thinkpad-t540p.local:5341/");
                })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });


    }
}
