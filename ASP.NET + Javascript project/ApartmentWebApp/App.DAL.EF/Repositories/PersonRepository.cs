using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PersonRepository : BaseEntityRepository<Person, App.Domain.Person, AppDbContext>, IPersonRepository
{
    public PersonRepository(AppDbContext dbContext, IMapper<Person, App.Domain.Person> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Person>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}