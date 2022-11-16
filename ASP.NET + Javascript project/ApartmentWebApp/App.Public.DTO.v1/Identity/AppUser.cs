using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1.Identity;

public class AppUser : DomainEntityId
{
    [MinLength((1))]
    [MaxLength((128))]
    public string FirstName { get; set; } = default!;
    
    [MinLength((1))]
    [MaxLength((128))]
    public string LastName { get; set; } = default!;
    public ICollection<Association>? Associations { get; set;}   
    public ICollection<Owner>? Owners { get; set;}

    public string FirstLastName => FirstName + " " + LastName;
    public string LastFirstName => LastName + " " + FirstName ;
}