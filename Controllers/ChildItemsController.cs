using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildItemsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public ChildItemsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/ChildItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChildItem>>> GetChildItem()
        {
            return await _context.ChildItem.ToListAsync();
        }

        // GET: api/ChildItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChildItem>> GetChildItem(string id)
        {
            var childItem = await _context.ChildItem.FindAsync(id);

            if (childItem == null)
            {
                return NotFound();
            }

            return childItem;
        }

        // PUT: api/ChildItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChildItem(string id, ChildItem childItem)
        {
            if (id != childItem.ItemName)
            {
                return BadRequest();
            }

            _context.Entry(childItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChildItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChildItem>> PostChildItem(ChildItem childItem)
        {
            _context.ChildItem.Add(childItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChildItemExists(childItem.ItemName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChildItem", new { id = childItem.ItemName }, childItem);
        }

        // DELETE: api/ChildItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChildItem>> DeleteChildItem(string id)
        {
            var childItem = await _context.ChildItem.FindAsync(id);
            if (childItem == null)
            {
                return NotFound();
            }

            _context.ChildItem.Remove(childItem);
            await _context.SaveChangesAsync();

            return childItem;
        }

        private bool ChildItemExists(string id)
        {
            return _context.ChildItem.Any(e => e.ItemName == id);
        }
    }
}
