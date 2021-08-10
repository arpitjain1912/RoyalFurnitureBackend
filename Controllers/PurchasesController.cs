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
    public class PurchasesController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public PurchasesController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchase([FromQuery] PaginationFilter page)
        {
            PaginationFilter p = new PaginationFilter(page.PageNumber, page.PageSize);
            var pagedresponse = await _context.Purchase.Skip((p.PageNumber - 1) * p.PageSize).Take(p.PageSize).Include("Vendor").Include("PurchaseItem.Store").ToListAsync();
            var totalcount = await _context.Purchase.CountAsync();
            return Ok(new PagedResponse<List<Purchase>>(pagedresponse, totalcount, p.PageNumber, p.PageSize));
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(new Response<Purchase>(purchase));
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        {
            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            DateTime aDate = DateTime.Now;
            int id = 0;
            id = await _context.Purchase.MaxAsync(x => (int?)x.PurchaseId) ?? 0;
            id++;
            purchase.PurchaseId = id;
            purchase.AddedAt = aDate;

            foreach(var p in purchase.PurchaseItem)
            {
                p.AddedAt = aDate;
            }

            _context.Purchase.Add(purchase);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PurchaseExists(purchase.PurchaseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //Add purchase items into CurrentStock
            var purchaseitems = purchase.PurchaseItem;

            foreach(var p in purchaseitems)
            {
                var c1 = await _context.CostPrice.FindAsync(p.CostPrice, p.ItemName, p.StoreId);
                var curr_stock = await _context.CurrentStock.FindAsync(p.ItemName, p.StoreId);

                if (c1 != null)
                {
                    c1.Quantityinstore += p.Quantity;
                }
                else
                {
                    var c = new CostPrice();
                    c.CostPriceId = p.CostPrice;
                    c.ItemName = p.ItemName;
                    c.StoreId = p.StoreId;
                    c.Quantityinstore = p.Quantity;
                    c.AddedAt = aDate;
                    _context.CostPrice.Add(c);
                }
                curr_stock.TotalQuantityInStore += p.Quantity;
                curr_stock.TotalQuantityLeft += p.Quantity;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (PurchaseExists(purchase.PurchaseId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

            }

            return CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Purchase>> DeletePurchase(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchase.Remove(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.PurchaseId == id);
        }
    }
}
