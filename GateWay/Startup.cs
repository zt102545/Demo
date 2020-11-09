using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GateWay.OcelotExtend;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Kubernetes;
using Ocelot.Provider.Polly;
using SkyApm.Utilities.DependencyInjection;

namespace GateWay
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOcelot()//ʹ��Ocelot
                .AddConsul()//ʹ��Consul
                .AddCacheManager(o => o.WithDictionaryHandle())//ʹ��Cache,Ĭ���ֵ�洢
                .AddPolly()//ʹ��Polly
                .AddKubernetes();//ʹ��K8s
            //�����IOcelotCache<CachedResponse>��Ĭ�ϵĻ����Լ�����滻���Զ����OcelotCache
            services.AddSingleton<IOcelotCache<CachedResponse>, OcelotCache>();
            //Skywalking
            services.AddSkyApmExtensions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot();//���������̵Ĺܵ�����Ocelot
        }
    }
}
