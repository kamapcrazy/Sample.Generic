using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Generic.Services;
using Sample.Generic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            IEnumerable<TestDynamic> test = GetListObject();

            Update(test);

            Console.WriteLine(string.Join(", ", test.Select(x => x.Prop1)));

            Console.ReadLine();
        }

        private static void Update(IEnumerable<TestDynamic> input)
        {
            foreach (var item in input)
            {
                item.Prop1 = "updated";
            }
        }

        public static void AssignmentAction(string x)
        {
            x = x + "updated";
        }

        private static IEnumerable<TestDynamic> GetListObject()
        {
            var test = new List<TestDynamic>();
            for (var i = 1; i < 10; i++)
                test.Add(GetTestObject());
            return test;
        }

        private static TestDynamic GetTestObject()
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
