using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class BillPayment : DomainEntityMetaId
{
    public Guid BillId { get; set; }
    public Bill? Bill { get; set; }
    
    public Guid PaymentId { get; set; }
    public Payment? Payment { get; set; }
}