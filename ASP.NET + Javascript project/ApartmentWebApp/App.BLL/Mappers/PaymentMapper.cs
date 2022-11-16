using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PaymentMapper : BaseMapper<App.BLL.DTO.Payment, App.DAL.DTO.Payment>
{
    public PaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}