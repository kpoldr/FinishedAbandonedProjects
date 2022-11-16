using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillPayment = App.BLL.DTO.BillPayment;

namespace WebApp.Areas.Admin.ViewModels;

public class BillPaymentCreateEditVm
{

    public App.BLL.DTO.BillPayment BillPayment { get; set; } = default!;
    public SelectList? BillSelectList { get; set; }
    public SelectList? PaymentSelectList { get; set; }

}