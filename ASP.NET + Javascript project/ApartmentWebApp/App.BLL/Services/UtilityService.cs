using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class UtilityService: BaseEntityService<App.BLL.DTO.Utility, App.DAL.DTO.Utility, IUtilityRepository>, IUtilityService
{
    public UtilityService(IUtilityRepository repository, IMapper<BLL.DTO.Utility, DAL.DTO.Utility> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Utility>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}