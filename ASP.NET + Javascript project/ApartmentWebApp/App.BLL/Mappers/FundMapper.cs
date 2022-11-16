using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class FundMapper : BaseMapper<App.BLL.DTO.Fund, App.DAL.DTO.Fund>
{
    public FundMapper(IMapper mapper) : base(mapper)
    {
    }
}