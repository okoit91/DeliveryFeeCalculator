using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;


public class ExtraFeeRepository : BaseEntityRepository<APPDomain.ExtraFee, DALDTO.ExtraFee, AppDbContext>, IExtraFeeRepository
{
    public ExtraFeeRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.ExtraFee, DALDTO.ExtraFee>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.ExtraFee>> GetAllSortedAsync()
    {
        var query = CreateQuery()
            .Include(e => e.VehicleType);
        var res = await query.ToListAsync();
        return res.Select(e => Mapper.Map(e)).ToList();
    }
    
    public async Task<IEnumerable<DALDTO.ExtraFee>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        var res = await query.ToListAsync();
        return (res).Select(e => Mapper.Map(e));
    }
    
    public async Task<IEnumerable<App.DAL.DTO.ExtraFee>> GetAllForVehicleAsync(Guid vehicleTypeId)
    {
        var query = CreateQuery()
            .Where(f => f.VehicleTypeId == vehicleTypeId);

        var extraFees = await query.ToListAsync();
        return extraFees.Select(Mapper.Map);
    }
    
    public async Task<DALDTO.ExtraFee?> FirstOrDefaultAsyncCustom(Guid id)
    {
        var query = CreateQuery()
            .Include(e => e.VehicleType) // Eager load VehicleType
            .Where(e => e.Id == id);

        var entity = await query.FirstOrDefaultAsync();
        if (entity == null) return null;
        
        var dto = Mapper.Map(entity);
        return dto;
    }
}