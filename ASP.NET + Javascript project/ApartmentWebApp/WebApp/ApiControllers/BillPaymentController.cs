#nullable disable

using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Public.DTO.v1.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BillPaymentController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly BillPaymentMapper _billPaymentMapper = new BillPaymentMapper();
        
        public BillPaymentController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/BillPayment
        /// <summary>
        /// Get all bill payments from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of bill payments</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.BillPayment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.BillPayment>>> GetBillPayments()
        {
            var req = await _bll.BillPayment.GetAllAsync();
            
            var result = req.Select(billPayment => _billPaymentMapper.Map(billPayment));
            
            return Ok(result);
            
        }

        // GET: api/BillPayment/5
        /// <summary>
        /// Get a specific bill payment from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply bill payment ID</param>
        /// <returns>a bill payment</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.BillPayment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.BillPayment>> GetBillPayment(Guid id)
        {
            var billPayment = await _bll.BillPayment.FirstOrDefaultAsync(id);

            if (billPayment == null)
            {
                return NotFound();
            }

            return _billPaymentMapper.Map(billPayment);
        }

        // PUT: api/BillPayment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific bill payment from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="billPayment">Supply a valid bill payment</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillPayment(Guid id, App.Public.DTO.v1.BillPayment billPayment)
        {
            if (id != billPayment.Id)
            {
                return BadRequest();
            }
            
            var billPaymentFromDb = await _bll.BillPayment.FirstOrDefaultAsync(billPayment.Id);
            
            if (billPaymentFromDb == null)
            {
                return NotFound();
            }
            
            billPaymentFromDb.BillId = billPayment.BillId;
            billPaymentFromDb.PaymentId = billPayment.PaymentId;
            
            _bll.BillPayment.Update(billPaymentFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BillPaymentExists(id))
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

        // POST: api/BillPayment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new bill payment in the backend if the bearer token is valid
        /// </summary>
        /// <param name="billPayment">Supply a valid billPayment</param>
        /// <returns>A bill payment with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.BillPayment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.BillPayment>> PostBillPayment(App.Public.DTO.v1.BillPayment billPayment)
        {
            var bllBillPayment = _billPaymentMapper.Map(billPayment);
            _bll.BillPayment.Add(bllBillPayment);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBillPayment", new { id = billPayment.Id }, billPayment);
        }

        // DELETE: api/BillPayment/5
        /// <summary>
        /// Delete an bill payment from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid bill payment ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillPayment(Guid id)
        {
            var billPayment = await _bll.BillPayment.FirstOrDefaultAsync(id);
            if (billPayment == null)
            {
                return NotFound();
            }

            _bll.BillPayment.Remove(billPayment);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private Task<bool> BillPaymentExists(Guid id)
        {
            return _bll.BillPayment.ExistsAsync(id);
        }
    }
}
