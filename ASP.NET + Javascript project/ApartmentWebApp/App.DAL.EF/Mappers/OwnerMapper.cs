using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class OwnerMapper : BaseMapper<Owner, Domain.Owner>
{
    public OwnerMapper(IMapper mapper) : base(mapper)
    {
    }
}