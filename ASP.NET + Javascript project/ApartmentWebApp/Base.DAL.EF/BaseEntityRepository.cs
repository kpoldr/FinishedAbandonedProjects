using Base.Contracts.Base;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

// TODO: do not fetch unneeded data from db on every request

public class BaseEntityRepository<TDalEntity,TDomainTEntity, TDbContext> : BaseEntityRepository<TDalEntity,TDomainTEntity, Guid, TDbContext>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDomainTEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity,TDomainTEntity> mapper) : base(dbContext, mapper)
    {
    }
}

public class BaseEntityRepository<TDalEntity,TDomainTEntity, TKey, TDbContext> : IEntityRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainTEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainTEntity> RepoDbSet;
    protected readonly IMapper<TDalEntity, TDomainTEntity> Mapper;
    

   

    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity,TDomainTEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TDomainTEntity>();
        Mapper = mapper;
    }
    
    protected virtual IQueryable<TDomainTEntity> CreateQuery(bool noTracking = true)
    {

        var query = RepoDbSet.AsQueryable();

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
    
    
    public virtual TDalEntity Add(TDalEntity entity)
    {
        
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        // var dalEntity = _mapper.Map(entity);
        // return _mapper.Map(RepoDbSet.Add(dalEntity).Entity);
        return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);

        if (entity == null)
        {
            throw new NullReferenceException($"{typeof(TDalEntity).Name} with id {id} was not found");
        }

        return Remove(entity);
    }

    public virtual TDalEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(a => a.Id.Equals(id)));
    }

    public virtual IEnumerable<TDalEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(x => Mapper.Map(x)!);
    }

    public virtual  bool Exists(TKey id)
    {
        return RepoDbSet.Any(a => a.Id.Equals(id));
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(x => Mapper.Map(x)!);
    } 

    public virtual async  Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
    }

    public virtual async  Task<TDalEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);

        if (entity == null)
        {
            throw new NullReferenceException($"{typeof(TDalEntity).Name} with id {id} was not found");
        }

        return Remove(entity);
    }
}