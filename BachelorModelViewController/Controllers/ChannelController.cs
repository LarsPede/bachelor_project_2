using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BachelorModelViewController.Models;
using BachelorModelViewController.Models.ViewModels.DataViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using BachelorModelViewController.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BachelorModelViewController.Controllers
{
    public class ChannelController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context; 
        
        public ChannelController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Channel
        public ActionResult Index()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);

            //var accessibleChannels = from Channels in _context.Channels ;

            var asssociations = from Channels in _context.Channels
                                join Associations in _context.Associations on Channels.Group equals Associations.GroupId into Association
                                from joined in Association
                                where joined.



    //        var innerGroupJoinQuery2 =
    //from category in categories
    //join prod in products on category.ID equals prod.CategoryID into prodGroup
    //from prod2 in prodGroup
    //where prod2.UnitPrice > 2.50M
    //select prod2;
            )
                                
                                     

            return View();
        }

        // GET: Channel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Channel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Channel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Channel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Channel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Channel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Channel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public void CreateDatatype(string s)
        {
            var jsonObject = JsonConvert.DeserializeObject(s);

            
        }
    }
}