using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class BillService: BaseEntityService<App.BLL.DTO.Bill, App.DAL.DTO.Bill, IBillRepository>, IBillService
{
    public BillService(IBillRepository repository, IMapper<BLL.DTO.Bill, DAL.DTO.Bill> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}