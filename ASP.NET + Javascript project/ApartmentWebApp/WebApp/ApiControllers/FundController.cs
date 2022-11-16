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
    public class FundController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly FundMapper _fundMapper = new FundMapper();
        
        public FundController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Fund
        /// <summary>
        /// Get all funds from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of funds</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Fund ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Fund>>> GetFunds()
        {
            var req = await _bll.Fund.GetAllAsync();
            
            var result = req.Select(fund => _fundMapper.Map(fund));
            
            return Ok(result);
        }

        // GET: api/Fund/5
        /// <summary>
        /// Get a specific fund from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply fund ID</param>
        /// <returns>A Fund</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Fund ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Fund>> GetFund(Guid id)
        {
            var fund = await _bll.Fund.FirstOrDefaultAsync(id);

            if (fund == null)
            {
                return NotFound();
            }

            return _fundMapper.Map(fund);
        }

        // PUT: api/Fund/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific fund from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="fund">Supply a valid fund</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFund(Guid id, App.Public.DTO.v1.Fund fund)
        {
            if (id != fund.Id)
            {
                return BadRequest();
            }
            
            var fundFromDb = await _bll.Fund.FirstOrDefaultAsync(fund.Id);
            
            if (fundFromDb == null)
            {
                return NotFound();
            }
            
            fundFromDb.Name.SetTranslation(fund.Name);

            fundFromDb.Value = fund.Value;
            fundFromDb.AssociationId = fund.AssociationId;
            
            _bll.Fund.Update(fundFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await FundExists(id))
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

        // POST: api/Fund
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new fund in the backend if the bearer token is valid
        /// </summary>
        /// <param name="fund">Supply a valid fund</param>
        /// <returns>A fund with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Fund ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Fund>> PostFund(App.Public.DTO.v1.Fund fund)
        {
            var bllFund = _fundMapper.Map(fund);
            
            var association = await _bll.Association.FirstOrDefaultAsync(fund.AssociationId);
            if (association != null)
            {
                association.Funds!.Add(bllFund);
                _bll.Association.Update(association);
                await _bll.SaveChangesAsync();    
            }

            
            _bll.Fund.Add(bllFund);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetFund", new { id = fund.Id }, fund);
        }

        // DELETE: api/Fund/5
        /// <summary>
        /// Delete a fund from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid fund ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFund(Guid id)
        {
            var fund = await _bll.Fund.FirstOrDefaultAsync(id);
            if (fund == null)
            {
                return NotFound();
            }

            _bll.Fund.Remove(fund);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> FundExists(Guid id)
        {
            return await _bll.Apartment.ExistsAsync(id);
        }
    }
}
