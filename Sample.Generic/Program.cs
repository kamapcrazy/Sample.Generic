using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Generic.Services;
using Sample.Generic.Services.Interfaces;
using System;

namespace Sample.Generic
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static ILogger<Program> _logger;

        static void Main(string[] args)
        {
            RegisterServiceProvider();

            //do the actual work here
            var htmlService = _serviceProvider.GetService<IHtmlService>();
            var result = htmlService.RemoveHtmlTag();

            Console.WriteLine(result);

            Console.ReadLine();
        }

        private static void RegisterServiceProvider()
        {
            //setup our DI
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IHtmlService, HtmlService>()
                .BuildServiceProvider();

            //configure console logging
            _serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Trace);

            RegisterLogger();

            _logger.LogInformation("Registration done! Application is ready!");
        }

        private static void RegisterLogger()
        {
            _logger = _serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            _logger.LogInformation("Starting application");
        }
    }
}
