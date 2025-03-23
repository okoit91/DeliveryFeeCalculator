using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IStationService :
    IEntityRepository<App.BLL.DTO.Station>, IStationRepositoryCustom<App.BLL.DTO.Station>
{
    
}