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
    public class VehicleTypesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.VehicleType, App.BLL.DTO.VehicleType> _mapper;

        public VehicleTypesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.VehicleType, App.BLL.DTO.VehicleType>(autoMapper);
        }
        /// <summary>
        /// Returns all vehicle types.
        /// </summary>
        /// <returns>List of vehicle types</returns>
        // GET: api/VehicleTypes
        [HttpGet]
        [ProducesResponseType<IEnumerable>((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.VehicleType>>((int) HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.VehicleType>>> GetVehicleTypes()
        {
            var vehicleTypes = (await _bll.VehicleTypes.GetAllSortedWithoutUserAsync()).Select(v => _mapper.Map(v)).ToList();

            if (!vehicleTypes.Any())
            {
                return NotFound("No vehicle types found.");
            }

            return Ok(vehicleTypes);
        }
        /// <summary>
        /// Returns the vehicle type with the given id.
        /// </summary>
        /// <param name="id">given ID</param>
        /// <returns>VehicleType with given ID</returns>
        // GET: api/VehicleTypes/5
        [HttpGet("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.VehicleType>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.VehicleType>> GetVehicleType(Guid id)
        {
            var res = await _bll.VehicleTypes.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map(res));
        }
        /// <summary>
        /// Updates the vehicle type with the given id.
        /// </summary>
        /// <param name="id">Given ID</param>
        /// <param name="input">Given object</param>
        /// <returns>No content if updated</returns>
        // PUT: api/VehicleTypes/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.VehicleType>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.VehicleType>> PutVehicleType(Guid id, App.DTO.v1_0.VehicleType input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            try
            {
                var res = _mapper.Map(input);
                var updatedInput = await _bll.VehicleTypes.UpdateAsync(res);
                if (updatedInput == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VehicleTypeExistsAsync(id))
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
        /// Creates a new vehicle type.
        /// </summary>
        /// <param name="input">Takes in VehicleType object</param>
        /// <returns>New vehicle type</returns>
        // POST: api/VehicleTypes
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.VehicleType>>((int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.VehicleType>> PostVehicleType(App.DTO.v1_0.VehicleType input)
        {
            var res = _mapper.Map(input);
            var createdInput = _bll.VehicleTypes.Add(res);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetVehicleType", new { id = createdInput.Id }, _mapper.Map(createdInput));
        }
        /// <summary>
        /// Deletes the vehicle type with the given id.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>No content</returns>
        // DELETE: api/VehicleTypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.VehicleType>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.VehicleType>> DeleteVehicleType(Guid id)
        {
            var res = await _bll.VehicleTypes.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
    
            await _bll.VehicleTypes.RemoveAsync(res);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Returns true if the vehicle type with the given id exists.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>True if ID exists</returns>
        private async Task<bool> VehicleTypeExistsAsync(Guid id)
        {
            return await _bll.VehicleTypes.ExistsAsync(id);
        }
    }
}
