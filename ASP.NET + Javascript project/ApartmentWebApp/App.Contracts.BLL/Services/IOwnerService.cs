using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IOwnerService : IEntityService<App.BLL.DTO.Owner>, IOwnerRepositoryCustom<App.BLL.DTO.Owner>
{
    
}
