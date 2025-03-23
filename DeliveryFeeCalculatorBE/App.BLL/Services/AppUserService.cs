using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.services;

public class AppUserService :
    BaseEntityService<App.DAL.DTO.AppUser, App.BLL.DTO.AppUser, IAppUserRepository>, IAppUserService
{
    public AppUserService(IAppUnitOfWork uow, IAppUserRepository repository, IMapper mapper) :
        base(uow, repository, new BllDalMapper<App.DAL.DTO.AppUser, App.BLL.DTO.AppUser>(mapper))
    {
    }

    public async Task<IEnumerable<AppUser>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
}