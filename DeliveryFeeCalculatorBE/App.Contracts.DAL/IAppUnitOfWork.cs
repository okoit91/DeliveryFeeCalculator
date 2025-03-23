using App.Contracts.DAL.Repositories;
using App.Domain;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    ICityRepository CityRepository { get; }
    IExtraFeeRepository ExtraFeeRepository { get; }
    IStandardFeeRepository StandardFeeRepository { get; }
    IVehicleTypeRepository VehicleTypeRepository { get; }
    IWeatherRepository WeatherRepository { get; }
    IStationRepository StationRepository { get; }
    IAppUserRepository AppUserRepository { get; }
    IAppRoleRepository AppRoleRepository { get;  }
}