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

        public IActionResult Index()
        {
            return View();
        }
       
        //public IActionResult post()
        //{
        //    PostController pc = new PostController();
        //    return pc.POST();
        //}
        
        public IActionResult post(string s)
        {
            PostController pc = new PostController();
            return pc.POST(s);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
