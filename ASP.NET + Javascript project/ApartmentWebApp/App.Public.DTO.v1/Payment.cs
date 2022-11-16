using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Payment : DomainEntityId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.Payment), Name = nameof(PaymentDate))]
    public DateTime PaymentDate { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Payment), Name = nameof(PaymentValue))]
    public double PaymentValue { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(OwnerId))]
    public Guid? OwnerId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Owner))]
    public Owner? Owner { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(FundId))]
    public Guid? FundId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Fund))]
    public Fund? Fund { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(PersonId))]
    public Guid? PersonId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Person))]
    public Person? Person { get; set; }
    
}