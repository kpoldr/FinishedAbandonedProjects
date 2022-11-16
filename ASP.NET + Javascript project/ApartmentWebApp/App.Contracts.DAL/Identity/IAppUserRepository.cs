using App.DAL.DTO.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Identity;

public interface IAppUserRepository : IEntityRepository<AppUser>, IAppUserRepositoryCustom<AppUser>
{
}

public interface IAppUserRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}