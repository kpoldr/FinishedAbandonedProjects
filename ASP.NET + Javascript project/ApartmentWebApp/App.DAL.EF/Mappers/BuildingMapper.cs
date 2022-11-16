using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BuildingMapper : BaseMapper<Building, Domain.Building>
{
    public BuildingMapper(IMapper mapper) : base(mapper)
    {
    }
}