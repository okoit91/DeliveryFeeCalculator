using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IVehicleTypeService :
    IEntityRepository<App.BLL.DTO.VehicleType>, IVehicleTypeRepositoryCustom<App.BLL.DTO.VehicleType>
{
    Task<IEnumerable<App.BLL.DTO.VehicleType>> GetAllSortedWithoutUserAsync();
}