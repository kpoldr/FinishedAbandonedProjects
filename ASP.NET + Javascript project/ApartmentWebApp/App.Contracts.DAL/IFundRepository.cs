using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IFundRepository : IEntityRepository<Fund>, IFundRepositoryCustom<Fund>
{
    
}

public interface IFundRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}