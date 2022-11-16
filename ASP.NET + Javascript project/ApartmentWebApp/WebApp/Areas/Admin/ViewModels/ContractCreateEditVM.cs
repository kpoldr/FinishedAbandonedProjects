using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Contract = App.BLL.DTO.Contract;

namespace WebApp.Areas.Admin.ViewModels;

public class ContractCreateEditVm
{

    public App.BLL.DTO.Contract Contract { get; set; } = default!;
    public SelectList? AssociationSelectList { get; set; }
    public SelectList? OwnerSelectList { get; set; }

}