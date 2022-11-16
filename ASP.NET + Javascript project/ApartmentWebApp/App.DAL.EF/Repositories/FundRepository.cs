using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class FundRepository : BaseEntityRepository<Fund, App.Domain.Fund, AppDbContext>, IFundRepository
{
    public FundRepository(AppDbContext dbContext, IMapper<Fund, App.Domain.Fund> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Fund>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}