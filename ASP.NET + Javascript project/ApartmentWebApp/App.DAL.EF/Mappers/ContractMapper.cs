using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ContractMapper : BaseMapper<Contract, Domain.Contract>
{
    public ContractMapper(IMapper mapper) : base(mapper)
    {
    }
}