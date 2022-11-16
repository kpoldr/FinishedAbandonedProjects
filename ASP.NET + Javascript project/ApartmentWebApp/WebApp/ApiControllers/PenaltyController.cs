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
    public class PenaltyController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PenaltyMapper _penaltyMapper = new PenaltyMapper();

        public PenaltyController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Penalty
        /// <summary>
        /// Get all penalties from the backend if bearer token is valid
        /// </summary>
        /// <returns>List of penalties</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Penalty ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Penalty>>> GetPenalties()
        {
            var req = await _bll.Penalty.GetAllAsync();
            
            var result = req.Select(penalty => _penaltyMapper.Map(penalty));
            
            return Ok(result);
        }

        // GET: api/Penalty/5
        /// <summary>
        /// Get a specific penalty from the backend if bearer token is valid
        /// </summary>
        /// <param name="id">Supply penalty ID</param>
        /// <returns>A penalty</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Penalty ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Penalty>> GetPenalty(Guid id)
        {
            var penalty = await _bll.Penalty.FirstOrDefaultAsync(id);


            if (penalty == null)
            {
                return NotFound();
            }

            return _penaltyMapper.Map(penalty);
        }

        // PUT: api/Penalty/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific penalty from the backend if bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="penalty">Supply a valid penalty</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPenalty(Guid id, App.Public.DTO.v1.Penalty penalty)
        {
            if (id != penalty.Id)
            {
                return BadRequest();
            }
            
            var penaltyFromDb = await _bll.Penalty.FirstOrDefaultAsync(penalty.Id);
            
            if (penaltyFromDb == null)
            {
                return NotFound();
            }
            
            penaltyFromDb.PenaltyName.SetTranslation(penalty.PenaltyName);
            penaltyFromDb.Value = penalty.Value;
            penaltyFromDb.Multiplier = penalty.Multiplier;
            penaltyFromDb.BillId = penalty.BillId;
            penaltyFromDb.OwnerId = penalty.OwnerId;
            
            _bll.Penalty.Update(penaltyFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PenaltyExists(id))
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

        // POST: api/Penalty
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new penalty in the backend if bearer token is valid
        /// </summary>
        /// <param name="penalty">Supply a valid penalty</param>
        /// <returns>A penalty with a newly created ID</returns>

        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Penalty ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Penalty>> PostPenalty(App.Public.DTO.v1.Penalty penalty)
        {
            var bllPenalty = _penaltyMapper.Map(penalty);
            _bll.Penalty.Add(bllPenalty);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPenalty", new { id = penalty.Id }, penalty);
        }

        // DELETE: api/Penalty/5
        /// <summary>
        /// Delete a penalty from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid penalty ID</param>
        /// <returns>Nothing</returns>
        
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePenalty(Guid id)
        {
            var penalty = await _bll.Penalty.FirstOrDefaultAsync(id);
            if (penalty == null)
            {
                return NotFound();
            }

            _bll.Penalty.Remove(penalty);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> PenaltyExists(Guid id)
        {
            return await _bll.Penalty.ExistsAsync(id);
        }
    }
}
