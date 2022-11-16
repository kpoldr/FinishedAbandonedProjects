using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IBuildingService : IEntityService<App.BLL.DTO.Building>, IBuildingRepositoryCustom<App.BLL.DTO.Building>
{
    
}
