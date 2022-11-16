using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Payment = App.BLL.DTO.Payment;

namespace WebApp.Areas.Admin.ViewModels;

public class PaymentCreateEditVm
{

    public App.BLL.DTO.Payment Payment { get; set; } = default!;
    public SelectList? FundSelectList { get; set; }
    public SelectList? OwnerSelectList { get; set; }
    public SelectList? PersonSelectList { get; set; }
    
    public SelectList? BillSelectList { get; set; }
    
}