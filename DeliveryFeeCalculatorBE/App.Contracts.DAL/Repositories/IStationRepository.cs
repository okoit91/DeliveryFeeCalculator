using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IStationRepository : IEntityRepository<DALDTO.Station>,
    IStationRepositoryCustom<DALDTO.Station>
{
}

public interface IStationRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync();
    
    Task<TEntity?> FirstOrDefaultByNameAsync(string name);
    
    Task<TEntity?> FirstOrDefaultAsyncCustom(Guid id);
}