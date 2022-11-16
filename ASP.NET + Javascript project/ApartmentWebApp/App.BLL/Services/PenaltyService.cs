using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PenaltyService: BaseEntityService<App.BLL.DTO.Penalty, App.DAL.DTO.Penalty, IPenaltyRepository>, IPenaltyService
{
    public PenaltyService(IPenaltyRepository repository, IMapper<BLL.DTO.Penalty, DAL.DTO.Penalty> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Penalty>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}