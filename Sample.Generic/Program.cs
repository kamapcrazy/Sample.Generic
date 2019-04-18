using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Generic.Services;
using Sample.Generic.Services.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Sample.Generic
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static ILogger<Program> _logger;

        static void Main(string[] args)
        {
            var test = GetTestObject();

            if (test.Prop3?.Child1 != null)
                Console.WriteLine("null");

            Console.WriteLine(test);

            Console.ReadLine();
        }

        private static dynamic GetTestObject()
        {
            var test = new TestDynamic
            {
                Prop1 = "test",
                Prop3 = new ChildDynamic
                {
                    Child1 = "test"
                }
            };

            return test;
        }



        private static string GenerateETag(byte[] responseData)
        {
            string checksum;

            using (var algo = SHA1.Create())
            {
                byte[] bytes = algo.ComputeHash(responseData);
                checksum = $"\"{Convert.ToBase64String(bytes)}\"";
            }

            return checksum;
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

        public static string DecodeFromUtf8(string utf8String)
        {
            // copy the string as UTF-8 bytes.
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
                utf8Bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }


    }

    internal class TestDynamic
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public ChildDynamic Prop3 { get; set; }

    }

    internal class ChildDynamic
    {
        public string Child1 { get; set; }
    }
}
