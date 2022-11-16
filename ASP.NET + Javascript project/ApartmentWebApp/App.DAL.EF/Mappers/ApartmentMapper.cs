using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ApartmentMapper : BaseMapper<Apartment, App.Domain.Apartment>
{
    public ApartmentMapper(IMapper mapper) : base(mapper)
    {
    }
}