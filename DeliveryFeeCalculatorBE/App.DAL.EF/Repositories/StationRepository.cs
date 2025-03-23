using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class StationRepository : 
    BaseEntityRepository<APPDomain.Station, DALDTO.Station, AppDbContext>, IStationRepository
{
    public StationRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Station, DALDTO.Station>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.Station>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        var res = await query.ToListAsync();
        return (res).Select(e => Mapper.Map(e));
    }
    
    public async Task<IEnumerable<DALDTO.Station>> GetAllSortedAsync()
    {
        var query = CreateQuery()
            .Include(e => e.City);
        var res = await query.ToListAsync();
        return res.Select(e => Mapper.Map(e)).ToList();
    }
    
    public async Task<DALDTO.Station?> FirstOrDefaultByNameAsync(string name)
    {
        var query = CreateQuery();
        var domainStation = await query.FirstOrDefaultAsync(c => c.StationName == name);
        return domainStation == null ? null : Mapper.Map(domainStation);
    }
    
    public async Task<DALDTO.Station?> FirstOrDefaultAsyncCustom(Guid id)
    {
        var query = CreateQuery()
            .Include(e => e.City)
            .Where(e => e.Id == id);

        var entity = await query.FirstOrDefaultAsync();
        if (entity == null) return null;
        
        var dto = Mapper.Map(entity);
        return dto;
    }
}