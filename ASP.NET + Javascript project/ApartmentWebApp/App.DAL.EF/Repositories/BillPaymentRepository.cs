using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BillPaymentRepository : BaseEntityRepository<BillPayment, App.Domain.BillPayment, AppDbContext>, IBillPaymentRepository
{
    public BillPaymentRepository(AppDbContext dbContext, IMapper<BillPayment, App.Domain.BillPayment> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<BillPayment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}