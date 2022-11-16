using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UtilityRepository: BaseEntityRepository<Utility, App.Domain.Utility, AppDbContext>, IUtilityRepository
{
    public UtilityRepository(AppDbContext dbContext, IMapper<Utility, App.Domain.Utility> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Utility>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}