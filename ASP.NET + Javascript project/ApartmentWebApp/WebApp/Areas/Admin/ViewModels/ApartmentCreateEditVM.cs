using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class ApartmentCreateEditVM
{
    public App.BLL.DTO.Apartment Apartment { get; set; } = default!;
    public SelectList? OwnersSelectList { get; set; }
    public SelectList? BuildingSelectList { get; set; }
}