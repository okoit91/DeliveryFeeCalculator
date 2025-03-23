using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IExtraFeeRepository : IEntityRepository<DALDTO.ExtraFee>, IExtraFeeRepositoryCustom<DALDTO.ExtraFee>
{
    Task<IEnumerable<App.DAL.DTO.ExtraFee>> GetAllForVehicleAsync(Guid vehicleTypeId);
}

public interface IExtraFeeRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
    Task<IEnumerable<TEntity>> GetAllSortedAsync();
    
    Task<TEntity?> FirstOrDefaultAsyncCustom(Guid id);
    
}