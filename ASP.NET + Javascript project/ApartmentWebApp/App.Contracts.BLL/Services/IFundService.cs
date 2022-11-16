using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IFundService : IEntityService<App.BLL.DTO.Fund>, IFundRepositoryCustom<App.BLL.DTO.Fund>
{
    
}
