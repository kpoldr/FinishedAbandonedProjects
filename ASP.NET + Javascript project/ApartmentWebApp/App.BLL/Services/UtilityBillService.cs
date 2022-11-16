using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class UtilityBillService: BaseEntityService<App.BLL.DTO.UtilityBill, App.DAL.DTO.UtilityBill, IUtilityBillRepository>, IUtilityBillService
{
    public UtilityBillService(IUtilityBillRepository repository, IMapper<BLL.DTO.UtilityBill, DAL.DTO.UtilityBill> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<UtilityBill>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}