using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using ExtraFee = App.BLL.DTO.ExtraFee;

namespace App.BLL.services;

public class ExtraFeeService :
    BaseEntityService<App.DAL.DTO.ExtraFee, App.BLL.DTO.ExtraFee, IExtraFeeRepository>, IExtraFeeService
{
    public ExtraFeeService(IAppUnitOfWork uow, IExtraFeeRepository repository, IMapper mapper) :
        base(uow, repository, new BllDalMapper<App.DAL.DTO.ExtraFee, App.BLL.DTO.ExtraFee>(mapper))
    {
        
    }
    
    public async Task<IEnumerable<ExtraFee>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
    
    public async Task<IEnumerable<ExtraFee>> GetAllSortedAsync()
    {
        return (await Repository.GetAllSortedAsync()).Select(e => Mapper.Map(e));
    }

    
    
    public async Task<ExtraFee?> FirstOrDefaultAsyncCustom(Guid id)
    { 
        var entity = await Repository.FirstOrDefaultAsyncCustom(id);
        return entity != null ? Mapper.Map(entity) : null; //  Correctly mapped result
    }

    public async Task<IEnumerable<ExtraFee>> GetAllForVehicleAsync(Guid vehicleTypeId)
    {
        var extraFees = await Repository.GetAllForVehicleAsync(vehicleTypeId);
        return extraFees.Select(Mapper.Map);
    }
    
}