using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PenaltyRepository : BaseEntityRepository<Penalty, App.Domain.Penalty, AppDbContext>, IPenaltyRepository
{
    public PenaltyRepository(AppDbContext dbContext, IMapper<Penalty, App.Domain.Penalty> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Penalty>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}