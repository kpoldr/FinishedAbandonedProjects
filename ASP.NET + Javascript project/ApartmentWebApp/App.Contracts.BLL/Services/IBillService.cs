using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IBillService : IEntityService<App.BLL.DTO.Bill>, IBillPaymentRepositoryCustom<App.BLL.DTO.Bill>
{
    
}
