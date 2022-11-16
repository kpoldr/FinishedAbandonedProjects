using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPenaltyRepository : IEntityRepository<Penalty>, IPenaltyRepositoryCustom<Penalty>
{
    
}

public interface IPenaltyRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}