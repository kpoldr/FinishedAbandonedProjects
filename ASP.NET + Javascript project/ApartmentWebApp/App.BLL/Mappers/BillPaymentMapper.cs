using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class BillPaymentMapper : BaseMapper<App.BLL.DTO.BillPayment, App.DAL.DTO.BillPayment>
{
    public BillPaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}