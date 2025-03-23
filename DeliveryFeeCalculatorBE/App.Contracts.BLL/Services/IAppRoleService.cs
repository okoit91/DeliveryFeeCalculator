using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IAppRoleService : IEntityRepository<App.BLL.DTO.AppRole>, IAppRoleRepositoryCustom<App.BLL.DTO.AppRole>
{
    
}