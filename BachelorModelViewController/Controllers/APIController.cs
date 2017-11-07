using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BachelorModelViewController.Models;
using BachelorModelViewController.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BachelorModelViewController.Controllers
{
    [Produces("application/json")]
    [Route("api/Channel")]
    public class APIController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public APIController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        // GET: api/Groups
        [HttpGet]
        public IEnumerable<Group> GetAll()
        {
            return _context.Groups.ToList();
        }

        // GET: api/Group/5
        [HttpGet("{id}", Name = "GetGroup")]
        public IActionResult GetGroupById(int id)
        {
            var group = _context.Groups.FirstOrDefault(t => t.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            return new ObjectResult(group);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGroupAsync([FromBody] Group item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Groups.Add(item);
            string userId =  (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            string roleId = (await _roleManager.FindByNameAsync("Administrator"))?.Id;
            var association = new Association { GroupId = item.Id, UserId = userId, RoleId = roleId };
            _context.Associations.Add(association);
            _context.SaveChanges();

            return CreatedAtRoute("GetGroup", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGroup(int id, [FromBody] Group item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var group = _context.Groups.FirstOrDefault(t => t.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            group.Name = item.Name;

            _context.Groups.Update(group);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // DELETE: api/Group/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var group = _context.Groups.FirstOrDefault(t => t.Id == id);

            if (group == null) return NotFound();

            _context.Groups.Remove(group);
            _context.SaveChanges();
            return new NoContentResult();
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
