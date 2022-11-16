using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ContractMapper : BaseMapper<App.BLL.DTO.Contract, App.DAL.DTO.Contract>
{
    public ContractMapper(IMapper mapper) : base(mapper)
    {
    }
}