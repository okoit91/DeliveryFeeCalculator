using App.Contracts.DAL.Repositories;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class WeatherRepository : BaseEntityRepository<APPDomain.Weather, DALDTO.Weather, AppDbContext>,
    IWeatherRepository
{

    public WeatherRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Weather, DALDTO.Weather>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.Weather>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        var res = await query.ToListAsync();
        return (res).Select(e => Mapper.Map(e));
    }

    public async Task<DALDTO.Weather?> GetLatestWeatherByCityAsync(Guid cityId)
    {
        var query = CreateQuery()
            .Include(w => w.Station)
            .Where(w => w.Station != null && w.Station.CityId == cityId) 
            .OrderByDescending(w => w.Timestamp);

        var latestWeather = await query.FirstOrDefaultAsync();

        return latestWeather == null ? null : Mapper.Map(latestWeather);
    }
}