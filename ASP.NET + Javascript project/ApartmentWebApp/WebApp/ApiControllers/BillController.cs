#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Public.DTO.v1.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BillController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly BillMapper _billMapper = new BillMapper();

        public BillController(IAppBLL bll)
        {
            _bll = bll;
        }
        
        // GET: api/Bill
        /// <summary>
        /// Get all bills from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of bills</returns>
        [ProducesResponseType(typeof( App.Public.DTO.v1.Bill ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Bill>>> GetBills()
        {
            var req = await _bll.Bill.GetAllAsync();
            
            var result = req.Select(association => _billMapper.Map(association));
            
            return Ok(result);
        }

        // GET: api/Bill/5
        /// <summary>
        /// Get a specific bill from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply bill's ID</param>
        /// <returns>A bill</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Bill ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Bill>> GetBill(Guid id)
        {
            var bill = await _bll.Bill.FirstOrDefaultAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            return _billMapper.Map(bill);
        }

        // PUT: api/Bill/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific bill from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="bill">Supply a valid bill</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutBill(Guid id, App.Public.DTO.v1.Bill bill)
        {
            if (id != bill.Id)
            {
                return BadRequest();
            }
            
            var billFromDb = await _bll.Bill.FirstOrDefaultAsync(bill.Id);
            
            if (billFromDb == null)
            {
                return NotFound();
            }
            
            billFromDb.Number = bill.Number;
            billFromDb.Date = bill.Date;
            billFromDb.DeadLine = bill.DeadLine;
            billFromDb.ApartmentId = bill.ApartmentId;
            billFromDb.OwnerId = bill.OwnerId;
            billFromDb.PreviousBillId = bill.PreviousBillId;

            _bll.Bill.Update(billFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BillExists(id))
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

        // POST: api/Bill
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new bill in the backend if the bearer token is valid
        /// </summary>
        /// <param name="bill">Supply a valid bill</param>
        /// <returns>A bill with a newly created ID</returns>

        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Bill ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Bill>> PostBill(App.Public.DTO.v1.Bill bill)
        {
            var bllBill = _billMapper.Map(bill);
            _bll.Bill.Add(bllBill);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBill", new { id = bill.Id }, bllBill);
        }

        // DELETE: api/Bill/5
        /// <summary>
        /// Delete a bill from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid bill ID</param>
        /// <returns>Nothing</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(Guid id)
        {
            var bill = await _bll.Bill.FirstOrDefaultAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            _bll.Bill.Remove(bill);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> BillExists(Guid id)
        {
            return await _bll.Bill.ExistsAsync(id);
        }
    }
}
