using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AppRoleRepository : BaseEntityRepository<APPDomain.Identity.AppRole, DALDTO.AppRole, AppDbContext>, IAppRoleRepository
{
    public AppRoleRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Identity.AppRole, DALDTO.AppRole>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.AppRole>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        

        var res = await query.ToListAsync();
        return (res).Select(e => Mapper.Map(e));
    }
}