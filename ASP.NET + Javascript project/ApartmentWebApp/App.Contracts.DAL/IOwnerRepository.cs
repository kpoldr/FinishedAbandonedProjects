using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IOwnerRepository : IEntityRepository<Owner>, IOwnerRepositoryCustom<Owner>
{
    
}
public interface IOwnerRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}