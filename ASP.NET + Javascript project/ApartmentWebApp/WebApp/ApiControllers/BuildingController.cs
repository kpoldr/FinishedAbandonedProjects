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
    public class BuildingController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly BuildingMapper _buildingMapper = new BuildingMapper();

        public BuildingController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Building
        /// <summary>
        /// Get all buildings from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of buildings</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Building ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Building>>> GetBuildings()
        {
            var req = await _bll.Building.GetAllAsync();
            
            var result = req.Select(building => _buildingMapper.Map(building));
            
            return Ok(result);
        }

        // GET: api/Building/5
        /// <summary>
        /// Get a specific building from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply building ID</param>
        /// <returns>A building</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Building ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Building>> GetBuilding(Guid id)
        {
            var building = await _bll.Building.FirstOrDefaultAsync(id);

            if (building == null)
            {
                return NotFound();
            }

            return _buildingMapper.Map(building);
        }

        // PUT: api/Building/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific building from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="building">Supply a valid building</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(Guid id,App.Public.DTO.v1.Building building)
        {
            if (id != building.Id)
            {
                return BadRequest();
            }
            
            var buildingFromDb = await _bll.Building.FirstOrDefaultAsync(building.Id);
            
            if (buildingFromDb == null)
            {
                return NotFound();
            }

            buildingFromDb.Address.SetTranslation(building.Address);
            buildingFromDb.AssociationId = building.AssociationId;
            
            _bll.Building.Update(buildingFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BuildingExists(id))
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

        // POST: api/Building
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new building in the backend if the bearer token is valid
        /// </summary>
        /// <param name="building">Supply a valid building</param>
        /// <returns>A building with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Building ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Building>> PostBuilding(App.Public.DTO.v1.Building building)
        {
            var bllBuilding = _buildingMapper.Map(building);
            _bll.Building.Add(bllBuilding);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBuilding", new { id = building.Id }, building);
        }

        // DELETE: api/Building/5
        /// <summary>
        /// Delete an building from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid building ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(Guid id)
        {
            var building = await _bll.Building.FirstOrDefaultAsync(id);
            
            if (building == null)
            {
                return NotFound();
            }
            
            
            
            _bll.Building.Remove(building);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> BuildingExists(Guid id)
        {
            return await _bll.Building.ExistsAsync(id);
        }
    }
}
