using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IBuildingRepository : IEntityRepository<Building>, IBuildingRepositoryCustom<Building>
{
    
}
public interface IBuildingRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}
