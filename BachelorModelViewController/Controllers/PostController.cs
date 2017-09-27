using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Controllers
{
    public class PostController : Controller
    {
        //public IActionResult POST()
        //{
        //    return View();
        //}

        public IActionResult POST(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                ViewData["holder"] = "shits not working";
                return View();
            }
            else
            {
                ViewData["holder"] = s;
                return View();
            }
        }
    }
}
