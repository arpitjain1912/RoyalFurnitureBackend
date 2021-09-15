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
    public class PurchasesController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public PurchasesController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchase([FromQuery] PaginationFilter page,[FromQuery] Purchase purchase)
        {
            IQueryable<Purchase> filterresponse;
            filterresponse = _context.Purchase.AsQueryable();
            PaginationFilter p = new PaginationFilter(page.PageNumber, page.PageSize);
            filterresponse =  filterresponse.Skip((p.PageNumber - 1) * p.PageSize).Take(p.PageSize).Include("Vendor").Include("PurchaseItem.Store");

            filterresponse = filter(purchase, filterresponse);

            var totalcount = await filterresponse.CountAsync();
            var pagedresponse = await filterresponse.ToListAsync();
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
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase,[FromQuery] int flag)
        {
            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            if (flag == 1)
            {
                var purchaseitems = await _context.PurchaseItem.Where(x => x.PurchaseId == id).ToListAsync();
                foreach (var pi in purchaseitems)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == pi.ItemName && x.StoreId == pi.StoreId);
                    curr_stock.TotalQuantityInStore -= pi.Quantity;
                    curr_stock.TotalQuantityLeft -= pi.Quantity;
                    var costprice = await _context.CostPrice.Where(x => x.CostPriceId == pi.CostPrice && x.ItemName == curr_stock.ItemName && x.StoreId == curr_stock.StoreId).FirstOrDefaultAsync();
                    if (costprice == null)
                    {
                        return BadRequest("Costprice entry was not found for a purchase item in the current purchase");
                    }
                    costprice.Quantityinstore -= pi.Quantity;
                    _context.PurchaseItem.Remove(pi);
                }

                foreach(var pi in purchase.PurchaseItem)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == pi.ItemName && x.StoreId == pi.StoreId);
                    curr_stock.TotalQuantityInStore += pi.Quantity;
                    curr_stock.TotalQuantityLeft += pi.Quantity;
                    var costprice = await _context.CostPrice.Where(x => x.CostPriceId == pi.CostPrice && x.ItemName == curr_stock.ItemName && x.StoreId == curr_stock.StoreId).FirstOrDefaultAsync();
                    if (costprice != null)
                    {
                        costprice.Quantityinstore += pi.Quantity;
                    }
                    else
                    {
                        var test = new CostPrice();
                        test.CostPriceId = pi.CostPrice;
                        test.ItemName = pi.ItemName;
                        test.StoreId = pi.StoreId;
                        test.Quantityinstore = pi.Quantity;
                        test.AddedAt = DateTime.Now;
                        test.ModifiedAt = DateTime.Now;
                        _context.CostPrice.Add(test);
                    }
                    _context.PurchaseItem.Add(pi);
                }
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

            if (purchase.PurchaseDate == null)
            {
                purchase.PurchaseDate = aDate;
            }

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

        private IQueryable<Purchase> filter(Purchase purchase, IQueryable<Purchase> filterresponse)
        {
            if (purchase.PurchaseId != 0)
            {
                filterresponse = filterresponse.Where(x => x.PurchaseId == purchase.PurchaseId);
            }
            if (purchase.PurchaseInvoice != null)
            {
                filterresponse = filterresponse.Where(x => x.PurchaseInvoice == purchase.PurchaseInvoice);
            }
            if (purchase.IsGst != 0)
            {
                filterresponse = filterresponse.Where(x => x.IsGst == purchase.IsGst);
            }
            if (purchase.VendorId != 0)
            {
                filterresponse = filterresponse.Where(x => x.VendorId == purchase.VendorId);
            }
            if (purchase.Amount != null)
            {
                filterresponse = filterresponse.Where(x => x.Amount == purchase.Amount);
            }
            if (purchase.PurchaseDate != null)
            {
                filterresponse = filterresponse.Where(x => x.PurchaseDate == purchase.PurchaseDate);
            }
            if (purchase.Status != null)
            {
                filterresponse = filterresponse.Where(x => x.Status == purchase.Status);
            }
            return filterresponse;
        }
    }
}
