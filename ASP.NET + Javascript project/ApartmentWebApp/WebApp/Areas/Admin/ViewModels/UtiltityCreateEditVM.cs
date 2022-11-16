using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class UtilityCreateEditVm
{

    public App.BLL.DTO.Utility Utility { get; set; } = default!;
    public SelectList? ApartmentSelectList { get; set; }
    public SelectList? BuildingSelectList { get; set; }

}