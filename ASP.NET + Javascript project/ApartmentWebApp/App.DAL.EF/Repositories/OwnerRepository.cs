using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class OwnerRepository : BaseEntityRepository<Owner, App.Domain.Owner, AppDbContext>, IOwnerRepository
{
    public OwnerRepository(AppDbContext dbContext, IMapper<Owner, App.Domain.Owner> mapper) : base(dbContext, mapper)
    {
        
    }

    public async Task<IEnumerable<Owner>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(u => u.AppUser);
            // .Where(m => m.AppUserId == userId);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}