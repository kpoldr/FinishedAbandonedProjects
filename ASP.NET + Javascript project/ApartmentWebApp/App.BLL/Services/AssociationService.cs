using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class AssociationService: BaseEntityService<App.BLL.DTO.Association, App.DAL.DTO.Association, IAssociationRepository>, IAssociationService
{
    
    public AssociationService(IAssociationRepository repository, IMapper<BLL.DTO.Association, DAL.DTO.Association> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Association>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}