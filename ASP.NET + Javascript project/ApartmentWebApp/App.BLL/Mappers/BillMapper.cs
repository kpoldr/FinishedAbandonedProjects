using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class BillMapper : BaseMapper<App.BLL.DTO.Bill, App.DAL.DTO.Bill>
{
    public BillMapper(IMapper mapper) : base(mapper)
    {
    }
}