using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BachelorModelViewController.Models;

namespace BachelorModelViewController.Controllers
{
    [Produces("application/json")]
    [Route("api/Channel")]
    public class ChannelController : Controller
    {
        private readonly ChannelContext _context;

        public ChannelController(ChannelContext context)
        {
            _context = context;

            if(_context.Channels.Count() == 0)
            {
                _context.Channels.Add(new Channel { Name = "SimpleChannel" });
                _context.SaveChanges();
            }
        }
        // GET: api/Channel
        [HttpGet]
        public IEnumerable<Channel> GetAll()
        {
            return _context.Channels.ToList();
        }

        // GET: api/Channel/5
        [HttpGet("{id}", Name = "GetChannel")]
        public IActionResult GetById(int id)
        {
            var item = _context.Channels.FirstOrDefault(t => t.ID == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        
        // POST: api/Channel
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Channel/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
