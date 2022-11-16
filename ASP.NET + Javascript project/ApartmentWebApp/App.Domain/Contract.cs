using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Contract : DomainEntityMetaId
{
    [MaxLength(256)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Name))]
    public LangStr Name { get; set; } = default!;

    [MaxLength(256)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Description))]
    public LangStr Description { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(OwnerId))]
    public Guid OwnerId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Owner))]
    public Owner? Owner { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(AssociationId))]
    public Guid AssociationId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Association))]
    public Association? Association { get; set; }
}