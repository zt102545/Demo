using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace ZZTCoreAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "ZZTApi文档",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "zzt", Email = "529166258@qq.com", Url = "www.baidu.com" }
                });

                #region 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlAPIPath = Path.Combine(basePath, "ZZTCoreAPI.xml");//这个就是刚刚配置的xml文件名
                var xmlModelPath = Path.Combine(basePath, "ZZTCoreModel.xml");
                c.IncludeXmlComments(xmlAPIPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                c.IncludeXmlComments(xmlModelPath, false);
                #endregion

                #region Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "ZZTAPI", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("ZZTAPI", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });
            #endregion

            #region JWT
            // 【授权】，好处就是不用在controller中，写多个 roles 。
            // 然后接口授权这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
            });

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Audience:Secret"])); 

            services.AddAuthentication("Bearer").AddJwtBearer(o=> {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //是否开启密钥认证和key值
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    //是否开启发行人认证和发行人
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Audience:Issuer"],

                    //是否开启订阅人认证和订阅人
                    ValidateAudience = true,
                    ValidAudience = Configuration["Audience:Audience"],

                    //认证时间的偏移量
                    ClockSkew = TimeSpan.Zero,
                    //是否开启时间认证
                    ValidateLifetime = true,
                    //是否该令牌必须带有过期时间
                    RequireExpirationTime = true
                };
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //HTTP管道是有先后顺序的，一定要写在 app.Mvc() 之前，否则不起作用。
            app.UseAuthentication();
            app.UseMvc();
           
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件，
            });
            #endregion
        }
    }
}
