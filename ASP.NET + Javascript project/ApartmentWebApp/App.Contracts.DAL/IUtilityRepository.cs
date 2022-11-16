using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUtilityRepository : IEntityRepository<Utility>,  IUtilityRepositoryCustom<Utility>
{
    
}
public interface IUtilityRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}