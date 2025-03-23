using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AppUserRepository : BaseEntityRepository<APPDomain.Identity.AppUser, DALDTO.AppUser, AppDbContext>, IAppUserRepository
{
    public AppUserRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Identity.AppUser, DALDTO.AppUser>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.AppUser>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        

        var res = await query.ToListAsync();
//        query = query.OrderBy(c => c.ContestName);
        return (res).Select(e => Mapper.Map(e));
    }
}