using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ICityService : IEntityRepository<App.BLL.DTO.City>, ICityRepositoryCustom<App.BLL.DTO.City>
{
    Task<IEnumerable<App.BLL.DTO.City>> GetAllSortedWithoutUserAsync();
}