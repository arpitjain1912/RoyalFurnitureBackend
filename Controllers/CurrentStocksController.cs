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
    public class CurrentStocksController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public CurrentStocksController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/CurrentStocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrentStock>>> GetCurrentStock()
        {
            return await _context.CurrentStock.ToListAsync();
        }

        // GET: api/CurrentStocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrentStock>> GetCurrentStock(string id)
        {
            var currentStock = await _context.CurrentStock.FindAsync(id);

            if (currentStock == null)
            {
                return NotFound();
            }

            return currentStock;
        }

        // PUT: api/CurrentStocks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrentStock(string id, CurrentStock currentStock)
        {
            if (id != currentStock.ItemName)
            {
                return BadRequest();
            }

            _context.Entry(currentStock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrentStockExists(id))
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

        // POST: api/CurrentStocks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CurrentStock>> PostCurrentStock(CurrentStock currentStock)
        {
            _context.CurrentStock.Add(currentStock);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CurrentStockExists(currentStock.ItemName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCurrentStock", new { id = currentStock.ItemName }, currentStock);
        }

        // DELETE: api/CurrentStocks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CurrentStock>> DeleteCurrentStock(string id)
        {
            var currentStock = await _context.CurrentStock.FindAsync(id);
            if (currentStock == null)
            {
                return NotFound();
            }

            _context.CurrentStock.Remove(currentStock);
            await _context.SaveChangesAsync();

            return currentStock;
        }

        private bool CurrentStockExists(string id)
        {
            return _context.CurrentStock.Any(e => e.ItemName == id);
        }
    }
}
