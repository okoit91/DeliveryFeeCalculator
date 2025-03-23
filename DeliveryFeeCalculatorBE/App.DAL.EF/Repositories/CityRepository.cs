using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;


public class CityRepository : BaseEntityRepository<APPDomain.City, DALDTO.City, AppDbContext>, ICityRepository
{
    public CityRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.City, DALDTO.City>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.City>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        var res = await query.ToListAsync();
        return (res).Select(e => Mapper.Map(e));
    }

    public async Task<DALDTO.City?> FirstOrDefaultByNameAsync(string name)
    {
        var query = CreateQuery();
        var domainCity = await query.FirstOrDefaultAsync(c => c.Name == name);
        return domainCity == null ? null : Mapper.Map(domainCity);
    }
    
    public async Task<IEnumerable<App.DAL.DTO.City>> GetAllSortedWithoutUserAsync()
    {
        var query = CreateQuery(); //  Call CreateQuery() without userId
        var result = await query.OrderBy(c => c.Name).ToListAsync();
        return result.Select(city => Mapper.Map(city));
    }
}