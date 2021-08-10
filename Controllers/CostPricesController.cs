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

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostPricesController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public CostPricesController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/CostPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostPrice>>> GetCostPrice()
        {
            var response = await _context.CostPrice.ToListAsync();
            return Ok(new Response<List<CostPrice>>(response));
        }

        // GET: api/CostPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CostPrice>> GetCostPrice(int id)
        {
            var costPrice = await _context.CostPrice.FindAsync(id);

            if (costPrice == null)
            {
                return NotFound();
            }

            return Ok(new Response<CostPrice>(costPrice));
        }

        // PUT: api/CostPrices/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCostPrice(int id, CostPrice costPrice)
        {
            if (id != costPrice.CostPriceId)
            {
                return BadRequest();
            }

            _context.Entry(costPrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostPriceExists(id))
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

        // POST: api/CostPrices
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CostPrice>> PostCostPrice(CostPrice costPrice)
        {
            DateTime aDate = DateTime.Now;
            costPrice.AddedAt = aDate;

            /*int id = 0;
            id = await _context.CostPrice.Where(c=> c.ItemName==costPrice.ItemName && c.StoreId==costPrice.StoreId).MaxAsync(x=>(int?)x.CostPriceId)?? 0;
            id++;*/

            //costPrice.CostPriceId = id;
            _context.CostPrice.Add(costPrice);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CostPriceExists(costPrice.CostPriceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCostPrice", new { id = costPrice.CostPriceId }, costPrice);
        }

        // DELETE: api/CostPrices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CostPrice>> DeleteCostPrice(int id)
        {
            var costPrice = await _context.CostPrice.FindAsync(id);
            if (costPrice == null)
            {
                return NotFound();
            }

            _context.CostPrice.Remove(costPrice);
            await _context.SaveChangesAsync();

            return costPrice;
        }

        private bool CostPriceExists(int id)
        {
            return _context.CostPrice.Any(e => e.CostPriceId == id);
        }
    }
}
