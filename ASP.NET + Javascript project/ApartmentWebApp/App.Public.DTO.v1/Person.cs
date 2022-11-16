using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Person  : DomainEntityId
{   
    [MaxLength(256)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Name) )]
    public LangStr? Name { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Person), Name = nameof(BoardMember))]
    public bool BoardMember { get; set; } = false;
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(ApartmentId))]
    public Guid? ApartmentId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Apartment))]
    public Apartment? Apartment { get; set; }

    public ICollection<Payment>? Payments { get; set; }
}