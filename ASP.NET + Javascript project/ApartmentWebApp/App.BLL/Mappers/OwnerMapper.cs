using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class OwnerMapper : BaseMapper<App.BLL.DTO.Owner, App.DAL.DTO.Owner>
{
    public OwnerMapper(IMapper mapper) : base(mapper)
    {
    }
}