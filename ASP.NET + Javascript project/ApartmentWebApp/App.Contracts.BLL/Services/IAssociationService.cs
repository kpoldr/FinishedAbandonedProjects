using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IAssociationService : IEntityService<App.BLL.DTO.Association>, IAssociationRepositoryCustom<App.BLL.DTO.Association>
{
    
}
