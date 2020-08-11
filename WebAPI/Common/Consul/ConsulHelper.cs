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
                c.Address = new Uri("http://120.79.246.191:8500/");//Consul服务端的地址
                c.Datacenter = "dc1";
            });

            //直接在VS里运行会报错，因为拿不到ip跟port，这些参数是启动的时候传入的，需要用指令启动
            //dotnet WebAPI.dll --urls="http://*:5005" --ip="127.0.0.1" --port=5005
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);
            //客户端向服务端注册的方法
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service " + ip + ":" + port,//ID唯一
                Name = "ZztService",//注册名称
                Address = ip,//注册客户端的IP
                Port = port,//注册客户端的端口号
                Tags = new string[] { weight.ToString() },//传入参数
                Check = new AgentServiceCheck()//心跳检测
                {
                    Interval = TimeSpan.FromSeconds(12),//每12s检查一次
                    HTTP = $"http://{ip}:{port}/api/Microservices/Index",//调用检测接口，该方法没有内容，直接返回200
                    Timeout = TimeSpan.FromSeconds(5),//超时时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)//超时后20s后服务端认为该客户端挂了，直接断开。
                }
            });
            //命令行参数获取
            Console.WriteLine($"{ip}:{port}--weight:{weight}");
        }
    }
}
