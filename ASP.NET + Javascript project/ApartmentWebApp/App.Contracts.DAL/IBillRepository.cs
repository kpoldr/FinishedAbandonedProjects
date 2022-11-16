using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IBillRepository : IEntityRepository<Bill>, IBillRepositoryCustom<Bill>
{
    
}

public interface IBillRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}