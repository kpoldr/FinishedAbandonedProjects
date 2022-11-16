using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UtilityBillRepository : BaseEntityRepository<UtilityBill, App.Domain.UtilityBill, AppDbContext>, IUtilityBillRepository
{
    public UtilityBillRepository(AppDbContext dbContext, IMapper<UtilityBill, App.Domain.UtilityBill> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<UtilityBill>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}