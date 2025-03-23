using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.BLL;
using VehicleType = App.BLL.DTO.VehicleType;

namespace App.BLL.services;

public class VehicleTypeService :
    BaseEntityService<App.DAL.DTO.VehicleType, App.BLL.DTO.VehicleType, IVehicleTypeRepository>, IVehicleTypeService
{
    public VehicleTypeService(IAppUnitOfWork uow, IVehicleTypeRepository repository, IMapper mapper) :
        base(uow, repository, new BllDalMapper<App.DAL.DTO.VehicleType, App.BLL.DTO.VehicleType>(mapper))
    {
        
    }
    public async Task<IEnumerable<VehicleType>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
    
    public async Task<IEnumerable<App.BLL.DTO.VehicleType>> GetAllSortedWithoutUserAsync()
    {
        var vehicleTypes = await Repository.GetAllSortedWithoutUserAsync();
        return vehicleTypes.Select(vehicle => Mapper.Map(vehicle));
    }
}