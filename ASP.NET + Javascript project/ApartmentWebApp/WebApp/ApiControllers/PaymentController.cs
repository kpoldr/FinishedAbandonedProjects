#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.BLL;
using App.Public.DTO.v1.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PaymentMapper _paymentMapper = new PaymentMapper();
        
        public PaymentController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Payment
        /// <summary>
        /// Get all payments from the backend if bearer token is valid
        /// </summary>
        /// <returns>List of payments</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Payment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Payment>>> GetPayments()
        {
            var req = await _bll.Payment.GetAllAsync();
            
            var result = req.Select(payment => _paymentMapper.Map(payment));
            
            return Ok(result);
        }

        // GET: api/Payment/5
        /// <summary>
        /// Get a specific payment from the backend if bearer token is valid
        /// </summary>
        /// <param name="id">Supply payment ID</param>
        /// <returns>A payment</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Payment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Payment>> GetPayment(Guid id)
        {
            var payment = await _bll.Payment.FirstOrDefaultAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return _paymentMapper.Map(payment);
        }

        // PUT: api/Payment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific payment from the backend if bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="payment">Supply a valid payment</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(Guid id, App.Public.DTO.v1.Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            var paymentFromDb = await _bll.Payment.FirstOrDefaultAsync(payment.Id);
            
            if (paymentFromDb == null)
            {
                return NotFound();
            }
            
            paymentFromDb.PaymentDate = payment.PaymentDate;
            paymentFromDb.PaymentValue = payment.PaymentValue;
            paymentFromDb.OwnerId = payment.OwnerId;
            paymentFromDb.FundId = payment.FundId;
            paymentFromDb.PersonId = payment.PersonId;

            _bll.Payment.Update(paymentFromDb);
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PaymentExists(id))
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

        // POST: api/Payment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        /// <summary>
        /// Create a new payment in the backend if the bearer token is valid
        /// </summary>
        /// <param name="payment">Supply a valid payment</param>
        /// <returns>A payment with a newly created ID</returns>

        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Payment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Payment>> PostPayment(App.Public.DTO.v1.Payment payment)
        {
            var bllPayment = _paymentMapper.Map(payment);
            _bll.Payment.Add(bllPayment);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        // DELETE: api/Payment/5
        /// <summary>
        /// Delete an payment from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid payment ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var payment = await _bll.Payment.FirstOrDefaultAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _bll.Payment.Remove(payment);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> PaymentExists(Guid id)
        {
            return await _bll.Payment.ExistsAsync(id);
        }
    }
}
