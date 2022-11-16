using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class FundService: BaseEntityService<App.BLL.DTO.Fund, App.DAL.DTO.Fund, IFundRepository>, IFundService
{
    public FundService(IFundRepository repository, IMapper<BLL.DTO.Fund, DAL.DTO.Fund> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Fund>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
    
}