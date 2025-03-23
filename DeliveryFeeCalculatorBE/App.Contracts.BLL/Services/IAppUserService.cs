using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IAppUserService : IEntityRepository<App.BLL.DTO.AppUser>, IAppUserRepositoryCustom<App.BLL.DTO.AppUser>
{
    
}