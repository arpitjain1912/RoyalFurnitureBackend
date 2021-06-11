using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesTransactionsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public SalesTransactionsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/SalesTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesTransaction>>> GetSalesTransaction()
        {
            return await _context.SalesTransaction.ToListAsync();
        }

        // GET: api/SalesTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesTransaction>> GetSalesTransaction(int id)
        {
            var salesTransaction = await _context.SalesTransaction.FindAsync(id);

            if (salesTransaction == null)
            {
                return NotFound();
            }

            return salesTransaction;
        }

        // PUT: api/SalesTransactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesTransaction(int id, SalesTransaction salesTransaction)
        {
            if (id != salesTransaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(salesTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesTransactionExists(id))
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

        // POST: api/SalesTransactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SalesTransaction>> PostSalesTransaction(SalesTransaction salesTransaction)
        {
            _context.SalesTransaction.Add(salesTransaction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesTransactionExists(salesTransaction.TransactionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSalesTransaction", new { id = salesTransaction.TransactionId }, salesTransaction);
        }

        // DELETE: api/SalesTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SalesTransaction>> DeleteSalesTransaction(int id)
        {
            var salesTransaction = await _context.SalesTransaction.FindAsync(id);
            if (salesTransaction == null)
            {
                return NotFound();
            }

            _context.SalesTransaction.Remove(salesTransaction);
            await _context.SaveChangesAsync();

            return salesTransaction;
        }

        private bool SalesTransactionExists(int id)
        {
            return _context.SalesTransaction.Any(e => e.TransactionId == id);
        }
    }
}
