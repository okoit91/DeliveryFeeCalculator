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
    public class CitiesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.City, App.BLL.DTO.City> _mapper;

        public CitiesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.City, App.BLL.DTO.City>(autoMapper);
        }
        /// <summary>
        /// Returns all cities.
        /// </summary>
        /// <returns>List of cities</returns>
        // GET: api/Cities
        [HttpGet]
        [ProducesResponseType<IEnumerable>((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.City>>((int) HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.City>>> GetCities()
        {
            var cities = (await _bll.Cities.GetAllSortedWithoutUserAsync()).Select(c => _mapper.Map(c)).ToList();

            if (!cities.Any())
            {
                return NotFound("No cities found.");
            }

            return Ok(cities);
        }
        /// <summary>
        /// Returns the city with the given id.
        /// </summary>
        /// <param name="id">given ID</param>
        /// <returns>City with given ID</returns>
        // GET: api/Cities/5
        [HttpGet("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.City>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.City>> GetCity(Guid id)
        {
            var res = await _bll.Cities.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map(res));
        }
        /// <summary>
        /// Updates the city with the given id.
        /// </summary>
        /// <param name="id">Given ID</param>
        /// <param name="input">Given object</param>
        /// <returns>No content if updated</returns>
        // PUT: api/Cities/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.City>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.City>> PutCity(Guid id, App.DTO.v1_0.City input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            try
            {
                var res = _mapper.Map(input);
                var updatedInput = await _bll.Cities.UpdateAsync(res);
                if (updatedInput == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CityExistsAsync(id))
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
        /// Creates a new city.
        /// </summary>
        /// <param name="input">Takes in City object</param>
        /// <returns>New city</returns>
        // POST: api/Cities
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.City>>((int)HttpStatusCode.OK)]
        
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.City>> PostCity(App.DTO.v1_0.City input)
        {
            var res = _mapper.Map(input);
            var createdInput = _bll.Cities.Add(res);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = createdInput.Id }, _mapper.Map(createdInput));
        }
        /// <summary>
        /// Deletes the city with the given id.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>No content</returns>
        // DELETE: api/Cities/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.City>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.City>> DeleteCity(Guid id)
        {
            var res = await _bll.Cities.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
    
            await _bll.Cities.RemoveAsync(res);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Returns true if the city with the given id exists.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>True if ID exists</returns>
        private async Task<bool> CityExistsAsync(Guid id)
        {
            return await _bll.Cities.ExistsAsync(id);
        }
    }
}
