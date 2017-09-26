using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BachelorModelViewController.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();
        
        static async Task<string> GetStringValue()
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync("http://localhost:12345/api/values/5");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = GetStringValue().Result;

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
