using Microsoft.AspNetCore.Mvc.Rendering;


namespace WebApp.Areas.Admin.ViewModels;

public class OwnerCreateEditVm
{

    public App.BLL.DTO.Owner Owner { get; set; } = default!;
    public SelectList? AppUserSelectList { get; set; }
    
}