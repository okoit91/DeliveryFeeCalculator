using System.Net;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.DTO.v1_0;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeliveryFeesController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly IDeliveryFeeService _feeService;

    public DeliveryFeesController(IAppBLL bll, IDeliveryFeeService feeService)
    {
        _bll = bll;
        _feeService = feeService;
    }
    /// <summary>
    /// Takes in the selected city as a parameter.
    /// Takes in the selected vehicle type as a parameter.
    /// Checks fees for the selected vehicle type in selected city. Applies weather fees if necessary.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Calculated fee for a specific city and vehicle type, based on current weather conditions</returns>
    
    
    [HttpPost("calculate")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType<IEnumerable<App.DTO.v1_0.DeliveryFee>>((int)HttpStatusCode.Unauthorized)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CalculateDeliveryFee([FromBody] DeliveryFee request)
    {
        try
        {
            var baseFee = await _bll.StandardFees.GetByCityAndVehicleAsync(request.CityId, request.VehicleTypeId);
            if (baseFee == null)
            {
                return BadRequest("No base fee found for the given city and vehicle type.");
            }

            var (extraFee, error) = await _feeService.CalculateExtraFee(request.CityId, request.VehicleTypeId);
            if (!string.IsNullOrEmpty(error))
            {
                return Ok(new { error });
            }

            var totalFee = baseFee.FeeAmount + extraFee;

            return Ok(new
            {
                BaseFee = baseFee.FeeAmount,
                ExtraFee = extraFee,
                TotalFee = totalFee
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error calculating delivery fee: {ex.Message}");
        }
    }
}