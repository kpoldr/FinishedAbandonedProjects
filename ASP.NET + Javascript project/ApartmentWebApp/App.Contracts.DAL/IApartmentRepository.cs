using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IApartmentRepository : IEntityRepository<Apartment>, IApartmentRepositoryCustom<Apartment>
{
}
public interface IApartmentRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}