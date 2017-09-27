using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Controllers
{
    public class PostController : Controller
    {

        public IActionResult Index(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                ViewData["debug"] = s;
            }
            return View();
        }
    }
}
