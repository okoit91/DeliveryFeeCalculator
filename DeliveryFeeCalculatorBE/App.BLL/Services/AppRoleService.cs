using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.services;

public class AppRoleService :
    BaseEntityService<App.DAL.DTO.AppRole, App.BLL.DTO.AppRole, IAppRoleRepository>, IAppRoleService
{
    public AppRoleService(IAppUnitOfWork uow, IAppRoleRepository repository, IMapper mapper) :
        base(uow, repository, new BllDalMapper<App.DAL.DTO.AppRole, App.BLL.DTO.AppRole>(mapper))
    {
    }

    public async Task<IEnumerable<AppRole>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
}