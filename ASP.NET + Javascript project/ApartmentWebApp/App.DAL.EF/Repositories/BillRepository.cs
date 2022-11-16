using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts.Base;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Bill = App.DAL.DTO.Bill;

namespace App.DAL.EF.Repositories;

public class BillRepository : BaseEntityRepository<Bill, App.Domain.Bill, AppDbContext>, IBillRepository
{
    public BillRepository(AppDbContext dbContext, IMapper<Bill, App.Domain.Bill> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}