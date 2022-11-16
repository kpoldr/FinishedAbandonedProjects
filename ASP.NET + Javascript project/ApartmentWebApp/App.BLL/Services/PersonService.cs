using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PersonService: BaseEntityService<App.BLL.DTO.Person, App.DAL.DTO.Person, IPersonRepository>, IPersonService
{
    public PersonService(IPersonRepository repository, IMapper<BLL.DTO.Person, DAL.DTO.Person> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Person>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}