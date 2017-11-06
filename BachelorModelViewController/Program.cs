using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace BachelorModelViewController
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();

            BachelorModelViewController.UnitTests.DatatypeTest datatypeTest = new BachelorModelViewController.UnitTests.DatatypeTest();
            datatypeTest.run();

            
            while (string.IsNullOrEmpty(System.Console.ReadKey().ToString())) ;
        }
    }
}
