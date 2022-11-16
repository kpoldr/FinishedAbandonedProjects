using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PaymentRepository : BaseEntityRepository<Payment, App.Domain.Payment, AppDbContext>, IPaymentRepository
{
    public PaymentRepository(AppDbContext dbContext, IMapper<Payment, App.Domain.Payment> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}