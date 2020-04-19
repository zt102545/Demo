using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

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
                    Version = "v0.1.0",//版本号
                    Title = "ZZTApi文档",//文档标题
                    Description = "框架说明文档",//文档描述
                    TermsOfService = new Uri("https://example.com/terms"),//服务条款
                    Contact = new OpenApiContact { Name = "zzt", Email = "000000@qq.com", Url = new Uri("https://blog.csdn.net/zt102545") }//联系人
                });
                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                //var xmlAPIPath = Path.Combine(basePath, "ZZTCoreAPI.xml");//这个就是刚刚配置的xml文件名
                //var xmlModelPath = Path.Combine(basePath, "ZZTCoreModel.xml");//这个是引用model层的XML文档。设置输出XML文档的方法跟上面的一样。
                //c.IncludeXmlComments(xmlAPIPath, true);//第二个参数true表示用控制器的XML注释。默认是false
                //c.IncludeXmlComments(xmlModelPath, true);
            });
            #endregion
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


            #region Swagger；NetCore3.0以上版本写法一样
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件。
            });
            #endregion
        }
    }
}
