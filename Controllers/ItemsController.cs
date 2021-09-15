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
    public class ItemsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public ItemsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Items>>> GetItems([FromQuery] PaginationFilter page,[FromQuery] Items item)
        {
            IQueryable<Items> filterresponse;
            filterresponse = _context.Items.AsQueryable();
            PaginationFilter p = new PaginationFilter(page.PageNumber, page.PageSize);
            filterresponse =filterresponse.Skip((p.PageNumber - 1) * p.PageSize).Take(p.PageSize).Include("Brand").Include("Category").Include("ChildItem");

            filterresponse=filter(item, filterresponse);
            
            var pagedresponse1 = await filterresponse.ToListAsync();
            var totalcount1 = await filterresponse.CountAsync();
            return Ok(new PagedResponse<List<Items>>(pagedresponse1,totalcount1, p.PageNumber, p.PageSize));
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

            return Ok(new Response<Items>(items));
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
            //Auto update item id(not a key) and insert added at.
            DateTime aDate = DateTime.Now;
            items.AddedAt = aDate;

            int id = 0;
            id = await _context.Items.MaxAsync(x => (int?)x.ItemId) ?? 0;
            id++;
            items.ItemId = id;

            //Auto update brand id and insert added at.
            if (items.BrandId == null )
            {
                items.Brand.AddedAt = aDate;
                int id2 = 0;
                id2 = await _context.Brand.MaxAsync(x => (int?)Convert.ToInt32(x.BrandId)) ?? 0;
                id2++;
                items.Brand.BrandId = Convert.ToString(id2);
            }

            //auto update category id and insert added at.
            if (items.CategoryId == null)
            {
                items.Category.AddedAt = aDate;
                int id3 = 0;
                id3 = await _context.Category.MaxAsync(x => (int?)Convert.ToInt32(x.CategoryId)) ?? 0;
                id3++;
                items.Category.CategoryId = Convert.ToString(id3);
            }

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
            //var parentitem;
            if (items.IsParent==0)
            {
                ChildItem ch=new ChildItem();
                ch.ItemName = items.ParentItemName;
                ch.ChildItemName = items.ItemName;
                ch.NumberOfCopy = items.NumberOfCopy;
                
                _context.ChildItem.Add(ch);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (ChildItemExists(ch.ItemName))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var storelist =await _context.Store.ToListAsync();

            foreach(var s in storelist)
            {
                CurrentStock x = new CurrentStock();
                x.ItemName = items.ItemName;
                x.StoreId = s.StoreId;
                x.TotalQuantityInStore = 0;
                x.TotalQuantityLeft = 0;
                x.AddedAt = aDate;
                _context.CurrentStock.Add(x);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (CurrentStockExists(x.ItemName))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
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

        private bool ChildItemExists(string id)
        {
            return _context.ChildItem.Any(e => e.ItemName == id);
        }

        private bool CurrentStockExists(string id)
        {
            return _context.CurrentStock.Any(e => e.ItemName == id);
        }

        private IQueryable<Items> filter(Items item,IQueryable<Items> filterresponse)
        {
            Console.WriteLine(item.Gstpercent);
            if (item.ItemId != 0)
            {
                filterresponse = filterresponse.Where(x => x.ItemId == item.ItemId);
            }
            if (item.ItemName != null)
            {
                filterresponse = filterresponse.Where(x => x.ItemName == item.ItemName);
            }
            if (item.IsParent != 0)
            {
                filterresponse = filterresponse.Where(x => x.IsParent == item.IsParent);
            }
            if (item.ParentItemName != null)
            {
                filterresponse = filterresponse.Where(x => x.ParentItemName == item.ParentItemName);
            }
            if (item.NumberOfCopy != 0)
            {
                filterresponse = filterresponse.Where(x => x.NumberOfCopy == item.NumberOfCopy);
            }
            if (item.CategoryId != null)
            {
                filterresponse = filterresponse.Where(x => x.CategoryId == item.CategoryId);
            }
            if (item.BrandId != null)
            {
                filterresponse = filterresponse.Where(x => x.BrandId == item.BrandId);
            }
            if (item.Description != null)
            {
                filterresponse = filterresponse.Where(x => x.Description == item.Description);
            }
            if (item.Gstpercent != null)
            {
                filterresponse = filterresponse.Where(x => x.Gstpercent == item.Gstpercent);
            }
            if (item.Hsncode != null)
            {
                filterresponse = filterresponse.Where(x => x.Hsncode == item.Hsncode);
            }
            if (item.AliasCode != null)
            {
                filterresponse = filterresponse.Where(x => x.AliasCode == item.AliasCode);
            }
            if (item.ImageUrl != null)
            {
                filterresponse = filterresponse.Where(x => x.ImageUrl == item.ImageUrl);
            }
            return filterresponse;
        }
    }
}
