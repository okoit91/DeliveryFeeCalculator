using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleTypeRepository :
    BaseEntityRepository<APPDomain.VehicleType, DALDTO.VehicleType, AppDbContext>, IVehicleTypeRepository
{
    public VehicleTypeRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.VehicleType, DALDTO.VehicleType>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.VehicleType>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        var res = await query.ToListAsync();
        return (res).Select(e => Mapper.Map(e));
    }
    
    public async Task<IEnumerable<App.DAL.DTO.VehicleType>> GetAllSortedWithoutUserAsync()
    {
        var query = CreateQuery(); //  Call CreateQuery() without userId
        var result = await query.OrderBy(v => v.Name).ToListAsync();
        return result.Select(vehicle => Mapper.Map(vehicle));
    }
}