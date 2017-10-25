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
        public IActionResult GetById(Guid id)
        {
            var item = _context.Channels.FirstOrDefault(t => t.ID == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] Channel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Channels.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetChannel", new { id = item.ID }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Channel item)
        {
            if(item == null || item.ID != id)
            {
                return BadRequest();
            }

            var channel = _context.Channels.FirstOrDefault(t => t.ID == id);

            if (channel == null)
            {
                return NotFound();
            }

            channel.Name = item.Name;

            _context.Channels.Update(channel);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // DELETE: api/Channel/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var channel = _context.Channels.FirstOrDefault(t => t.ID == id);

            if (channel == null) return NotFound();

            _context.Channels.Remove(channel);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
