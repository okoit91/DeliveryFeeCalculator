using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IStandardFeeRepository : IEntityRepository<DALDTO.StandardFee>,
    IStandardFeeRepositoryCustom<DALDTO.StandardFee>
{ 
    Task<App.DAL.DTO.StandardFee?> GetByCityAndVehicleAsync(Guid cityId, Guid vehicleTypeId);
}

public interface IStandardFeeRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
    Task<IEnumerable<TEntity>> GetAllSortedAsync();
    
    Task<TEntity?> FirstOrDefaultAsyncCustom(Guid id);
}