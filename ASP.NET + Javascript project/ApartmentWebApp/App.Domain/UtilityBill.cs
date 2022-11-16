using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class UtilityBill : DomainEntityMetaId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.UtilityBill), Name = nameof(Quantity))]
    public int Quantity { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.UtilityBill), Name = nameof(Price))]
    public double Price { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(UtilityId))]
    public Guid UtilityId { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Utility))]
    public Utility? Utility { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(BillId))]
    public Guid BillId { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Bill))]
    public Bill? Bill { get; set; }
}