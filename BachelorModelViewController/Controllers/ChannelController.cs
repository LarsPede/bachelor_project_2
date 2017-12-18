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
using BachelorModelViewController.Helpers;
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
            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            if (currentUser != null)
            {
                var groups = _context.Associations.Where(x => x.User == currentUser).Select(x => x.Group).ToList();
                var administratorAccess = _context.Associations.Where(x => x.User == currentUser && x.Role == adminRole).Select(x => x.Group).FirstOrDefault();

                if (administratorAccess != null)
                {
                    accessibleChannels.AdminForGroup = true;
                } else
                {
                    accessibleChannels.AdminForGroup = false;
                }

                accessibleChannels.GroupRestrictedChannels = _context.Channels
                                                            .Where(x => x.AccessRestriction.GroupRestricted == true && groups.Contains(x.Group))
                                                                    .Select(x => new ChannelViewModel
                                                                    {
                                                                        Name = x.Name,
                                                                        Description = x.Description,
                                                                        User = x.User,
                                                                        Group = x.Group
                                                                    }).ToList();


                accessibleChannels.UserRestrictedChannels = _context.Channels
                                                            .Where(x => x.AccessRestriction.GroupRestricted == false
                                                                    && x.AccessRestriction.UserRestricted == true)
                                                                    .Select(x => new ChannelViewModel
                                                                        {
                                                                            Name = x.Name,
                                                                            Description = x.Description,
                                                                            User = x.User,
                                                                            Group = x.Group
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
                                                                        Group = x.Group
                                                                    }).ToList().AsQueryable();
            return View(accessibleChannels);
        }

        // GET: Channel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Channel/Create
        public async Task<ActionResult> Create(bool asUser)
        {
            var createView = new CreateViewModel();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            if (asUser)
            {
                createView.User = currentUser;
            } else
            {
                createView.AccessibleGroups = _context.Associations.Where(x => x.User == currentUser && x.Role == adminRole).Select(x => x.Group).ToList();
                if (createView.AccessibleGroups.Count() == 1)
                {
                    createView.Group = createView.AccessibleGroups.First();
                }
            }
            return View(createView);
        }

        // POST: Channel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            try
            {
                var channel = new Channel();
                if (ModelState.IsValid)
                {
                    if (model.Group != null)
                    {
                        channel.Group = model.Group;
                    } else
                    {
                        channel.User = model.User;
                    }

                    var dh1 = new DatatypeHelper();

                    var modelObject1 = dh1.HandleAsObject(model.JsonContentAsString);

                    return RedirectToAction("Index");
                }
                var dh = new DatatypeHelper();

                var modelObject = dh.HandleAsObject(model.JsonContentAsString);


                return View(model);

            }
            catch(Exception e)
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