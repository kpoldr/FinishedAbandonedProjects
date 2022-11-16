using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IUtilityService : IEntityService<App.BLL.DTO.Utility>, IUtilityRepositoryCustom<App.BLL.DTO.Utility>
{
    
}
