using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IWeatherRepository : IEntityRepository<DALDTO.Weather>, IWeatherRepositoryCustom<DALDTO.Weather>
{
    Task<App.DAL.DTO.Weather?> GetLatestWeatherByCityAsync(Guid cityId);
}


public interface IWeatherRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
}