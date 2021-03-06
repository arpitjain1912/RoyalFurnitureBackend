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
    public class VendorsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public VendorsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendor([FromQuery] Vendor vendor)
        {
            IQueryable<Vendor> filterresponse;
            filterresponse = _context.Vendor.AsQueryable();
            if (vendor.VendorId != 0)
            {
                filterresponse = filterresponse.Where(x => x.VendorId == vendor.VendorId);
            }
            if (vendor.VendorName != null)
            {
                filterresponse = filterresponse.Where(x => x.VendorName == vendor.VendorName);
            }
            if (vendor.Phone != null)
            {
                filterresponse = filterresponse.Where(x => x.Phone == vendor.Phone);
            }
            if (vendor.Address != null)
            {
                filterresponse = filterresponse.Where(x => x.Address == vendor.Address);
            }
            if (vendor.Gstnumber != null)
            {
                filterresponse = filterresponse.Where(x => x.Gstnumber == vendor.Gstnumber);
            }
            if (vendor.Email != null)
            {
                filterresponse = filterresponse.Where(x => x.Email == vendor.Email);
            }
            var response = await filterresponse.ToListAsync();
            return Ok(new Response<List<Vendor>>(response));
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var vendor = await _context.Vendor.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return Ok(new Response<Vendor>(vendor));
        }

        // PUT: api/Vendors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.VendorId)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: api/Vendors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            DateTime aDate = DateTime.Now;
            vendor.AddedAt = aDate;

            int id = 0;
            id = await _context.Vendor.MaxAsync(x => (int?)x.VendorId) ?? 0;
            id++;

            vendor.VendorId = id;

            _context.Vendor.Add(vendor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VendorExists(vendor.VendorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVendor", new { id = vendor.VendorId }, vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vendor>> DeleteVendor(int id)
        {
            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendor.Remove(vendor);
            await _context.SaveChangesAsync();

            return vendor;
        }

        private bool VendorExists(int id)
        {
            return _context.Vendor.Any(e => e.VendorId == id);
        }

        private IQueryable<Order> filter(Order order, IQueryable<Order> filterresponse)
        {
            return filterresponse;
        }
    }
}
