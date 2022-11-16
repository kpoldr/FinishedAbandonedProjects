using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PenaltyMapper : BaseMapper<App.BLL.DTO.Penalty, App.DAL.DTO.Penalty>
{
    public PenaltyMapper(IMapper mapper) : base(mapper)
    {
    }
}