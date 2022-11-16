using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ApartmentMapper : BaseMapper<App.BLL.DTO.Apartment, App.DAL.DTO.Apartment>
{
    public ApartmentMapper(IMapper mapper) : base(mapper)
    {
    }
}