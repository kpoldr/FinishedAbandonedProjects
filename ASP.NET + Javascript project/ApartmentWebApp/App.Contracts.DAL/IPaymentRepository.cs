using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPaymentRepository : IEntityRepository<Payment>, IPaymentRepositoryCustom<Payment>
{
    
}

public interface IPaymentRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}