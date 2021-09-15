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
    public class OrderItemsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public OrderItemsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItem([FromQuery] OrderItem orderitem)
        {
            IQueryable<OrderItem> filterresponse;
            filterresponse = _context.OrderItem.AsQueryable();
            filterresponse = filter(orderitem, filterresponse);
            var response = await filterresponse.Include("Store").Include("ItemNameNavigation").Include("CostPriceAssigned").ToListAsync();
            return Ok(new Response<List<OrderItem>>(response));
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(string id)
        {
            var orderItem = await _context.OrderItem.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(new Response<OrderItem>(orderItem));
        }

        // PUT: api/OrderItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(string id, OrderItem orderItem)
        {
            if (id != orderItem.ItemName)
            {
                return BadRequest();
            }
            if (!OrderItemExists(id))
            {
                return NotFound();
            }
            var oldEntry = await _context.OrderItem.FindAsync(id);

            var curr_stock = await _context.CurrentStock.FindAsync(orderItem.ItemName, orderItem.StoreId);
            curr_stock.TotalQuantityLeft += oldEntry.Quantity;
            curr_stock.TotalQuantityLeft -= orderItem.Quantity;

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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

        // POST: api/OrderItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            DateTime aDate = DateTime.Now;
            orderItem.AddedAt = aDate;
            orderItem.ModifiedAt = aDate;

            var curr_stock = await _context.CurrentStock.FindAsync(orderItem.ItemName, orderItem.StoreId);
            curr_stock.TotalQuantityLeft -= orderItem.Quantity;

            _context.OrderItem.Add(orderItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderItemExists(orderItem.ItemName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderItem", new { id = orderItem.ItemName }, orderItem);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderItem>> DeleteOrderItem(string id)
        {
            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            var curr_stock = await _context.CurrentStock.FindAsync(orderItem.ItemName, orderItem.StoreId);
            curr_stock.TotalQuantityLeft += orderItem.Quantity;

            _context.OrderItem.Remove(orderItem);
            await _context.SaveChangesAsync();

            return orderItem;
        }

        private bool OrderItemExists(string id)
        {
            return _context.OrderItem.Any(e => e.ItemName == id);
        }

        private IQueryable<OrderItem> filter(OrderItem orderitem, IQueryable<OrderItem> filterresponse)
        {
            if (orderitem.ItemName != null)
            {
                filterresponse = filterresponse.Where(x => x.ItemName == orderitem.ItemName);
            }
            if (orderitem.OrderId != 0)
            {
                filterresponse = filterresponse.Where(x => x.OrderId == orderitem.OrderId);
            }
            if (orderitem.StoreId != 0)
            {
                filterresponse = filterresponse.Where(x => x.StoreId == orderitem.StoreId);
            }
            if (orderitem.Quantity != 0)
            {
                filterresponse = filterresponse.Where(x => x.Quantity == orderitem.Quantity);
            }
            if (orderitem.CostPrice != 0)
            {
                filterresponse = filterresponse.Where(x => x.CostPrice == orderitem.CostPrice);
            }
            if (orderitem.SellingPrice != 0)
            {
                filterresponse = filterresponse.Where(x => x.SellingPrice == orderitem.SellingPrice);
            }
            return filterresponse;
        }
    }
}
