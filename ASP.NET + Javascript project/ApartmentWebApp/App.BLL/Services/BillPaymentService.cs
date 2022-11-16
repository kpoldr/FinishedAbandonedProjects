using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class BillPaymentService: BaseEntityService<App.BLL.DTO.BillPayment, App.DAL.DTO.BillPayment, IBillPaymentRepository>, IBillPaymentService
{
    public BillPaymentService(IBillPaymentRepository repository, IMapper<BLL.DTO.BillPayment, DAL.DTO.BillPayment> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<BillPayment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}