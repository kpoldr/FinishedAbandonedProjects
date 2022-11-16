using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class Association : DomainEntityId
{
    
    [MaxLength(256)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Name))]
    public LangStr Name { get; set; } = default!;


    [MaxLength(4096)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Description))]
    public LangStr? Description { get; set; }


    [MaxLength(256)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Association), Name = nameof(Email))]
    public string Email { get; set; } = default!;
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Association), Name = nameof(Phone))]
    public int Phone { get; set; }

    [MaxLength(256)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Association), Name = nameof(BankName))]
    public LangStr? BankName { get; set; }

    [MaxLength(256)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Association), Name = nameof(BankNumber))]
    public string? BankNumber { get; set; }

    public ICollection<Building>? Buildings { get; set; }
    public ICollection<Fund>? Funds { get; set; }
    public ICollection<Contract>? Contracts { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(AppUserId))]
    public Guid? AppUserId { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

}