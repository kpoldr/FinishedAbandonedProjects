using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Person = App.BLL.DTO.Person;

namespace WebApp.Areas.Admin.ViewModels;

public class PersonCreateEditVm
{

    public App.BLL.DTO.Person Person { get; set; } = default!;
    public SelectList? ApartmentSelectList { get; set; }

}