using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IAppUserRepository : IEntityRepository<DALDTO.AppUser>, IAppUserRepositoryCustom<DALDTO.AppUser>
{
    
}

public interface IAppUserRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
}