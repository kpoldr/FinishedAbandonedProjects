using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IContractService : IEntityService<App.BLL.DTO.Contract>, IContractRepositoryCustom<App.BLL.DTO.Contract>
{
    
}
