using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IBillPaymentRepository :  IEntityRepository<BillPayment>,  IBillPaymentRepositoryCustom<BillPayment>
{
    
}

public interface IBillPaymentRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
}