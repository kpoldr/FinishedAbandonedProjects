using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class OwnerService: BaseEntityService<App.BLL.DTO.Owner, App.DAL.DTO.Owner, IOwnerRepository>, IOwnerService
{
    public OwnerService(IOwnerRepository repository, IMapper<BLL.DTO.Owner, DAL.DTO.Owner> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Owner>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}