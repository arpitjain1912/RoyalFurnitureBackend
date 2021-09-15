using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Filters;
using WebApplication1.Wrappers;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<CurrentStock>>> GetCurrentStock([FromQuery] CurrentStock currentstock)
        {
            IQueryable<CurrentStock> filterresponse;
            filterresponse = _context.CurrentStock.AsQueryable();
            if (currentstock.ItemName != null)
            {
                filterresponse = filterresponse.Where(x => x.ItemName == currentstock.ItemName);
            }
            if (currentstock.StoreId != 0)
            {
                filterresponse = filterresponse.Where(x => x.StoreId == currentstock.StoreId);
            }
            if (currentstock.CostPriceId != 0)
            {
                filterresponse = filterresponse.Where(x => x.CostPriceId == currentstock.CostPriceId);
            }
            if (currentstock.TotalQuantityInStore != 0)
            {
                filterresponse = filterresponse.Where(x => x.TotalQuantityInStore == currentstock.TotalQuantityInStore);
            }
            if (currentstock.TotalQuantityLeft != 0)
            {
                filterresponse = filterresponse.Where(x => x.TotalQuantityLeft == currentstock.TotalQuantityLeft);
            }
            var response = await filterresponse.Include("CostPrice").ToListAsync();
            return Ok(new Response<List<CurrentStock>>(response));
        }

        // GET: api/CurrentStocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrentStock>> GetCurrentStock(string id,int x)
        {
            //here int x is added just to differentiate the two get definations. x will be remove later once more filter parameters will be added above.
            var currentStock = await _context.CurrentStock.FindAsync(id);

            if (currentStock == null)
            {
                return NotFound();
            }

            return Ok(new Response<CurrentStock>(currentStock));
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

        private IQueryable<Order> filter(Order order, IQueryable<Order> filterresponse)
        {
            return filterresponse;
        }
    }
}
