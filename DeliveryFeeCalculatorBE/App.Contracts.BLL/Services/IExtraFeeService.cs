using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IExtraFeeService :
    IEntityRepository<App.BLL.DTO.ExtraFee>, IExtraFeeRepositoryCustom<App.BLL.DTO.ExtraFee>
{
    Task<IEnumerable<ExtraFee>> GetAllForVehicleAsync(Guid vehicleTypeId);
    
}