using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.BLL.DTO;

public class Penalty : DomainEntityId
{
    
    [MaxLength(128)]
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Penalty), Name = nameof(PenaltyName) )]
    public LangStr PenaltyName { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.App.Domain.Penalty), Name = nameof(Value))]
    public double Value { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Penalty), Name = nameof(Multiplier))]
    public double Multiplier { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(BillId))]
    public Guid? BillId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Bill))]
    public Bill? Bill { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(OwnerId))]
    public Guid? OwnerId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Owner))]
    public Owner? Owner { get; set; }
}