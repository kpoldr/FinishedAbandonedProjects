using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BillPaymentMapper : BaseMapper<BillPayment, Domain.BillPayment>
{
    public BillPaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}