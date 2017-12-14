using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BachelorModelViewController.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using BachelorModelViewController.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BachelorModelViewController.Models.ViewModels.ChannelViewModels;

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
        public async Task<ActionResult> Index()
        {
            var accessibleChannels = new AccessibleChannelsViewModel();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null)
            {
                var groups = _context.Associations.Where(x => x.User == currentUser).Select(x => x.Group).ToList();

                accessibleChannels.GroupRestrictedChannels = await _context.Channels
                                                            .Where(x => x.AccessRestriction.GroupRestricted == true && groups.Contains(x.Group))
                                                                    .Select(x => new ChannelViewModel
                                                                    {
                                                                        Name = x.Name,
                                                                        Description = x.Description,
                                                                        User = x.User,
                                                                        Group = x.Group,
                                                                        EntryPoint = x.EntryPoint,
                                                                        EndPoint = x.EndPoint
                                                                    }).ToList();


                accessibleChannels.UserRestrictedChannels = await _context.Channels
                                                            .Where(x => x.AccessRestriction.GroupRestricted == false
                                                                    && x.AccessRestriction.UserRestricted == true)
                                                                    .Select(x => new ChannelViewModel
                                                                        {
                                                                            Name = x.Name,
                                                                            Description = x.Description,
                                                                            User = x.User,
                                                                            Group = x.Group,
                                                                            EntryPoint = x.EntryPoint,
                                                                            EndPoint = x.EndPoint
                                                                        }).ToList().AsQueryable();

            }
                accessibleChannels.UnRestrictedChannels = _context.Channels
                                                            .Where(x => x.AccessRestriction.GroupRestricted == false 
                                                                    && x.AccessRestriction.UserRestricted == false)
                                                                    .Select(x => new ChannelViewModel
                                                                    {
                                                                        Name = x.Name,
                                                                        Description = x.Description,
                                                                        User = x.User,
                                                                        Group = x.Group,
                                                                        EntryPoint = x.EntryPoint,
                                                                        EndPoint = x.EndPoint
                                                                    }).ToList().AsQueryable();
            return View(accessibleChannels);
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