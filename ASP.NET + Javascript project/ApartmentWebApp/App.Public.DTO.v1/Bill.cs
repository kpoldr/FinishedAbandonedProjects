using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Bill : DomainEntityId
{
    [MaxLength(128)] public string Number { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Date))]
    public DateTime Date { get; set; }

    public DateTime DeadLine { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(ApartmentId))]
    public Guid? ApartmentId { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Apartment))]
    public Apartment? Apartment { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(OwnerId))]
    public Guid? OwnerId { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Owner))]
    public Owner? Owner { get; set; }

    public ICollection<UtilityBill>? UtilityBills { get; set; }
    public ICollection<Penalty>? Penalties { get; set; }
    public ICollection<BillPayment>? BillPayments { get; set; }
    
    public Guid? PreviousBillId { get; set; }
    public virtual Bill? PreviousBill { get; set; }
    
}