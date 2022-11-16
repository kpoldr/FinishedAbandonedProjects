using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class UtilityBillMapper : BaseMapper<UtilityBill, Domain.UtilityBill>
{
    public UtilityBillMapper(IMapper mapper) : base(mapper)
    {
    }
}