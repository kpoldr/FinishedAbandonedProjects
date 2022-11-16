using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ContractService: BaseEntityService<App.BLL.DTO.Contract, App.DAL.DTO.Contract, IContractRepository>, IContractService
{
    public ContractService(IContractRepository repository, IMapper<BLL.DTO.Contract, DAL.DTO.Contract> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Contract>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}