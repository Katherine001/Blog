using System.Collections.Generic;
using System.IO;
using Blog.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace Blog
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
                    Title = "Blog",
                    Description = "框架说明文档",
                });

                #region 读取xml信息

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Blog.xml"); //这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true); //默认的第二个参数是false，这个是controller的注释，记得修改

                #endregion

                #region Token绑定到ConfigureServices

                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> {{"Blog", new string[] { }},};
                c.AddSecurityRequirement(security);
                //方案名称“Blog”可自定义，上下一致即可
                c.AddSecurityDefinition("Blog", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization", //jwt默认的参数名称
                    In = "header", //jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });

                #endregion
            });

            #endregion

            #region Connect to mysql

            string connectionString = Configuration.GetConnectionString("BlogDatabase");
            services.AddDbContext<MySQLDbContext>(options =>
                options.UseMySql(connectionString));

            #endregion

            #region 跨域问题

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1"); });

            #endregion

            app.UseMvc();
            app.UseCors("AllowSpecificOrigin");
        }
    }
}