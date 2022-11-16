using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAssociationRepository : IEntityRepository<Association>, IAssociationRepositoryCustom<Association>
{
   
}

public interface IAssociationRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}