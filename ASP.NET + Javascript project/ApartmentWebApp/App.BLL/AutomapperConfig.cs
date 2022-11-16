using AutoMapper;

namespace App.BLL;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<App.BLL.DTO.Apartment, App.DAL.DTO.Apartment>().ReverseMap();
        CreateMap<App.BLL.DTO.Association, App.DAL.DTO.Association>().ReverseMap();
        CreateMap<App.BLL.DTO.Bill, App.DAL.DTO.Bill>().ReverseMap();
        CreateMap<App.BLL.DTO.BillPayment, App.DAL.DTO.BillPayment>().ReverseMap();
        CreateMap<App.BLL.DTO.Building, App.DAL.DTO.Building>().ReverseMap();
        CreateMap<App.BLL.DTO.Contract, App.DAL.DTO.Contract>().ReverseMap();
        CreateMap<App.BLL.DTO.Fund, App.DAL.DTO.Fund>().ReverseMap();
        CreateMap<App.BLL.DTO.Owner, App.DAL.DTO.Owner>().ReverseMap();
        CreateMap<App.BLL.DTO.Payment, App.DAL.DTO.Payment>().ReverseMap();
        CreateMap<App.BLL.DTO.Penalty, App.DAL.DTO.Penalty>().ReverseMap();
        CreateMap<App.BLL.DTO.Person, App.DAL.DTO.Person>().ReverseMap();
        CreateMap<App.BLL.DTO.UtilityBill, App.DAL.DTO.UtilityBill>().ReverseMap();
        CreateMap<App.BLL.DTO.Utility, App.DAL.DTO.Utility>().ReverseMap();
    }
}