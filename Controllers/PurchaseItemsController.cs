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
    public class PurchaseItemsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public PurchaseItemsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseItem>>> GetPurchaseItem([FromQuery] PurchaseItem purchaseitem)
        {
            IQueryable<PurchaseItem> filterresponse;
            filterresponse = _context.PurchaseItem.AsQueryable();
            if (purchaseitem.PurchaseId != 0)
            {
                filterresponse = filterresponse.Where(x => x.PurchaseId == purchaseitem.PurchaseId);
            }
            if (purchaseitem.StoreId != 0)
            {
                filterresponse = filterresponse.Where(x => x.StoreId == purchaseitem.StoreId);
            }
            if (purchaseitem.Quantity != 0)
            {
                filterresponse = filterresponse.Where(x => x.Quantity == purchaseitem.Quantity);
            }
            if (purchaseitem.CostPrice != 0)
            {
                filterresponse = filterresponse.Where(x => x.CostPrice == purchaseitem.CostPrice);
            }
            if (purchaseitem.ItemName != null)
            {
                filterresponse = filterresponse.Where(x => x.ItemName == purchaseitem.ItemName);
            }
            var response = await filterresponse.Include("Store").Include("ItemNameNavigation").ToListAsync();
            return Ok(new Response<List<PurchaseItem>>(response));
        }

        // GET: api/PurchaseItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseItem>> GetPurchaseItem(int id)
        {
            var purchaseItem = await _context.PurchaseItem.FindAsync(id);

            if (purchaseItem == null)
            {
                return NotFound();
            }

            return Ok(new Response<PurchaseItem>(purchaseItem));
        }

        // PUT: api/PurchaseItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseItem(int id, PurchaseItem purchaseItem)
        {
            if (id != purchaseItem.PurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(purchaseItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseItemExists(id))
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

        // POST: api/PurchaseItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PurchaseItem>> PostPurchaseItem(PurchaseItem purchaseItem)
        {
            _context.PurchaseItem.Add(purchaseItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PurchaseItemExists(purchaseItem.PurchaseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }



            return CreatedAtAction("GetPurchaseItem", new { id = purchaseItem.PurchaseId }, purchaseItem);
        }

        // DELETE: api/PurchaseItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseItem>> DeletePurchaseItem(int id)
        {
            var purchaseItem = await _context.PurchaseItem.FindAsync(id);
            if (purchaseItem == null)
            {
                return NotFound();
            }

            _context.PurchaseItem.Remove(purchaseItem);
            await _context.SaveChangesAsync();

            return purchaseItem;
        }

        private bool PurchaseItemExists(int id)
        {
            return _context.PurchaseItem.Any(e => e.PurchaseId == id);
        }

        private IQueryable<Order> filter(Order order, IQueryable<Order> filterresponse)
        {
            return filterresponse;
        }
    }
}
