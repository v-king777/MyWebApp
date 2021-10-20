using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyWebApp.Middlewares;
using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserInfoRepository, UserInfoRepository>();
            
            string connection = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MyWebAppContext>(options =>
            options.UseSqlServer(connection), ServiceLifetime.Singleton);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment() || _env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMiddleware<LoggingMiddleware>();

            // Обработчик для главной страницы
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {_env.ApplicationName}!");
                });
            });

            // Обработчики для остальных страниц
            app.Map("/about", About);
            app.Map("/config", Config);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"404 - Page not found");
            });
        }

        /// <summary>
        /// Обработчик для страницы About
        /// </summary>
        private void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{_env.ApplicationName} - " +
                    $"ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        /// Обработчик для страницы Config
        /// </summary>
        private void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {_env.ApplicationName}. " +
                    $"App running configuration: {_env.EnvironmentName}");
            });
        }
    }
}
