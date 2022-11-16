#nullable disable
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AssociationMapper = App.Public.DTO.v1.Mappers.AssociationMapper;

namespace WebApp.ApiControllers
{   
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssociationController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly AssociationMapper _associationMapper = new AssociationMapper();
        
        public AssociationController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Association
        /// <summary>
        /// Get all associations from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of associations</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Association ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Association>>> GetAssociations()
        {
            
            var req = await _bll.Association.GetAllAsync();
            
            var result = req.Select(association => _associationMapper.Map(association));
            
            return Ok(result);
            
        }
        
        
        // GET: api/Association/5
        /// <summary>
        /// Get a specific association from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply association's ID</param>
        /// <returns>An association</returns>
        [ProducesResponseType(typeof( App.Public.DTO.v1.Association ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Association>> GetAssociation(Guid id)
        {
            var association = await _bll.Association.FirstOrDefaultAsync(id);

            
            if (association == null)
            {
                return NotFound();
            }

            return _associationMapper.Map(association);
        }

        // PUT: api/Association/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific associations from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="association">Supply a valid association</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutAssociation(Guid id, App.Public.DTO.v1.Association association)
        {
            if (id != association.Id)
            {
                return BadRequest();
            } 
            
            
            var associationFromDb = await _bll.Association.FirstOrDefaultAsync(association.Id);
            
            if (associationFromDb == null)
            {
                return NotFound();
            }
            
            associationFromDb.Name.SetTranslation(association.Name);
            associationFromDb.Description.SetTranslation(association.Description);
            associationFromDb.BankName.SetTranslation(association.BankName);
            
            associationFromDb.Email = association.Email;
            associationFromDb.Phone = association.Phone;
            associationFromDb.BankNumber = (association.BankNumber);
            associationFromDb.AppUserId = association.AppUserId;

            _bll.Association.Update(associationFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AssociationExists(id))
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

        // POST: api/Association
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new association in the backend if the bearer token is valid
        /// </summary>
        /// <param name="association">Supply a valid association</param>
        /// <returns>An association with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Association ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Association>> PostAssociation(App.Public.DTO.v1.Association association)
        {
            var bllAssociation = _associationMapper.Map(association);
            _bll.Association.Add(bllAssociation);
            await _bll.SaveChangesAsync();
            return CreatedAtAction("GetAssociation", new {id = association.Id}, bllAssociation);
        }

        // DELETE: api/Association/5
        /// <summary>
        /// Delete an association from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid association ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssociation(Guid id)
        {
            var association = await _bll.Association.FirstOrDefaultAsync(id);
            if (association == null)
            {
                return NotFound();
            }
                
            _bll.Association.Remove(association);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AssociationExists(Guid id)
        {
            return await _bll.Association.ExistsAsync(id);
        }
    }
}