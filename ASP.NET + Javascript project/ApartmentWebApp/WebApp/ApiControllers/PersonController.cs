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
    public class PersonController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PersonMapper _personMapper = new PersonMapper();
        
        public PersonController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Person
        /// <summary>
        /// Get all persons from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of persons</returns>

        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Person ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Person>>> GetPersons()
        {
            var req = await _bll.Person.GetAllAsync();
            
            var result = req.Select(person => _personMapper.Map(person));
            
            return Ok(result);
        }

        // GET: api/Person/5
        /// <summary>
        /// Get a specific person from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply person ID</param>
        /// <returns>A person</returns>

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Person ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<App.Public.DTO.v1.Person>> GetPerson(Guid id)
        {
            var person = await _bll.Person.FirstOrDefaultAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return _personMapper.Map(person);
        }

        // PUT: api/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        /// <summary>
        /// Update a specific person from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="person">Supply a valid person</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(Guid id, App.Public.DTO.v1.Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            
            var personFromDb = await _bll.Person.FirstOrDefaultAsync(person.Id);
            
            if (personFromDb == null)
            {
                return NotFound();
            }
            
            personFromDb.Name.SetTranslation(person.Name);
            personFromDb.BoardMember = person.BoardMember;
            personFromDb.ApartmentId = person.ApartmentId;
            
            _bll.Person.Update(personFromDb);
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PersonExists(id))
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

        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new person in the backend if the bearer token is valid
        /// </summary>
        /// <param name="person">Supply a valid person</param>
        /// <returns>An person with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Person ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Person>> PostPerson(App.Public.DTO.v1.Person person)
        {
            var bllPerson = _personMapper.Map(person);
            _bll.Person.Add(bllPerson);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/Person/5
        /// <summary>
        /// Delete a person from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid person ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _bll.Person.FirstOrDefaultAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _bll.Person.Remove(person);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> PersonExists(Guid id)
        {
            return await _bll.Person.ExistsAsync(id);
        }
    }
}
