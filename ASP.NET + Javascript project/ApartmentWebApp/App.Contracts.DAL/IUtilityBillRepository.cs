using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUtilityBillRepository : IEntityRepository<UtilityBill>, IUtilityBillRepositoryCustom<UtilityBill>
{
    
}

public interface IUtilityBillRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}