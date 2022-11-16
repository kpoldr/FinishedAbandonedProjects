using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class Owner : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Name) )]
    [MaxLength(256)] public LangStr Name { get; set; } = default!;
    
    [MaxLength(256)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Email) )]
    public string Email { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.App.Domain.Owner), Name = nameof(Phone) )]
    public int Phone { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Owner), Name = nameof(AdvancedPay) )]
    public double AdvancedPay { get; set; }

    public ICollection<Payment>? Payments { get; set; }
    public ICollection<Bill>? Bills { get; set; }
    public ICollection<Penalty>? Penalties { get; set; }
    public ICollection<Apartment>? Apartments { get; set; }
    public ICollection<Contract>? Contracts { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(AppUserId))]
    public Guid? AppUserId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}