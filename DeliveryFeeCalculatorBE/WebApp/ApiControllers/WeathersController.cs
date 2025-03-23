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
    public class WeathersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Weather, App.BLL.DTO.Weather> _mapper;

        public WeathersController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Weather, App.BLL.DTO.Weather>(autoMapper);
        }
        /// <summary>
        /// Returns all weathers visible to current user.
        /// </summary>
        /// <returns>List of weathers</returns>
        // GET: api/Weathers
        [HttpGet]
        [ProducesResponseType<IEnumerable>((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Weather>>((int) HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Weather>>> GetWeathers()
        {
            var res = (await _bll.Weathers.GetAllSortedAsync(
                    Guid.Parse(_userManager.GetUserId(User))
                ))
                .Select(e => _mapper.Map(e))
                .ToList();
            
            return Ok(res);
        }
        /// <summary>
        /// Returns the weather with the given id.
        /// </summary>
        /// <param name="id">given ID</param>
        /// <returns>Weather with given ID</returns>
        // GET: api/Weathers/5
        [HttpGet("{id}")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Weather>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Weather>> GetWeather(Guid id)
        {
            var res = await _bll.Weathers.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map(res));
        }
        /// <summary>
        /// Updates the weather with the given id.
        /// </summary>
        /// <param name="id">Given ID</param>
        /// <param name="input">Given object</param>
        /// <returns>No content if updated</returns>
        // PUT: api/Weathers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Weather>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Weather>> PutWeather(Guid id, App.DTO.v1_0.Weather input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            try
            {
                var res = _mapper.Map(input);
                var updatedInput = await _bll.Weathers.UpdateAsync(res);
                if (updatedInput == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WeatherExistsAsync(id))
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
        /// Creates a new weather.
        /// </summary>
        /// <param name="input">Takes in Weather object</param>
        /// <returns>New weather</returns>
        // POST: api/Weathers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Weather>>((int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Weather>> PostWeather(App.DTO.v1_0.Weather input)
        {
            var res = _mapper.Map(input);
            var createdInput = _bll.Weathers.Add(res);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetWeather", new { id = createdInput.Id }, _mapper.Map(createdInput));
        }
        /// <summary>
        /// Deletes the weather with the given id.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>No content</returns>
        // DELETE: api/Weathers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<IEnumerable>((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Weather>>((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Weather>> DeleteWeather(Guid id)
        {
            var res = await _bll.Weathers.FirstOrDefaultAsync(id);
            if (res == null)
            {
                return NotFound();
            }
    
            await _bll.Weathers.RemoveAsync(res);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Returns true if the weather with the given id exists.
        /// </summary>
        /// <param name="id">Takes in ID</param>
        /// <returns>True if ID exists</returns>
        private async Task<bool> WeatherExistsAsync(Guid id)
        {
            return await _bll.Weathers.ExistsAsync(id);
        }
    }
}
