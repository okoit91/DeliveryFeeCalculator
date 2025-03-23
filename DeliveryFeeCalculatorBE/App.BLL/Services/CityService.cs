using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using City = App.BLL.DTO.City;

namespace App.BLL.services;

public class CityService :
    BaseEntityService<App.DAL.DTO.City, App.BLL.DTO.City, ICityRepository>, ICityService
{
    public CityService(IAppUnitOfWork uow, ICityRepository repository, IMapper mapper) :
        base(uow, repository, new BllDalMapper<App.DAL.DTO.City, App.BLL.DTO.City>(mapper))
    {
        
    }
    public async Task<IEnumerable<City>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
    
    public async Task<City?> FirstOrDefaultByNameAsync(string name)
    {
        var city = await Repository.FirstOrDefaultByNameAsync(name);
        return city == null ? null : Mapper.Map(city);
    }
    
    public async Task<IEnumerable<App.BLL.DTO.City>> GetAllSortedWithoutUserAsync()
    {
        var cities = await Repository.GetAllSortedWithoutUserAsync();
        return cities.Select(city => Mapper.Map(city));
    }
    
}