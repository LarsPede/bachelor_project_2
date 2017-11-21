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
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public ActionResult Index(string errorMessage)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var groups =
                from Group in _context.Groups
                join Association in _context.Associations on Group.Id equals Association.GroupId
                select new MemberGroupsViewModel { GroupId = Group.Id, UserId = Association.UserId, GroupName = Group.Name, RoleId = Association.RoleId };
            groups = groups.Where(x => x.UserId == userId).ToList().AsQueryable();
            ViewBag.ErrorMessage = errorMessage;
            return View(groups);
        }

        // GET: Group/Details/5
        public ActionResult Details(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var association = _context.Associations.Where(x => x.GroupId == id).Where(x => x.UserId == userId).FirstOrDefault();
            var group = _context.Groups.Where(x => x.Id == id).Select(x => new DetailViewModel() { Id = x.Id, Name = x.Name, RoleId = association.RoleId }).FirstOrDefault();
            return View(group);
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
        public async Task<ActionResult> EditAsync(int id)
        {
            var currUser = await _userManager.GetUserAsync(HttpContext.User);
            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            var members =
                from User in _context.Users
                join Association in _context.Associations on User.Id equals Association.UserId
                select new MemberViewModel { User = User, GroupId = Association.GroupId, RoleId = Association.RoleId };
            var applyingMembers = members.Where(x => x.GroupId == id).Where(x => x.RoleId == null).ToList().AsQueryable();
            members = members.Where(x => x.GroupId == id).Where(x => x.RoleId != null).ToList().AsQueryable();
            if (!(members.Where(x => x.User.Id == currUser.Id).Where(x => x.RoleId == adminRole.Id).Any()))
            {
                return RedirectToAction("Index");
            }
            var group = _context.Groups.Where(x => x.Id == id).Select(x => new EditViewModel() { Id = x.Id, GroupName = x.Name, Members = members, ApplyingMembers = applyingMembers }).FirstOrDefault();
            

            return View(group);
        }

        // POST: Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GroupViewModel collection)
        {
            try
            {
                _context.Groups.Where(x => x.Id == id).FirstOrDefault().Name = collection.Name;
                _context.SaveChanges();
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

        // GET: Group/Apply
        public ActionResult Apply(string id, int groupId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var groups =
                from Group in _context.Groups
                join Association in _context.Associations on Group.Id equals Association.GroupId
                select new NonMemberGroupsViewModel { Id = Group.Id, UserId = Association.UserId, GroupName = Group.Name };
            groups = groups.Where(x => x.UserId == userId).ToList().AsQueryable();
            var allGroups = _context.Groups.ToList();
            allGroups.RemoveAll(x => groups.Any(y => y.Id == x.Id));
            var wantedGroup = allGroups.Select(x => new NonMemberGroupsViewModel() { Id = x.Id, GroupName = x.Name }).ToList();
            return View(wantedGroup);
        }

        // Get: Group/Apply/1
        public async Task<ActionResult> ApplyGroupAsync(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var role = await _roleManager.FindByNameAsync("Administrator");
                var association = new Association { GroupId = id, UserId = user.Id };
                _context.Associations.Add(association);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Group/Approve/1/1
        public async Task<ActionResult> ApproveAsync(string id, int groupId)
        {
            var role = await _roleManager.FindByNameAsync("Consumer");
            _context.Associations.Where(x => x.UserId == id && x.GroupId == groupId).FirstOrDefault().RoleId = role.Id;
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = groupId });
        }

        // GET: Group/Deny/123-123-123/1
        public ActionResult Deny(string id, int groupId)
        {
            Association association = _context.Associations.Where(x => x.UserId == id && x.GroupId == groupId).FirstOrDefault();
            _context.Associations.Remove(association);
            _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = groupId });
        }


        // POST: Group/RoleChange/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleChangeAsync(int groupId, EditViewModel collection)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(collection.RoleName);
                _context.Associations.Where(x => x.UserId == collection.UserId && x.GroupId == collection.Id).FirstOrDefault().RoleId = role.Id;
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = collection.Id });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
    }
}