using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Penalty = App.BLL.DTO.Penalty;

namespace WebApp.Areas.Admin.ViewModels;

public class PenaltyCreateEditVm
{

    public App.BLL.DTO.Penalty Penalty { get; set; } = default!;
    public SelectList? BillSelectList { get; set; }
    public SelectList? OwnerSelectList { get; set; }

}