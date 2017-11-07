using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BachelorModelViewController.Models.ViewModels.GroupViewModels;
using BachelorModelViewController.Models;
using BachelorModelViewController.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BachelorModelViewController.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public GroupController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }

        // GET: Group/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Group/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var group = new Group { Name = model.GroupName};
                    _context.Groups.Add(group);
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var role = await _roleManager.FindByNameAsync("Administrator");
                    var association = new Association { GroupId = group.Id, UserId = user.Id, RoleId = role.Id };
                    _context.Associations.Add(association);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Group/Edit/5
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

        // GET: Group/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Group/Delete/5
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
    }
}