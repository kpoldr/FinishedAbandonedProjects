#nullable disable

using App.Contracts.BLL;
using App.Public.DTO.v1.Mappers;
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
    public class ApartmentController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly ApartmentMapper _apartmentMapper = new ApartmentMapper();

        public ApartmentController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Apartment
        /// <summary>
        /// Get all apartments from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of apartments</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof( IEnumerable<App.Public.DTO.v1.Apartment> ), 200)]
        [ProducesResponseType(typeof( IEnumerable<App.Public.DTO.v1.Apartment> ), 401)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Apartment>>> GetApartments()
        {
            var req = await _bll.Apartment.GetAllAsync();
            
            var result = req.Select(association => _apartmentMapper.Map(association));
            
            return Ok(result);
        }

        // GET: api/Apartment/5
        /// <summary>
        /// Get a specific apartment from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply apartment ID</param>
        /// <returns>An apartment</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Apartment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        public async Task<ActionResult<App.Public.DTO.v1.Apartment>> GetApartment(Guid id)
        {
            var apartment = await _bll.Apartment.FirstOrDefaultAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            return _apartmentMapper.Map(apartment);
        }

        // PUT: api/Apartment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific apartment from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="apartment">Supply a valid apartment</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Apartment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutApartment(Guid id, App.Public.DTO.v1.Apartment apartment)
        {
            if (id != apartment.Id)
            {
                return BadRequest();
            }
            
            var apartmentFromDb = await _bll.Apartment.FirstOrDefaultAsync(apartment.Id);
            
            if (apartmentFromDb == null)
            {
                return NotFound();
            }
            
            apartmentFromDb.Number = apartment.Number;
            apartmentFromDb.Floor = apartment.Floor;
            apartmentFromDb.Size = apartment.Size;
            apartmentFromDb.OwnerId = apartment.OwnerId;
            apartmentFromDb.BuildingId = apartment.BuildingId;

            _bll.Apartment.Update(apartmentFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ApartmentExists(id))
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

        // POST: api/Apartment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new apartment in the backend if the bearer token is valid
        /// </summary>
        /// <param name="apartment">Supply a valid apartment</param>
        /// <returns>An apartment with a newly created ID</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Apartment ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<App.Public.DTO.v1.Apartment>> PostApartment(App.Public.DTO.v1.Apartment apartment)
        {
            var bllApartment = _apartmentMapper.Map(apartment);
            _bll.Apartment.Add(bllApartment);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetApartment", new { id = apartment.Id }, bllApartment);
        }

        // DELETE: api/Apartment/5
        
        /// <summary>
        /// Delete an apartment from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid apartment ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(Guid id)
        {
            var apartment = await _bll.Apartment.FirstOrDefaultAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            _bll.Apartment.Remove(apartment);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        
        private async Task<bool> ApartmentExists(Guid id)
        {
            return await _bll.Apartment.ExistsAsync(id);
        }
    }
}
