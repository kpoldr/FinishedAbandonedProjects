using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IPersonService : IEntityService<App.BLL.DTO.Person>, IPersonRepositoryCustom<App.BLL.DTO.Person>
{
    
}
