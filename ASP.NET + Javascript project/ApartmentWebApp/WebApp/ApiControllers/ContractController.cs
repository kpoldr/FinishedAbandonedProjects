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
    public class ContractController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly ContractMapper _contractMapper = new ContractMapper();

        public ContractController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/Contract
        /// <summary>
        /// Get all contracts from the backend if the bearer token is valid
        /// </summary>
        /// <returns>List of contracts</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Contract ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Contract>>> GetContracts()
        {
            var req = await _bll.Contract.GetAllAsync();
            
            var result = req.Select(contract => _contractMapper.Map(contract));
            
            return Ok(result);
        }

        // GET: api/Contract/5
        /// <summary>
        /// Get a specific contract from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply contract ID</param>
        /// <returns>A contract</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Contract ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<App.Public.DTO.v1.Contract>> GetContract(Guid id)
        {
            var contract = await _bll.Contract.FirstOrDefaultAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            return _contractMapper.Map(contract);
        }

        // PUT: api/Contract/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update a specific contract from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply an id for the entity to update</param>
        /// <param name="contract">Supply a valid contract</param>
        /// <returns>Nothing</returns>
        [Consumes("application/json")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(Guid id, App.Public.DTO.v1.Contract contract)
        {
            if (id != contract.Id)
            {
                return BadRequest();
            }
            
            var contractFromDb = await _bll.Contract.FirstOrDefaultAsync(contract.Id);
            
            if (contractFromDb == null)
            {
                return NotFound();
            }
            
            contractFromDb.Name.SetTranslation(contract.Name);
            contractFromDb.Description.SetTranslation(contract.Description);
            
            contractFromDb.OwnerId = contract.OwnerId;
            contractFromDb.AssociationId = contract.AssociationId;
            
            _bll.Contract.Update(contractFromDb);

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ContractExists(id))
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

        // POST: api/Contract
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new contract in the backend if the bearer token is valid
        /// </summary>
        /// <param name="contract">Supply a valid contract</param>
        /// <returns>A contract with a newly created ID</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof( App.Public.DTO.v1.Contract ), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<App.Public.DTO.v1.Contract>> PostContract(App.Public.DTO.v1.Contract contract)
        {
            var bllContract = _contractMapper.Map(contract);
            _bll.Contract.Add(bllContract);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetContract", new { id = contract.Id }, contract);
        }

        // DELETE: api/Contract/5
        /// <summary>
        /// Delete a contract from the backend if the bearer token is valid
        /// </summary>
        /// <param name="id">Supply a valid contract ID</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(Guid id)
        {
            var contract = await _bll.Contract.FirstOrDefaultAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            _bll.Contract.Remove(contract);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ContractExists(Guid id)
        {
            return await _bll.Contract.ExistsAsync(id);
        }
    }
}
