using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    
    ICityService Cities { get; }
    IExtraFeeService ExtraFees { get; }
    
    IStandardFeeService StandardFees { get; }
    
    IVehicleTypeService VehicleTypes { get; }
    IWeatherService Weathers { get; }
    
    IStationService Stations { get; }
    
    IAppUserService AppUsers { get; }
    
    IAppRoleService AppRoles { get; }
}