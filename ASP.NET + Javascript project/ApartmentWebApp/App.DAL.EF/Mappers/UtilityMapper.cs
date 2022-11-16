using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class UtilityMapper : BaseMapper<Utility, Domain.Utility>
{
    public UtilityMapper(IMapper mapper) : base(mapper)
    {
    }
}