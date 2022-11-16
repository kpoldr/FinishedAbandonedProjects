using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BuildingRepository : BaseEntityRepository<Building, App.Domain.Building, AppDbContext>, IBuildingRepository
{
    public BuildingRepository(AppDbContext dbContext, IMapper<Building, App.Domain.Building> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Building>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}