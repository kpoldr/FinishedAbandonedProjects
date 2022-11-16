using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using UtilityBill = App.BLL.DTO.UtilityBill;

namespace WebApp.Areas.Admin.ViewModels;

public class UtilityBillCreateEditVm
{

    public App.BLL.DTO.UtilityBill UtilityBill { get; set; } = default!;
    public SelectList? BillSelectList { get; set; }
    public SelectList? UtilitySelectList { get; set; }

}