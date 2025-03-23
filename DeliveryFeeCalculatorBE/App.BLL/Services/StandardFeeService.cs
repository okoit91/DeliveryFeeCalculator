using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using StandardFee = App.BLL.DTO.StandardFee;

namespace App.BLL.Services
{
    public class StandardFeeService :
        BaseEntityService<App.DAL.DTO.StandardFee, App.BLL.DTO.StandardFee, IStandardFeeRepository>, IStandardFeeService
    {
        public StandardFeeService(IAppUnitOfWork uow, IStandardFeeRepository repository, IMapper mapper) :
            base(uow, repository, new BllDalMapper<App.DAL.DTO.StandardFee, App.BLL.DTO.StandardFee>(mapper))
        {
        }

        // Existing method that requires a userId
        public async Task<IEnumerable<StandardFee>> GetAllSortedAsync(Guid userId)
        {
            return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
        }
        
        // New overload that does not require a userId
        public async Task<IEnumerable<StandardFee>> GetAllSortedAsync()
        {
            return (await Repository.GetAllSortedAsync()).Select(e => Mapper.Map(e));
        }
        
        public async Task<StandardFee?> GetByCityAndVehicleAsync(Guid cityId, Guid vehicleTypeId)
        {
            var standardFee = await Repository.GetByCityAndVehicleAsync(cityId, vehicleTypeId);
            return Mapper.Map(standardFee);
        }
        
        public async Task<StandardFee?> FirstOrDefaultAsyncCustom(Guid id)
        { 
            var entity = await Repository.FirstOrDefaultAsyncCustom(id);
            return entity != null ? Mapper.Map(entity) : null;
        }
    }
}