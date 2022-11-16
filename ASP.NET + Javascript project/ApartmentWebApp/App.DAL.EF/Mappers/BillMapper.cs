using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BillMapper : BaseMapper<Bill, Domain.Bill>
{
    public BillMapper(IMapper mapper) : base(mapper)
    {
    }
}