using AutoMapper;

namespace App.Public.DTO.v1.MappingProfiles;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.BLL.DTO.Association, App.Public.DTO.v1.Association>().ReverseMap();
    }
}