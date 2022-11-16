using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UtilityMapper : BaseMapper<App.BLL.DTO.Utility, App.DAL.DTO.Utility>
{
    public UtilityMapper(IMapper mapper) : base(mapper)
    {
    }
}