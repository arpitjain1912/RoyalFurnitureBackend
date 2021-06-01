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
    public class ItemsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public ItemsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Items>>> GetItems()
        {
            return await _context.Items.Include("Brand").ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Items>> GetItems(string id)
        {
            var items = await _context.Items.FindAsync(id);

            if (items == null)
            {
                return NotFound();
            }

            return items;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItems(string id, Items items)
        {
            if (id != items.ItemName)
            {
                return BadRequest();
            }

            _context.Entry(items).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemsExists(id))
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

        // POST: api/Items
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Items>> PostItems(Items items)
        {
            _context.Items.Add(items);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ItemsExists(items.ItemName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetItems", new { id = items.ItemName }, items);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Items>> DeleteItems(string id)
        {
            var items = await _context.Items.FindAsync(id);
            if (items == null)
            {
                return NotFound();
            }

            _context.Items.Remove(items);
            await _context.SaveChangesAsync();

            return items;
        }

        private bool ItemsExists(string id)
        {
            return _context.Items.Any(e => e.ItemName == id);
        }
    }
}
