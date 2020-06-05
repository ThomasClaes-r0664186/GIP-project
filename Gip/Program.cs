using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logzio.DotNet.NLog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;

namespace Gip
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var logzioTarget = new LogzioTarget
            {
                Token = "GaBfHGSpWGPkZTaCCmseJTNissRUpksL",
                LogzioType = "nlog",
                ListenerUrl = "https://listener-nl.logz.io:8071",
            };
            config.AddTarget("Logzio", logzioTarget);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, "Logzio", "*");
            LogManager.Configuration = config;

            CreateHostBuilder(args).Build().Run();

            LogManager.Shutdown();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
