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
    public class OrdersController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public OrdersController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder([FromQuery] PaginationFilter page,[FromQuery] Order order)
        {
            IQueryable<Order> filterresponse;

            filterresponse = _context.Order.AsQueryable();
            PaginationFilter p = new PaginationFilter(page.PageNumber, page.PageSize);
            filterresponse = filterresponse.Skip((p.PageNumber - 1) * p.PageSize).Take(p.PageSize).Include("Staff").Include("Customer").Include("OrderItem").Include("SalesTransaction");

            filterresponse = filter(order, filterresponse);

            var pagedresponse = await filterresponse.ToListAsync();
            var totalcount = await filterresponse.CountAsync();
            return Ok(new PagedResponse<List<Order>>(pagedresponse, totalcount, p.PageNumber, p.PageSize));
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(new Response<Order>(order));
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order,[FromQuery] int flag)
        {
            if (id != order.OrderId)
            {
                return BadRequest("Id is not equal to order.OrderId");
            }

            DateTime aDate = DateTime.Now;
            order.ModifiedAt = aDate;

            if (order.OrderItem != null&&flag==1)
            {
                var orderitems = await _context.OrderItem.Where(x => x.OrderId == id).ToListAsync();
                foreach(var oi in orderitems)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == oi.ItemName && x.StoreId == oi.StoreId);
                    curr_stock.TotalQuantityLeft += oi.Quantity;
                    _context.OrderItem.Remove(oi);
                }

                foreach (var oi in order.OrderItem)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == oi.ItemName && x.StoreId == oi.StoreId);
                    curr_stock.TotalQuantityLeft -= oi.Quantity;
                    oi.OrderId = order.OrderId;
                    _context.OrderItem.Add(oi);
                }
            }

            var oldEntry = await _context.Order.FindAsync(id);

            if (order.Status == "D"&&oldEntry.Status=="P")
            {
                var orderitems = await _context.OrderItem.Where(x => x.OrderId == id).ToListAsync();
                foreach (var oi in orderitems)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == oi.ItemName && x.StoreId == oi.StoreId);
                    
                    if (curr_stock == null || curr_stock.TotalQuantityInStore < oi.Quantity)
                    {
                        return BadRequest("Not Enough copies of Item: "+ oi.ItemName + " are a available in store. Please purchase some more copies to deliver this order."); //******************** custom error message.
                    }
                    curr_stock.TotalQuantityInStore -= oi.Quantity;
                    
                    int temp = oi.Quantity;
                    
                    var costprices = await _context.CostPrice.Where(x => x.ItemName == curr_stock.ItemName && x.StoreId == curr_stock.StoreId).Where(x=>x.Quantityinstore>0).OrderBy(x => x.CostPriceId).ToListAsync();

                    System.Diagnostics.Debug.WriteLine(costprices.Count());
                    foreach (var cp in costprices)
                    {
                        System.Diagnostics.Debug.WriteLine(cp.Quantityinstore);
                        if (cp.Quantityinstore <= temp)
                        { 
                            
                            var costpriceassign = new CostPriceAssigned();
                            costpriceassign.AddedAt = aDate;
                            costpriceassign.OrderItem = oi;
                            costpriceassign.Quantity = cp.Quantityinstore;
                            costpriceassign.CostPrice = cp.CostPriceId;
                            //check if costpriceassign already exists. If it does just add the quantity in already existing entry.
                            var check = await _context.CostPriceAssigned.FindAsync(costpriceassign.ItemName, costpriceassign.OrderId, costpriceassign.StoreId, costpriceassign.CostPrice);
                            if (check != null)
                            {
                                check.Quantity += cp.Quantityinstore;
                            }
                            else
                            {
                                oi.CostPriceAssigned.Add(costpriceassign);
                            }
                            temp -= cp.Quantityinstore;
                            cp.Quantityinstore = 0;
                        }
                        else
                        {
                            cp.Quantityinstore -=temp;
                            var costpriceassign = new CostPriceAssigned();
                            costpriceassign.AddedAt = aDate;
                            costpriceassign.OrderItem = oi;
                            costpriceassign.Quantity = temp;
                            costpriceassign.CostPrice = cp.CostPriceId;
                            //check if costpriceassign already exists. If it does just add the quantity in already existing entry.
                            var check = await _context.CostPriceAssigned.FindAsync(costpriceassign.ItemName,costpriceassign.OrderId,costpriceassign.StoreId,costpriceassign.CostPrice);
                            if (check != null)
                            {
                                check.Quantity += temp;
                            }
                            else
                            {
                                oi.CostPriceAssigned.Add(costpriceassign);
                            }
                            temp = 0;
                        }
                        if (temp == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (order.Status == "P" && oldEntry.Status == "D")
            {
                var orderitems = await _context.OrderItem.Where(x => x.OrderId == id).ToListAsync();
                foreach (var oi in orderitems)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == oi.ItemName && x.StoreId == oi.StoreId);
                    curr_stock.TotalQuantityInStore += oi.Quantity;

                    var costpriceassigned = await _context.CostPriceAssigned.Where(x => x.ItemName == oi.ItemName && x.StoreId == oi.StoreId && x.OrderId == oi.OrderId).ToListAsync();
                    foreach(var cpa in costpriceassigned)
                    {
                        var costprice = await _context.CostPrice.Where(x => x.CostPriceId == cpa.CostPrice&&x.ItemName==oi.ItemName&&x.StoreId==oi.StoreId).FirstOrDefaultAsync();
                        costprice.Quantityinstore += cpa.Quantity;
                        _context.CostPriceAssigned.Remove(cpa);
                    }
                }
            }

            if(order.Status == "C" && oldEntry.Status == "P")
            {
                var orderitems = await _context.OrderItem.Where(x => x.OrderId == id).ToListAsync();
                foreach(var oi in orderitems)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == oi.ItemName && x.StoreId == oi.StoreId);
                    curr_stock.TotalQuantityLeft += oi.Quantity;
                }
            }

            if (order.Status == "P" && oldEntry.Status == "C")
            {
                var orderitems = await _context.OrderItem.Where(x => x.OrderId == id).ToListAsync();
                foreach (var oi in orderitems)
                {
                    var curr_stock = await _context.CurrentStock.FirstOrDefaultAsync(x => x.ItemName == oi.ItemName && x.StoreId == oi.StoreId);
                    curr_stock.TotalQuantityLeft -= oi.Quantity;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _context.Entry(oldEntry).State = EntityState.Detached;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            DateTime aDate = DateTime.Now;
            int id = 0;
            id = await _context.Order.MaxAsync(x => (int?)x.OrderId) ?? 0;
            id++;

            order.AddedAt = aDate;
            order.ModifiedAt = aDate;
            order.OrderId = id;
            order.Status = "P";

            foreach (var o in order.OrderItem)
            {
                o.AddedAt = aDate;
            }

            _context.Order.Add(order);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var orderitems = order.OrderItem;

            foreach(var oi in orderitems)
            {
                var curr_stock = await _context.CurrentStock.FindAsync(oi.ItemName, oi.StoreId);
                curr_stock.TotalQuantityLeft -= oi.Quantity;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        private IQueryable<Order> filter(Order order,IQueryable<Order> filterresponse)
        {
            if (order.OrderId != 0)
            {
                filterresponse = filterresponse.Where(x => x.OrderId == order.OrderId);
            }
            if (order.OrderInvoice != null)
            {
                filterresponse = filterresponse.Where(x => x.OrderInvoice == order.OrderInvoice);
            }
            if(order.IsAdjustment != 0)
            {
                filterresponse = filterresponse.Where(x => x.IsAdjustment == order.IsAdjustment);
            }
            if (order.IsGst != 0)
            {
                filterresponse = filterresponse.Where(x => x.IsGst == order.IsGst);
            }
            if (order.CustomerId != 0)
            {
                filterresponse = filterresponse.Where(x => x.CustomerId == order.CustomerId);
            }
            if (order.StaffId != 0)
            {
                filterresponse = filterresponse.Where(x => x.StaffId == order.StaffId);
            }
            if (order.Amount != 0)
            {
                filterresponse = filterresponse.Where(x => x.Amount == order.Amount);
            }
            if (order.Gstpercent != 0)
            {
                filterresponse = filterresponse.Where(x => x.Gstpercent == order.Gstpercent);
            }
            if (order.DiscountPercent != null)
            {
                filterresponse = filterresponse.Where(x => x.DiscountPercent == order.DiscountPercent);
            }
            return filterresponse;
        }
    }
}
