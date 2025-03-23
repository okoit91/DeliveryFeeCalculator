using System.Collections;
using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StandardFeesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.StandardFee, App.BLL.DTO.StandardFee> _mapper;

        public StandardFeesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.StandardFee, App.BLL.DTO.StandardFee>(autoMapper);
        }
        /// <summary>
        /// Returns all standard fees.
        /// </summary>
        /// <returns>List of standard fees</returns>
        // GET: api/StandardFees
        [HttpGet]
        [ProducesResponseType<IEnumerable>((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.StandardFee>>((int) HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.StandardFee>>> GetStandardFees()
        {
            var res = await _bll.StandardFees.GetAllSortedAsync();
            
            return Ok(res);
        }
        /// <summary>
        /// Returns the standard fee with the given id.
        /// </summary>
        /// <param name="id">given ID</param>
        /// <returns>StandardFee with given ID</returns>
        // GET: api/StandardFees/5
        [HttpGet("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.StandardFee>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.StandardFee>> GetStandardFee(Guid id)
        {
            var res = await _bll.StandardFees.FirstOrDefaultAsyncCustom(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map(res));
        }
        /// <summary>
        /// Updates the standard fee with the given id.
        /// </summary>
        /// <param name="id">Given ID</param>
        /// <param name="input">Given object</param>
        /// <returns>No content if updated</returns>
        // PUT: api/StandardFees/5
        [HttpPut("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.StandardFee>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.StandardFee>> PutStandardFee(Guid id, App.DTO.v1_0.StandardFee input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            try
            {
                var res = _mapper.Map(input);
                var updatedInput = await _bll.StandardFees.UpdateAsync(res);
                if (updatedInput == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StandardFeeExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Creates a new standard fee.
        /// </summary>
        /// <param name="input">Takes in StandardFee object</param>
        /// <returns>New standard fee</returns>
        // POST: api/StandardFees
        [HttpPost]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.StandardFee>>((int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.StandardFee>> PostStandardFee(App.DTO.v1_0.StandardFee input)
        {
            var res = _mapper.Map(input);
            var createdInput = _bll.StandardFees.Add(res);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetStandardFee", new { id = createdInput.Id }, _mapper.Map(createdInput));
        }
        /// <summary>
        /// Deletes the standard fee with the given id.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>No content</returns>
        // DELETE: api/StandardFees/5
        [HttpDelete("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.StandardFee>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.StandardFee>> DeleteStandardFee(Guid id)
        {
            var res = await _bll.StandardFees.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
    
            await _bll.StandardFees.RemoveAsync(res);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Returns true if the standard fee with the given id exists.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>True if ID exists</returns>
        private async Task<bool> StandardFeeExistsAsync(Guid id)
        {
            return await _bll.StandardFees.ExistsAsync(id);
        }
    }
}
