using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UtilityBillMapper : BaseMapper<App.BLL.DTO.UtilityBill, App.DAL.DTO.UtilityBill>
{
    public UtilityBillMapper(IMapper mapper) : base(mapper)
    {
    }
}