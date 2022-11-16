using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IUtilityBillService : IEntityService<App.BLL.DTO.UtilityBill>, IUtilityBillRepositoryCustom<App.BLL.DTO.UtilityBill>
{
    
}
