using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PaymentMapper : BaseMapper<Payment, Domain.Payment>
{
    public PaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}