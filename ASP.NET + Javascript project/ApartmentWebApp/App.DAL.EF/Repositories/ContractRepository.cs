using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContractRepository : BaseEntityRepository<Contract, App.Domain.Contract, AppDbContext>, IContractRepository
{
    public ContractRepository(AppDbContext dbContext, IMapper<Contract, App.Domain.Contract> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Contract>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}