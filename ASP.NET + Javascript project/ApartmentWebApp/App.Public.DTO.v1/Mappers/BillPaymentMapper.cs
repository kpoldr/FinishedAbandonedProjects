namespace App.Public.DTO.v1.Mappers;

public class BillPaymentMapper
{
    public App.BLL.DTO.BillPayment Map(App.Public.DTO.v1.BillPayment billPayment)
    {
        return new App.BLL.DTO.BillPayment()
        {
            BillId = billPayment.BillId,
            PaymentId = billPayment.PaymentId,

        };
    }
    
    public App.Public.DTO.v1.BillPayment Map(App.BLL.DTO.BillPayment billPayment)
    {
        return new App.Public.DTO.v1.BillPayment()
        {
            Id = billPayment.Id,
            BillId = billPayment.BillId,
            PaymentId = billPayment.PaymentId,
        };
    }
}