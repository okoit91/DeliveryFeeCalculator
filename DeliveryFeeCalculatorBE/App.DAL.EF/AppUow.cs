using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using App.Domain;
using App.Domain.Identity;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUow : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    public AppUow(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }
    
    private ICityRepository? _cityRepository;
    private IExtraFeeRepository? _extraFeeRepository;
    private IStandardFeeRepository? _standardFeeRepository;
    private IVehicleTypeRepository? _vehicleTypeRepository;
    private IWeatherRepository? _weatherRepository;
    private IStationRepository? _stationRepository;
    private IAppUserRepository? _appUserRepository;
    private IAppRoleRepository? _appRoleRepository;
    
    public ICityRepository CityRepository => _cityRepository ?? new CityRepository(UowDbContext, _mapper);
    public IExtraFeeRepository ExtraFeeRepository => _extraFeeRepository ?? new ExtraFeeRepository(UowDbContext, _mapper);
    public IStandardFeeRepository StandardFeeRepository => _standardFeeRepository ?? new StandardFeeRepository(UowDbContext, _mapper);
    public IVehicleTypeRepository VehicleTypeRepository => _vehicleTypeRepository ?? new VehicleTypeRepository(UowDbContext, _mapper);
    public IWeatherRepository WeatherRepository => _weatherRepository ?? new WeatherRepository(UowDbContext, _mapper);
    public IStationRepository StationRepository => _stationRepository ?? new StationRepository(UowDbContext, _mapper);
    public IAppUserRepository AppUserRepository => _appUserRepository ?? new AppUserRepository(UowDbContext, _mapper);
    public IAppRoleRepository AppRoleRepository => _appRoleRepository ?? new AppRoleRepository(UowDbContext, _mapper);
    
}

