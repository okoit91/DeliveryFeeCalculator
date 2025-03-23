using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.BLL;
using Weather = App.BLL.DTO.Weather;

namespace App.BLL.services;

public class WeatherService :
    BaseEntityService<App.DAL.DTO.Weather, App.BLL.DTO.Weather, IWeatherRepository>, IWeatherService
{
    public WeatherService(IAppUnitOfWork uow, IWeatherRepository repository, IMapper mapper) :
        base(uow, repository, new BllDalMapper<App.DAL.DTO.Weather, App.BLL.DTO.Weather>(mapper))
    {
        
    }
    public async Task<IEnumerable<Weather>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }

    public async Task<Weather?> GetLatestWeatherByCityAsync(Guid cityId)
    {
        var latestWeather = await Repository.GetLatestWeatherByCityAsync(cityId);
        return Mapper.Map(latestWeather);
    }
}