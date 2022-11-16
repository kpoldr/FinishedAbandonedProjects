using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class BuildingService: BaseEntityService<App.BLL.DTO.Building, App.DAL.DTO.Building, IBuildingRepository>, IBuildingService
{
    public BuildingService(IBuildingRepository repository, IMapper<BLL.DTO.Building, DAL.DTO.Building> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Building>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}