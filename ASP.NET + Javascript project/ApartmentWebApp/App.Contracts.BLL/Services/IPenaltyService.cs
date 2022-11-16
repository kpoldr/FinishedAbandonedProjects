using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IPenaltyService : IEntityService<App.BLL.DTO.Penalty>, IPenaltyRepositoryCustom<App.BLL.DTO.Penalty>
{
    
}
