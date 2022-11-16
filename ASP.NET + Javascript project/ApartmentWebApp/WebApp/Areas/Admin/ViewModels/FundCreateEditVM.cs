using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fund = App.BLL.DTO.Fund;

namespace WebApp.Areas.Admin.ViewModels;

public class FundCreateEditVm
{

    public App.BLL.DTO.Fund Fund { get; set; } = default!;
    public SelectList? AssociationSelectList { get; set; }

}