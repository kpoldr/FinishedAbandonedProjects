using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Utility : DomainEntityMetaId

{
    [MaxLength(256)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Name))]
    public LangStr Name { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(ApartmentId))]
    public Guid? ApartmentId { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Apartment))]
    public Apartment? Apartment { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(BuildingId))]
    public Guid? BuildingId { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Building))]
    public Building? Building { get; set; }

    public ICollection<UtilityBill>? UtilityBills { get; set; }
}