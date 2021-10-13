using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment() || _env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Логгирование в текстовый файл
            app.Use(async (context, next) =>
            {
                string logMessage = $"[{DateTime.Now}]: New request to http://" +
                    $"{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

                string logFilePath = Path.Combine(_env.ContentRootPath, "Logs", "RequestLog.txt");

                await File.AppendAllTextAsync(logFilePath, logMessage);

                await next.Invoke();
            });

            // Логгирование в консоль
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"[{DateTime.Now}]: New request to http://" +
                    $"{context.Request.Host.Value + context.Request.Path}");

                await next.Invoke();
            });

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
