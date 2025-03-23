using System.Collections;
using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;
using AppUser = App.Domain.Identity.AppUser;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExtraFeesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.ExtraFee, App.BLL.DTO.ExtraFee> _mapper;

        public ExtraFeesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.ExtraFee, App.BLL.DTO.ExtraFee>(autoMapper);
        }
        /// <summary>
        /// Returns all extra fees.
        /// </summary>
        /// <returns>List of extra fees</returns>
        // GET: api/ExtraFees
        [HttpGet]
        [ProducesResponseType<IEnumerable>((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.ExtraFee>>((int) HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.ExtraFee>>> GetExtraFees()
        {
            var res = (await _bll.ExtraFees.GetAllSortedAsync());
    
            return Ok(res);
        }
        
        
        /// <summary>
        /// Returns the extra fee with the given id.
        /// </summary>
        /// <param name="id">given ID</param>
        /// <returns>ExtraFee with given ID</returns>
        // GET: api/ExtraFees/5
        [HttpGet("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.ExtraFee>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.ExtraFee>> GetExtraFee(Guid id)
        {
            var res = await _bll.ExtraFees.FirstOrDefaultAsyncCustom(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map(res));
        }
        
        /// <summary>
        /// Updates the extra fee with the given id.
        /// </summary>
        /// <param name="id">Given ID</param>
        /// <param name="input">Given object</param>
        /// <returns>No content if updated</returns>
        // PUT: api/ExtraFees/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.ExtraFee>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutExtraFee(Guid id, App.DTO.v1_0.ExtraFee input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            try
            {
                var res = _mapper.Map(input);
                var updatedInput = await _bll.ExtraFees.UpdateAsync(res);
                if (updatedInput == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExtraFeeExistsAsync(id))
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
        /// Creates a new extra fee.
        /// </summary>
        /// <param name="input">Takes in ExtraFee object</param>
        /// <returns>New extra fee</returns>
        // POST: api/ExtraFees
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.ExtraFee>>((int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.ExtraFee>> PostExtraFee(App.DTO.v1_0.ExtraFee input)
        {
            var res = _mapper.Map(input);
            var createdInput = _bll.ExtraFees.Add(res);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetExtraFee", new { id = createdInput.Id }, _mapper.Map(createdInput));
        }
        /// <summary>
        /// Deletes the extra fee with the given id.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>No content</returns>
        // DELETE: api/ExtraFees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.ExtraFee>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.ExtraFee>> DeleteExtraFee(Guid id)
        {
            var res = await _bll.ExtraFees.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
    
            await _bll.ExtraFees.RemoveAsync(res);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Returns true if the extra fee with the given id exists.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>True if ID exists</returns>
        private async Task<bool> ExtraFeeExistsAsync(Guid id)
        {
            return await _bll.ExtraFees.ExistsAsync(id);
        }
    }
}
