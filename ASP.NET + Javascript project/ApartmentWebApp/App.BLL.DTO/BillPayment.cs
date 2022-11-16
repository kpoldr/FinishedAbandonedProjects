using Base.Domain;

namespace App.BLL.DTO;

public class BillPayment : DomainEntityId
{
    public Guid BillId { get; set; }
    public Bill? Bill { get; set; }
    
    public Guid PaymentId { get; set; }
    public Payment? Payment { get; set; }
}