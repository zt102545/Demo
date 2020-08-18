using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Common.WebSocket.Extension;
using WebAPI.Common;
using WebAPI.Common.Consul;
using SkyApm.Utilities.DependencyInjection;

namespace WebAPI
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
            services.AddControllers();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v0.1.0",//�汾��
                    Title = "ZZTApi�ĵ�",//�ĵ�����
                    Description = "���˵���ĵ�",//�ĵ�����
                    TermsOfService = new Uri("https://example.com/terms"),//��������
                    Contact = new OpenApiContact { Name = "zzt", Email = "000000@qq.com", Url = new Uri("https://blog.csdn.net/zt102545") }//��ϵ��
                });
                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                //var xmlAPIPath = Path.Combine(basePath, "ZZTCoreAPI.xml");//������Ǹո����õ�xml�ļ���
                //var xmlModelPath = Path.Combine(basePath, "ZZTCoreModel.xml");//���������model���XML�ĵ����������XML�ĵ��ķ����������һ����
                //c.IncludeXmlComments(xmlAPIPath, true);//�ڶ�������true��ʾ�ÿ�������XMLע�͡�Ĭ����false
                //c.IncludeXmlComments(xmlModelPath, true);
            });
            #endregion

            //Skywalking
            services.AddSkyApmExtensions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Swagger��NetCore3.0���ϰ汾д��һ��
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";//·�����ã�����Ϊ�գ���ʾֱ�ӷ��ʸ��ļ���
            });
            #endregion

            #region ʹ��websocket(��չ����)
            //app.AddWebSocketService<WSTestWSBehavior>("/test");//��������

            //������ӣ�����˷��ͣ�WebAPIWebSocketService.CurrentServiceHost("/test").Sessions.SendTo(dataStr, key);
            var test = Configuration.GetSection("WebSocketHosts:Server:TestUrl").Value;
            WebAPIWebSocketService.AddWebSocketService<WSTestWSBehavior>(test);
            app.AddWebSocketMap();
            #endregion

            #region Consulע�� 
            //վ���������--ִ����ִֻ��һ��
            this.Configuration.ConsulRegist();
            #endregion
        }
    }
}
