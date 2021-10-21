using Microsoft.AspNetCore.Http;
using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserInfoRepository _repo;

        /// <summary>
        /// Middleware-компонент должен иметь конструктор, принимающий RequestDelegate
        /// </summary>
        public LoggingMiddleware(RequestDelegate next, IUserInfoRepository repo)
        {
            _next = next;
            _repo = repo;
        }

        /// <summary>
        /// Необходимо реализовать метод Invoke или InvokeAsync
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"][0];

            var userInfo = new UserInfo()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                UserAgent = userAgent
            };

            await _repo.Add(userInfo);

            LogConsole(context);

            await LogFile(context);

            await _next.Invoke(context);
        }

        /// <summary>
        /// Логирование в консоль
        /// </summary>
        private void LogConsole(HttpContext context)
        {
            Console.WriteLine($"[{DateTime.Now}]: New request to http://" +
                $"{context.Request.Host.Value + context.Request.Path}");
        }

        /// <summary>
        /// Логирование в файл
        /// </summary>
        private async Task LogFile(HttpContext context)
        {
            string logMessage = $"[{DateTime.Now}]: New request to http://" +
                $"{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "RequestLog.txt");

            await File.AppendAllTextAsync(logFilePath, logMessage);
        }
    }
}
