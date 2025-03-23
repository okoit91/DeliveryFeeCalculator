using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IStandardFeeService : 
    IEntityRepository<App.BLL.DTO.StandardFee>, IStandardFeeRepositoryCustom<App.BLL.DTO.StandardFee>
{
    Task<StandardFee?> GetByCityAndVehicleAsync(Guid cityId, Guid vehicleTypeId);
    
}