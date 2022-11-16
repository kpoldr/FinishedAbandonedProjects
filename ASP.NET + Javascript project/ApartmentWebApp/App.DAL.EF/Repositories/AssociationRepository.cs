using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AssociationRepository : BaseEntityRepository<Association, App.Domain.Association, AppDbContext>,
    IAssociationRepository
{
    public AssociationRepository(AppDbContext dbContext, IMapper<Association, App.Domain.Association> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Association>> GetAllByNameAsync(string name, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        return (await query.Where(a => a.Name.ToString().ToUpper().Contains(name.ToUpper())).ToListAsync()).Select(x =>
            Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Association>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(u => u.AppUser);
            // .Where(m => m.AppUserId == userId);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }


    // public Task<IEnumerable<AppUser>> GetAllUsers(Guid associationId, bool noTracking = true)
    // {
    //     var query = CreateQuery(noTracking);
    //     query = query
    //         .Include(u => u.AppUser)
    //         .Where(a => a.Id == associationId);
    //     
    //     return await query.ToListAsync();
    // }
    //
    // public Task<Association> FirstOrDefaultAsyncWithUsers(Guid associationId, bool noTracking = true)
    // {
    //     var query = CreateQuery(noTracking);
    //     query = query
    //         .Include(u => u.AppUser)
    //         .Where(a => a.Id == associationId);
    //
    //     var result = query.SingleOrDefault() ?? throw new InvalidOperationException();
    //     
    //     return Task.FromResult(result);
    // }
}