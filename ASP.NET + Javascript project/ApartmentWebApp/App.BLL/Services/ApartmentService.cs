using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ApartmentService: BaseEntityService<App.BLL.DTO.Apartment, App.DAL.DTO.Apartment, IApartmentRepository>, IApartmentService
{
    public ApartmentService(IApartmentRepository repository, IMapper<BLL.DTO.Apartment, DAL.DTO.Apartment> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Apartment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}