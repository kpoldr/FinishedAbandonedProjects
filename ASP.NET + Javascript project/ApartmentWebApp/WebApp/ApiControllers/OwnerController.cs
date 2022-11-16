#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Http;
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
    public class OwnerController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly OwnerMapper _ownerMapper = new OwnerMapper();
        public OwnerController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Owner
        /// <summary>
        /// Get all owners from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of owners</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Owner ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Owner>>> GetOwners()
        {
            var req = await _bll.Owner.GetAllAsync();
            
            var result = req.Select(owner => _ownerMapper.Map(owner));
            
            return Ok(result);
            
        }

        // GET: api/Owner/5
        /// <summary>
        /// Get a specific owner from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply owner ID</param>
        /// <returns>An owner</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Owner ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Owner>> GetOwner(Guid id)
        {
            var owner = await _bll.Owner.FirstOrDefaultAsync(id);

            if (owner == null)
            {
                return NotFound();
            }

            return _ownerMapper.Map(owner);
        }

        // PUT: api/Owner/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific owner from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="owner">Supply a valid owner</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOwner(Guid id, App.Public.DTO.v1.Owner owner)
        {
            if (id != owner.Id)
            {
                return BadRequest();
            }
            
            var ownerFromDb = await _bll.Owner.FirstOrDefaultAsync(owner.Id);
            
            if (ownerFromDb == null)
            {
                return NotFound();
            }
            
            ownerFromDb.Name.SetTranslation(owner.Name);

            ownerFromDb.Email = owner.Email;
            ownerFromDb.Phone = owner.Phone;
            ownerFromDb.AdvancedPay = owner.AdvancedPay;
            ownerFromDb.AppUserId = owner.AppUserId;
            
            _bll.Owner.Update(ownerFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OwnerExists(id))
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

        // POST: api/Owner
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new owner in the backend if the bearer token is valid
        /// </summary>
        /// <param name="owner">Supply a valid owner</param>
        /// <returns>A owner with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Owner ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Owner>> PostOwner(App.Public.DTO.v1.Owner owner)
        {
            var bllOwner = _ownerMapper.Map(owner);
            _bll.Owner.Add(bllOwner);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetOwner", new { id = owner.Id }, bllOwner);
        }

        // DELETE: api/Owner/5
        /// <summary>
        /// Delete an onwer from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid owner ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var owner = await _bll.Owner.FirstOrDefaultAsync(id);
            if (owner == null)
            {
                return NotFound();
            }

            _bll.Owner.Remove(owner);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> OwnerExists(Guid id)
        {
            return await _bll.Fund.ExistsAsync(id);
        }
    }
}
