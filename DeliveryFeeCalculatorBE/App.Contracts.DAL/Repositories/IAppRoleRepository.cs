using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IAppRoleRepository : IEntityRepository<DALDTO.AppRole>, IAppRoleRepositoryCustom<DALDTO.AppRole>
{
    
}


public interface IAppRoleRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
}