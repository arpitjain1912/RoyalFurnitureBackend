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
    public class StaffsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public StaffsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/Staffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaff([FromQuery] Staff staff)
        {
            IQueryable<Staff> filterresponse;
            filterresponse = _context.Staff.AsQueryable();
            if (staff.StaffId != 0)
            {
                filterresponse = filterresponse.Where(x => x.StaffId == staff.StaffId);
            }
            if (staff.Phone != null)
            {
                filterresponse = filterresponse.Where(x => x.Phone == staff.Phone);
            }
            if (staff.Address != null)
            {
                filterresponse = filterresponse.Where(x => x.Address == staff.Address);
            }
            if (staff.StaffName != null)
            {
                filterresponse = filterresponse.Where(x => x.StaffName == staff.StaffName);
            }
            var response = await filterresponse.ToListAsync();
            return Ok(new Response<List<Staff>>(response));
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Staff>> GetStaff(int id)
        {
            var staff = await _context.Staff.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return Ok(new Response<Staff>(staff));
        }

        // PUT: api/Staffs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, Staff staff)
        {
            if (id != staff.StaffId)
            {
                return BadRequest();
            }

            _context.Entry(staff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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

        // POST: api/Staffs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Staff>> PostStaff(Staff staff)
        {
            DateTime aDate = DateTime.Now;
            staff.AddedAt = aDate;

            int id = 0;
            id = await _context.Staff.MaxAsync(x => (int?)x.StaffId) ?? 0;
            id++;
            staff.StaffId = id;

            _context.Staff.Add(staff);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StaffExists(staff.StaffId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStaff", new { id = staff.StaffId }, staff);
        }

        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Staff>> DeleteStaff(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();

            return staff;
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.StaffId == id);
        }

        private IQueryable<Order> filter(Order order, IQueryable<Order> filterresponse)
        {
            return filterresponse;
        }
    }
}
