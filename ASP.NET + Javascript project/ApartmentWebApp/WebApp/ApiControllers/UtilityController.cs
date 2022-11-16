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
    public class UtilityController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UtilityMapper _utilityMapper = new UtilityMapper();

        public UtilityController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Utility
        /// <summary>
        /// Get all utilities from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of utilities</returns>


        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Utility ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Utility>>> GetUtilities()
        {
            var req = await _bll.Utility.GetAllAsync();
            
            var result = req.Select(utility => _utilityMapper.Map(utility));
            
            return Ok(result);
        }

        // GET: api/Utility/5
        /// <summary>
        /// Get a specific utility from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply utility ID</param>
        /// <returns>An utility</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Utility ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Utility>> GetUtility(Guid id)
        {
            var utility = await _bll.Utility.FirstOrDefaultAsync(id);

            if (utility == null)
            {
                return NotFound();
            }

            return _utilityMapper.Map(utility);
        }

        // PUT: api/Utility/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific utility from the backend if the  bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="utility">Supply a valid utility</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtility(Guid id, App.Public.DTO.v1.Utility utility)
        {
            if (id != utility.Id)
            {
                return BadRequest();
            }
            
            var utilityFromDb = await _bll.Utility.FirstOrDefaultAsync(utility.Id);
            
            if (utilityFromDb == null)
            {
                return NotFound();
            }
            
            utilityFromDb.Name.SetTranslation(utility.Name);
            utilityFromDb.ApartmentId = utility.ApartmentId;
            utilityFromDb.BuildingId = utility.BuildingId;
            
            _bll.Utility.Update(utilityFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UtilityExists(id))
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

        // POST: api/Utility
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new utility in the backend if the bearer token is valid
        /// </summary>
        /// <param name="utility">Supply a valid utility</param>
        /// <returns>An utility with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Utility ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Utility>> PostUtility(App.Public.DTO.v1.Utility utility)
        {
            var bllUtility = _utilityMapper.Map(utility);
            _bll.Utility.Add(bllUtility);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetUtility", new { id = utility.Id }, utility);
        }

        // DELETE: api/Utility/5
        /// <summary>
        /// Delete an utility from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid utility ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtility(Guid id)
        {
            var utility = await _bll.Utility.FirstOrDefaultAsync(id);
            if (utility == null)
            {
                return NotFound();
            }

            _bll.Utility.Remove(utility);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> UtilityExists(Guid id)
        {
            return await _bll.Utility.ExistsAsync(id);
        }
    }
}
