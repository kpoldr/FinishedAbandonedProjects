using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bill = App.BLL.DTO.Bill;

namespace WebApp.Areas.Admin.ViewModels;

public class BillCreateEditVM
{
    public App.BLL.DTO.Bill Bill { get; set; } = default!;
    
    public SelectList? ApartmentSelectList { get; set; }
    public SelectList? OwnerSelectList { get; set; }
}