using App.BLL.DTO;
using App.BLL.DTO.Identity;
using App.Contracts.BLL.Services;
using App.Contracts.BLL.Services.Identity;
using App.Contracts.DAL;
using App.Contracts.DAL.Identity;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services.Identity;

public class AppUserService: BaseEntityService<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser, IAppUserRepository>, IAppUserService
{
    public AppUserService(IAppUserRepository repository, IMapper<AppUser, DAL.DTO.Identity.AppUser> mapper) : base(repository, mapper)
    {
    }


    public async Task<IEnumerable<AppUser>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}