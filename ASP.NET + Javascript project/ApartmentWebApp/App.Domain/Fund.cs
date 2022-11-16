using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Fund : DomainEntityMetaId
{
    [MaxLength(256)] 
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.CommonOverlap), Name = nameof(Name) )]
    public LangStr Name { get; set; } = default!;
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Fund), Name = nameof(Value) )]
    public double Value { get; set; }

    public ICollection<Payment>? Payments { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(AssociationId))]
    public Guid AssociationId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Association))]
    public Association? Association { get; set; }
}