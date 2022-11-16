namespace App.Public.DTO.v1.Mappers;


public class PersonMapper 
{
    private readonly PaymentMapper _paymentMapper = new PaymentMapper();
   
        
    public App.BLL.DTO.Person Map(App.Public.DTO.v1.Person person)
    {
        var payments = new List<BLL.DTO.Payment>();

        if (person.Payments != null)
            payments.AddRange(person.Payments.Select(payment => _paymentMapper.Map(payment)));
        else
        {
            payments = null;
        }
        
        return new App.BLL.DTO.Person()
        {
            Name = person.Name,
            BoardMember = person.BoardMember,
            ApartmentId = person.ApartmentId,
            Payments = payments,
        };
    }
    
    public App.Public.DTO.v1.Person Map(App.BLL.DTO.Person person)
    {
        var payments = new List<App.Public.DTO.v1.Payment>();

        if (person.Payments != null)
            payments.AddRange(person.Payments.Select(payment => _paymentMapper.Map(payment)));
        else
        {
            payments = null;
        }
        
        return new App.Public.DTO.v1.Person()
        {
            Id = person.Id,
            Name = person.Name,
            BoardMember = person.BoardMember,
            ApartmentId = person.ApartmentId,
            Payments = payments,
        };
    }
    
}