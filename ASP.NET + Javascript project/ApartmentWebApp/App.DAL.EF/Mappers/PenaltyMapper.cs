using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PenaltyMapper : BaseMapper<Penalty, Domain.Penalty>
{
    public PenaltyMapper(IMapper mapper) : base(mapper)
    {
    }
}