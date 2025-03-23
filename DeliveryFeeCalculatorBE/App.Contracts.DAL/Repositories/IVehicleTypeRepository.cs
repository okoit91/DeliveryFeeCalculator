using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IVehicleTypeRepository : IEntityRepository<DALDTO.VehicleType>,
    IVehicleTypeRepositoryCustom<DALDTO.VehicleType>
{
    Task<IEnumerable<App.DAL.DTO.VehicleType>> GetAllSortedWithoutUserAsync();
}


public interface IVehicleTypeRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
}