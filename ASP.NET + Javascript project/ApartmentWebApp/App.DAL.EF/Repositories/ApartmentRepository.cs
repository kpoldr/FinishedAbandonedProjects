using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ApartmentRepository : BaseEntityRepository<Apartment, App.Domain.Apartment, AppDbContext>, IApartmentRepository
{
    public ApartmentRepository(AppDbContext dbContext,  IMapper<Apartment, App.Domain.Apartment> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Apartment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}