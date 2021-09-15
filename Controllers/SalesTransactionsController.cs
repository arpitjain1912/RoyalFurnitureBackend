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
    public class SalesTransactionsController : ControllerBase
    {
        private readonly royalfurnitureDBContext _context;

        public SalesTransactionsController(royalfurnitureDBContext context)
        {
            _context = context;
        }

        // GET: api/SalesTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesTransaction>>> GetSalesTransaction([FromQuery] SalesTransaction salestransaction)
        {
            IQueryable<SalesTransaction> filterresponse;
            filterresponse = _context.SalesTransaction.AsQueryable();
            if (salestransaction.TransactionId != 0)
            {
                filterresponse = filterresponse.Where(x => x.TransactionId == salestransaction.TransactionId);
            }
            if (salestransaction.OrderId != 0)
            {
                filterresponse = filterresponse.Where(x => x.OrderId == salestransaction.OrderId);
            }
            if (salestransaction.ModeOfPayment != 0)
            {
                filterresponse = filterresponse.Where(x => x.ModeOfPayment == salestransaction.ModeOfPayment);
            }
            if (salestransaction.AmountPaid != 0)
            {
                filterresponse = filterresponse.Where(x => x.AmountPaid == salestransaction.AmountPaid);
            }
            var response = await filterresponse.ToListAsync();
            return Ok(new Response<List<SalesTransaction>>(response));
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

            return Ok(new Response<SalesTransaction>(salesTransaction));
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
            DateTime aDate = DateTime.Now;
            salesTransaction.AddedAt = aDate;

            int id = 0;
            id = await _context.SalesTransaction.MaxAsync(x => (int?)x.TransactionId) ?? 0;
            id++;

            salesTransaction.TransactionId = id;

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

        private IQueryable<Order> filter(Order order, IQueryable<Order> filterresponse)
        {
            return filterresponse;
        }
    }
}
