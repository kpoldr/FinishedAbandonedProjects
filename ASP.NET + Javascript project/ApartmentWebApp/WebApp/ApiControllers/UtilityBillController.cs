#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.BLL;
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
    public class UtilityBillController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UtilityBillMapper _utilityBillMapper = new UtilityBillMapper();

        public UtilityBillController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/UtilityBill
        /// <summary>
        /// Get all utility bills from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of utility bills</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.UtilityBill ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.UtilityBill>>> GetUtilityBills()
        {
            var req = await _bll.UtilityBill.GetAllAsync();
            
            var result = req.Select(utilityBill => _utilityBillMapper.Map(utilityBill));
            
            return Ok(result);
        }

        // GET: api/UtilityBill/5
        /// <summary>
        /// Get a specific utility bill from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an utility bill ID</param>
        /// <returns>An utility bill</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.UtilityBill ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.UtilityBill>> GetUtilityBill(Guid id)
        {
            var utilityBill = await _bll.UtilityBill.FirstOrDefaultAsync(id);

            if (utilityBill == null)
            {
                return NotFound();
            }

            return _utilityBillMapper.Map(utilityBill);
        }

        // PUT: api/UtilityBill/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific utility bills from the backend if the  bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="utilityBill">Supply a valid utility bill</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilityBill(Guid id, App.Public.DTO.v1.UtilityBill utilityBill)
        {
            if (id != utilityBill.Id)
            {
                return BadRequest();
            }
            
            var utilityBillFromDb = await _bll.UtilityBill.FirstOrDefaultAsync(utilityBill.Id);
            
            if (utilityBillFromDb == null)
            {
                return NotFound();
            }
            
            utilityBillFromDb.Quantity = utilityBill.Quantity;
            utilityBillFromDb.Price = utilityBill.Price;
            utilityBillFromDb.UtilityId = utilityBill.UtilityId;
            utilityBillFromDb.BillId = utilityBill.BillId;
            
            _bll.UtilityBill.Update(utilityBillFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UtilityBillExists(id))
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

        // POST: api/UtilityBill
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new utility bill in the backend if the bearer token is valid
        /// </summary>
        /// <param name="utilityBill">Supply a valid utility bill</param>
        /// <returns>An utility bill with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.UtilityBill ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.UtilityBill>> PostUtilityBill(App.Public.DTO.v1.UtilityBill utilityBill)
        {
            var bllUtilityBill = _utilityBillMapper.Map(utilityBill);
            _bll.UtilityBill.Add(bllUtilityBill);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetUtilityBill", new { id = utilityBill.Id }, utilityBill);
        }

        // DELETE: api/UtilityBill/5
        /// <summary>
        /// Delete an utility bill from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid utility bill ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilityBill(Guid id)
        {
            var utilityBill = await _bll.UtilityBill.FirstOrDefaultAsync(id);
            if (utilityBill == null)
            {
                return NotFound();
            }

            _bll.UtilityBill.Remove(utilityBill);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> UtilityBillExists(Guid id)
        {
            return await _bll.UtilityBill.ExistsAsync(id);
        }
    }
}
