using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IBillPaymentService : IEntityService<App.BLL.DTO.BillPayment>, IBillPaymentRepositoryCustom<App.BLL.DTO.BillPayment>
{
    
}
