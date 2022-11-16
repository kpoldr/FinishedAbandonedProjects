using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class BuildingMapper : BaseMapper<App.BLL.DTO.Building, App.DAL.DTO.Building>
{
    public BuildingMapper(IMapper mapper) : base(mapper)
    {
    }
}