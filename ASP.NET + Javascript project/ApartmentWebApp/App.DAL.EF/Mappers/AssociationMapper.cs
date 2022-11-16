using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AssociationMapper : BaseMapper<Association, Domain.Association>
{
    public AssociationMapper(IMapper mapper) : base(mapper)
    {
    }
}