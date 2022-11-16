using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PaymentService: BaseEntityService<App.BLL.DTO.Payment, App.DAL.DTO.Payment, IPaymentRepository>, IPaymentService
{
    public PaymentService(IPaymentRepository repository, IMapper<BLL.DTO.Payment, DAL.DTO.Payment> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}