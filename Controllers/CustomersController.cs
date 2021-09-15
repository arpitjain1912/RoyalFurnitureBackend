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
    public class CustomersController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public CustomersController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer([FromQuery] Customer customer)
        {
            IQueryable<Customer> filterresponse;
            filterresponse = _context.Customer.AsQueryable();
            if (customer.CustomerName != null)
            {
                filterresponse = filterresponse.Where(x => x.CustomerName == customer.CustomerName);
            }
            if (customer.CustomerId!=0)
            {
                filterresponse = filterresponse.Where(x => x.CustomerId == customer.CustomerId);
            }
            if (customer.Phone != null)
            {
                filterresponse = filterresponse.Where(x => x.Phone == customer.Phone);
            }
            if (customer.Address != null)
            {
                filterresponse = filterresponse.Where(x => x.Address == customer.Address);
            }
            if (customer.Email != null)
            {
                filterresponse = filterresponse.Where(x => x.Email == customer.Email);
            }
            if (customer.Gstno != null)
            {
                filterresponse = filterresponse.Where(x => x.Gstno == customer.Gstno);
            }
            
            var response = await filterresponse.ToListAsync();
            return Ok(new Response<List<Customer>>(response));
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(new Response<Customer>(customer));
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            DateTime aDate = DateTime.Now;
            customer.AddedAt = aDate;

            int id = 0;
            id = await _context.Customer.MaxAsync(x => (int?)x.CustomerId) ?? 0;
            id++;
            customer.CustomerId = id;
            

            _context.Customer.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }

        /*private IQueryable<T> filter(T order, IQueryable<T> filterresponse)
        {
            return filterresponse;
        }*/
    }
}
