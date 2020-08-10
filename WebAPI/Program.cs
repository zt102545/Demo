using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebAPI.Common.WebSocket;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new ConfigurationBuilder()
            //      .SetBasePath(Directory.GetCurrentDirectory())
            //      .AddCommandLine(args)//支持命令行参数
            //      .Build();
            var bindingConfig = new ConfigurationBuilder()
               .AddCommandLine(args)
               .Build();
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
