using App.BLL.services;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.DAL.EF;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL : BaseBLL<AppDbContext>, IAppBLL
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public AppBLL(IAppUnitOfWork uoW, IMapper mapper) : base(uoW)
    {
        _mapper = mapper;
        _uow = uoW;
    }
    
    private ICityService? _cities;
    private IExtraFeeService? _extraFees;
    private IStandardFeeService? _standardFees;
    private IVehicleTypeService? _vehicleTypes;
    private IWeatherService? _weathers;
    private IStationService? _stations;
    public IAppUserService? _appUsers;
    public IAppRoleService? _appRoles;
    

    public ICityService Cities =>
        _cities ?? new CityService(_uow, _uow.CityRepository, _mapper);
    
    public IExtraFeeService ExtraFees =>
        _extraFees ?? new ExtraFeeService(_uow, _uow.ExtraFeeRepository, _mapper);
    
    public IStandardFeeService StandardFees =>
        _standardFees ?? new StandardFeeService(_uow, _uow.StandardFeeRepository, _mapper);
    
    public IVehicleTypeService VehicleTypes =>
        _vehicleTypes ?? new VehicleTypeService(_uow, _uow.VehicleTypeRepository, _mapper);
    
    public IWeatherService Weathers =>
        _weathers ?? new WeatherService(_uow, _uow.WeatherRepository, _mapper);
    
    public IStationService Stations =>
        _stations ?? new StationService(_uow, _uow.StationRepository, _mapper);

    public IAppUserService AppUsers =>
        _appUsers ?? new AppUserService(_uow, _uow.AppUserRepository, _mapper);

    public IAppRoleService AppRoles =>
        _appRoles ?? new AppRoleService(_uow, _uow.AppRoleRepository, _mapper);
}

