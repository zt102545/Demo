using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Common.Consul
{
    public static class ConsulHelper
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://120.79.246.191:8500/");
                c.Datacenter = "dc1";
            });

            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);

            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service " + ip + ":" + port,//ID唯一
                Name = "ZztService",//注册名称
                Address = ip,
                Port = port,
                Tags = new string[] { weight.ToString() },//传入参数
                Check = new AgentServiceCheck()//心跳检测
                {
                    Interval = TimeSpan.FromSeconds(12),
                    HTTP = $"http://{ip}:{port}/api/Microservices/Index",
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)
                }
            });
            //命令行参数获取
            Console.WriteLine($"{ip}:{port}--weight:{weight}");
        }
    }
}
