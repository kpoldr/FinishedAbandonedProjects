using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IContractRepository : IEntityRepository<Contract>, IContractRepositoryCustom<Contract>
{
    
}

public interface IContractRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}