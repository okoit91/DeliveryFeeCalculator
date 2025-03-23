using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IWeatherService :
    IEntityRepository<App.BLL.DTO.Weather>, IWeatherRepositoryCustom<App.BLL.DTO.Weather>
{
    Task<Weather?> GetLatestWeatherByCityAsync(Guid cityId);
}