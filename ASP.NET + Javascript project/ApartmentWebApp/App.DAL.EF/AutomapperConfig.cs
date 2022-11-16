using App.DAL.DTO;
using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<AppUser, App.Domain.Identity.AppUser>().ReverseMap();
        CreateMap<Apartment, App.Domain.Apartment>().ReverseMap();
        CreateMap<Association, App.Domain.Association>().ReverseMap();
        CreateMap<Bill, App.Domain.Bill>().ReverseMap();
        CreateMap<BillPayment, App.Domain.BillPayment>().ReverseMap();
        CreateMap<Building, App.Domain.Building>().ReverseMap();
        CreateMap<Contract, App.Domain.Contract>().ReverseMap();
        CreateMap<Fund, App.Domain.Fund>().ReverseMap();
        CreateMap<Owner, App.Domain.Owner>().ReverseMap();
        CreateMap<Payment, App.Domain.Payment>().ReverseMap();
        CreateMap<Penalty, App.Domain.Penalty>().ReverseMap();
        CreateMap<Person, App.Domain.Person>().ReverseMap();
        CreateMap<UtilityBill, App.Domain.UtilityBill>().ReverseMap();
        CreateMap<Utility, App.Domain.Utility>().ReverseMap();
    }
}