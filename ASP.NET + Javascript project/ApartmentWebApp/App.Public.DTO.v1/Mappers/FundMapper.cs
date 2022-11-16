namespace App.Public.DTO.v1.Mappers;

public class FundMapper
{   
    private readonly PaymentMapper _paymentMapper = new PaymentMapper();
    public App.BLL.DTO.Fund Map(App.Public.DTO.v1.Fund fund)
    {
        var payments = new List<BLL.DTO.Payment>();

        if (fund.Payments != null)
            payments.AddRange(fund.Payments.Select(payment => _paymentMapper.Map(payment)));
        else
        {
            payments = null;
        }
        
        return new App.BLL.DTO.Fund()
        {
            Name = fund.Name,
            Value = fund.Value,
            Payments = payments,
            AssociationId = fund.AssociationId,
        };
    }
    
    public App.Public.DTO.v1.Fund Map(App.BLL.DTO.Fund fund)
    {
        var payments = new List<App.Public.DTO.v1.Payment>();

        if (fund.Payments != null)
            payments.AddRange(fund.Payments.Select(payment => _paymentMapper.Map(payment)));
        else
        {
            payments = null;
        }
        
        return new App.Public.DTO.v1.Fund()
        {
            Id = fund.Id,
            Name = fund.Name,
            Value = fund.Value,
            Payments = null,
            AssociationId = fund.AssociationId,
        };
    }
}