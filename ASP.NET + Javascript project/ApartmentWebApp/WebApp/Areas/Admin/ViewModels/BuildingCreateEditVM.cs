using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class BuilidngCreateEditVM
{
    public App.BLL.DTO.Building Building { get; set; } = default!;
    public SelectList? AssociationSelectList { get; set; }

}