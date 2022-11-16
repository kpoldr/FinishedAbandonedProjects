﻿using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Bill = App.DAL.DTO.Bill;
using Building = App.DAL.DTO.Building;
using Owner = App.DAL.DTO.Owner;
using Person = App.DAL.DTO.Person;
using Utility = App.DAL.DTO.Utility;

namespace App.DAL.DTO;

public class Apartment : DomainEntityId
{
    [MaxLength(128)]
    public string Number { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.App.Domain.Apartment), Name = nameof(Floor))]
    public int Floor { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Apartment), Name = nameof(Size))]
    
    public double Size { get; set; }

    public ICollection<Utility>? Utilities { get; set; }
    public ICollection<Person>? Persons { get; set; }
    public ICollection<Bill>? Bills { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(OwnerId))]
    public Guid OwnerId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Owner))]
    public Owner? Owner { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Ids), Name = nameof(BuildingId))]
    public Guid BuildingId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.EntityNames), Name = nameof(Building))]

    public Building? Building { get; set; }
}