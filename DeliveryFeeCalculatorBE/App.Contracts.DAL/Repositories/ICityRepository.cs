using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ICityRepository : IEntityRepository<DALDTO.City>, ICityRepositoryCustom<DALDTO.City>
{ 
    Task<IEnumerable<App.DAL.DTO.City>> GetAllSortedWithoutUserAsync();
}


public interface ICityRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
    
    Task<TEntity?> FirstOrDefaultByNameAsync(string name);
}