using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class FundMapper : BaseMapper<Fund, Domain.Fund>
{
    public FundMapper(IMapper mapper) : base(mapper)
    {
    }
}