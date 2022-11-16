using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IPaymentService : IEntityService<App.BLL.DTO.Payment>, IPaymentRepositoryCustom<App.BLL.DTO.Payment>
{
    
}
