namespace App.Public.DTO.v1.Mappers;


public class PaymentMapper 
{
    public App.BLL.DTO.Payment Map(App.Public.DTO.v1.Payment payment)
    {
        return new App.BLL.DTO.Payment()
        {
            PaymentDate = payment.PaymentDate,
            PaymentValue = payment.PaymentValue,
            OwnerId = payment.OwnerId,
            FundId = payment.FundId,
            PersonId = payment.PersonId
        };
    }
    
    public App.Public.DTO.v1.Payment Map(App.BLL.DTO.Payment payment)
    {
        return new App.Public.DTO.v1.Payment()
        {
            Id = payment.Id,
            PaymentDate = payment.PaymentDate,
            PaymentValue = payment.PaymentValue,
            OwnerId = payment.OwnerId,
            FundId = payment.FundId,
            PersonId = payment.PersonId
        };
    }
    
}