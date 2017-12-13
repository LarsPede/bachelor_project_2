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
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using BachelorModelViewController.Interfaces;

namespace BachelorModelViewController.Controllers
{
    public interface IAPIController
    {

    }

    [Produces("application/json")]
    [Route("api")]
    public class APIController : Controller, IAPIController
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMongoOperations _mongoOperations;

        public APIController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMongoOperations mongoOperations)
        {
            _context = context;
            _mongoOperations = mongoOperations;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        // GET: api/Groups
        [HttpGet]
        public IEnumerable<Group> GetAll()
        {
            return _context.Groups.ToList();
        }

        // GET: api/get_mongo_collection/{collectionName}
        [Route("get_mongo_collection/{name}")]
        [HttpGet]
        public JsonResult GetMongoCollection(string name)
        {
            var json = GetAllFromCollectionInternal(name).Result;

            return Json(json);
        }

        private async Task<List<BsonDocument>> GetAllFromCollectionInternal(string collectionName)
        {
            return await _mongoOperations.GetAllFromCollection(collectionName);
        }

        //// GET: api/get_mongo_collection_from/{time}/{collectionName}
        //[Route("get_mongo_collection_from"), HttpGet("{time}/{name}", Name = "getAllFromMongoCollectionFrom")]
        //public async Task<IActionResult> GetMongoCollectionFrom(DateTime time, string name)
        //{
        //    var collection = _mongoContext.GetMongoCollection(name);
        //    var list = await collection.Find(_ => true).ToListAsync();
        //    return Json(list);
        //    //var result = await collection.Find(_ => true).ToListAsync(); //collection.Find(_ => true).ToCursor(); //.AsQueryable().ToList();
        //    //return Json(result);
        //}

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
