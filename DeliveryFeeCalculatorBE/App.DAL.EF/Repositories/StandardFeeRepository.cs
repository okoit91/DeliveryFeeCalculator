using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class StandardFeeRepository : 
    BaseEntityRepository<APPDomain.StandardFee, DALDTO.StandardFee, AppDbContext>, IStandardFeeRepository
{
    public StandardFeeRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.StandardFee, DALDTO.StandardFee>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.StandardFee>> GetAllSortedAsync()
    {
        var query = CreateQuery()
            .Include(e => e.City)
            .Include(e => e.VehicleType);
        var res = await query.ToListAsync();
        return res.Select(e => Mapper.Map(e)).ToList();
    }
    
    public async Task<App.DAL.DTO.StandardFee?> GetByCityAndVehicleAsync(Guid cityId, Guid vehicleTypeId)
    {
        var query = CreateQuery()
            .Where(sf => sf.CityId == cityId && sf.VehicleTypeId == vehicleTypeId);

        var standardFee = await query.FirstOrDefaultAsync();
        return Mapper.Map(standardFee);
    }

    public async Task<IEnumerable<DALDTO.StandardFee>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        var res = await query.ToListAsync();
        return (res).Select(e => Mapper.Map(e));
    }
    
    public async Task<DALDTO.StandardFee?> FirstOrDefaultAsyncCustom(Guid id)
    {
        var query = CreateQuery()
            .Include(e => e.City)
            .Include(e => e.VehicleType)
            .Where(e => e.Id == id);

        var entity = await query.FirstOrDefaultAsync();
        if (entity == null) return null;
        
        var dto = Mapper.Map(entity);
        return dto;
    }
}