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
    public class StoresController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public StoresController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Stores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStore([FromQuery] Store store)
        {
            IQueryable<Store> filterresponse;
            filterresponse = _context.Store.AsQueryable();
            if (store.StoreId != 0)
            {
                filterresponse = filterresponse.Where(x => x.StoreId == store.StoreId);
            }
            if (store.StoreName != null)
            {
                filterresponse = filterresponse.Where(x => x.StoreName == store.StoreName);
            }
            if (store.Phone != null)
            {
                filterresponse = filterresponse.Where(x => x.Phone == store.Phone);
            }
            if (store.Address != null)
            {
                filterresponse = filterresponse.Where(x => x.Address == store.Address);
            }
            var response = await filterresponse.ToListAsync();
            return Ok(new Response<List<Store>>(response));
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _context.Store.FindAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            return Ok(new Response<Store>(store));
        }

        // PUT: api/Stores/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            if (id != store.StoreId)
            {
                return BadRequest();
            }

            _context.Entry(store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
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

        // POST: api/Stores
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Store>> PostStore(Store store)
        {
            DateTime aDate = DateTime.Now;
            store.AddedAt = aDate;

            int id = 0;
            id = await _context.Store.MaxAsync(x => (int?)x.StoreId) ?? 0;
            id++;

            store.StoreId = id;

            _context.Store.Add(store);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoreExists(store.StoreId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var itemlist =await _context.Items.ToListAsync();

            foreach(var i in itemlist)
            {
                CurrentStock x = new CurrentStock();
                x.ItemName = i.ItemName;
                x.StoreId = store.StoreId;
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

            return CreatedAtAction("GetStore", new { id = store.StoreId }, store);
        }

        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Store>> DeleteStore(int id)
        {
            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Store.Remove(store);
            await _context.SaveChangesAsync();

            return store;
        }

        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.StoreId == id);
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
