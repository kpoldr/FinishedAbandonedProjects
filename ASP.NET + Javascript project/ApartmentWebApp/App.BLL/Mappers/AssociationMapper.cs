using AutoMapper;
using Base.DAL;


namespace App.BLL.Mappers;

public class AssociationMapper : BaseMapper<App.BLL.DTO.Association, App.DAL.DTO.Association>
{
    public AssociationMapper(IMapper mapper) : base(mapper)
    {
    }
}