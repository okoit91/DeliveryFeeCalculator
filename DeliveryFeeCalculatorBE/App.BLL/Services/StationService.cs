using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using City = App.BLL.DTO.City;

namespace App.BLL.services;

public class StationService :
    BaseEntityService<App.DAL.DTO.Station, App.BLL.DTO.Station, IStationRepository>, IStationService
{
    public StationService(IAppUnitOfWork uow, IStationRepository repository, IMapper mapper) :
        base(uow, repository, new BllDalMapper<App.DAL.DTO.Station, App.BLL.DTO.Station>(mapper))
    {
        
    }
    public async Task<IEnumerable<Station>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
    
    public async Task<IEnumerable<Station>> GetAllSortedAsync()
    {
        return (await Repository.GetAllSortedAsync()).Select(e => Mapper.Map(e));
    }
    
    public async Task<Station?> FirstOrDefaultByNameAsync(string name)
    {
        var station = await Repository.FirstOrDefaultByNameAsync(name);
        return station == null ? null : Mapper.Map(station);
    }
    
    public async Task<Station?> FirstOrDefaultAsyncCustom(Guid id)
    { 
        var entity = await Repository.FirstOrDefaultAsyncCustom(id);
        return entity != null ? Mapper.Map(entity) : null;
    }
    
}