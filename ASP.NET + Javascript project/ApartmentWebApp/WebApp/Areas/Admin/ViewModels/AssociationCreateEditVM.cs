using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Association = App.DAL.DTO.Association;

namespace WebApp.Areas.Admin.ViewModels;

public class AssociationCreateEditVM
{
    public App.BLL.DTO.Association Association { get; set; } = default!;
    public SelectList? AppUserSelectList { get; set; }
}