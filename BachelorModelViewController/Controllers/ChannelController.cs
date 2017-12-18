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
using BachelorModelViewController.Interfaces;

namespace BachelorModelViewController.Controllers
{
    public class ChannelController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context; 
        private readonly IMongoOperations _mongoOperations;

        public ChannelController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IMongoOperations mongoOperations)
        {
            _context = context;
            _mongoOperations = mongoOperations;
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
                                                                        Id = x.Id,
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
                                                                        Id = x.Id,
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
                                                                        Id = x.Id,
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
        public async Task<ActionResult> Create(bool? asUser)
        {
            var createView = new CreateViewModel();
            if (asUser != null)
            {
                createView.AsUser = asUser;
            }
            if (createView.AsUser == null)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            if (createView.AsUser.Value)
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
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            try
            {
                var channel = new Channel();
                if (ModelState.IsValid)
                {
                    switch (model.AccessRestriction)
                    {
                        case 1:
                            channel.AccessRestriction = _context.AccessRestrictions.Where(x => x.Id == 1).First();
                            break;
                        case 2:
                            channel.AccessRestriction = _context.AccessRestrictions.Where(x => x.Id == 2).First();
                            break;
                        case 3:
                            if (model.DemandedRole.Id == _roleManager.FindByNameAsync("Administrator").Result.Id)
                            {
                                channel.AccessRestriction = _context.AccessRestrictions.Where(x => x.Id == 5).First();
                            } else if (model.DemandedRole.Id == _roleManager.FindByNameAsync("Supplier").Result.Id)
                            {
                                channel.AccessRestriction = _context.AccessRestrictions.Where(x => x.Id == 4).First();
                            } else
                            {
                                channel.AccessRestriction = _context.AccessRestrictions.Where(x => x.Id == 3).First();
                            }
                            break;
                        default:
                            channel.AccessRestriction = _context.AccessRestrictions.Where(x => x.Id == 1).First();
                            break;
                    }
                    if (model.GroupId != null)
                    {
                        channel.Group = _context.Groups.Where(x => x.Id == model.GroupId).FirstOrDefault();
                    }
                    channel.User = _context.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
                    channel.Name = model.Name;
                    channel.Description = channel.Description;
                    _context.Add(channel);
                    _context.SaveChanges();
                    _mongoOperations.CreateCollection(model.Name);
                    return RedirectToAction("Index");
                }
                if (!model.AsUser.Value)
                { 
                    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                    var adminRole = await _roleManager.FindByNameAsync("Administrator");
                    model.AccessibleGroups = _context.Associations.Where(x => x.User == currentUser && x.Role == adminRole).Select(x => x.Group).ToList();
                    if (model.AccessibleGroups.Count() == 1)
                    {
                        model.Group = model.AccessibleGroups.First();
                    }
                }
                return View(model);

            }
            catch
            {
                return View(model);
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