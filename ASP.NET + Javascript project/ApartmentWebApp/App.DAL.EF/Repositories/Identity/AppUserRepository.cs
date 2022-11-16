using App.Contracts.DAL.Identity;
using App.DAL.DTO.Identity;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.Identity;

public class AppUserRepository : BaseEntityRepository<AppUser, App.Domain.Identity.AppUser, AppDbContext>, IAppUserRepository
{
    public AppUserRepository(AppDbContext dbContext, IMapper<AppUser, App.Domain.Identity.AppUser> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Include(x => x.Email);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}