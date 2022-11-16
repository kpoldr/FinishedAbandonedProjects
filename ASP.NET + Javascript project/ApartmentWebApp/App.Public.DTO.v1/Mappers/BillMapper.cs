namespace App.Public.DTO.v1.Mappers;

public class BillMapper
{
    private readonly UtilityBillMapper _utilityMapper = new UtilityBillMapper();
    private readonly PenaltyMapper _penaltyMapper = new PenaltyMapper();
    private readonly BillPaymentMapper _billPaymentMapper = new BillPaymentMapper();
    
    
    public App.BLL.DTO.Bill Map(App.Public.DTO.v1.Bill bill)
    {
        var utilityBills = new List<BLL.DTO.UtilityBill>();
        var penalties = new List<BLL.DTO.Penalty>();
        var billPayments = new List<BLL.DTO.BillPayment>();

        if (bill.UtilityBills != null)
            utilityBills.AddRange(bill.UtilityBills.Select(utilityBill => _utilityMapper.Map(utilityBill)));
        else
        {
            utilityBills = null;
        }
        
        if (bill.Penalties != null)
            penalties.AddRange(bill.Penalties.Select(penalty => _penaltyMapper.Map(penalty)));
        else
        {
            penalties = null;
        }
        
        if (bill.BillPayments != null)
            billPayments.AddRange(bill.BillPayments.Select(billPayment => _billPaymentMapper.Map(billPayment)));
        else
        {
            billPayments = null;
        }
        
        return new App.BLL.DTO.Bill()
        {
            Date = bill.Date,
            DeadLine = bill.DeadLine,
            ApartmentId = bill.ApartmentId,
            OwnerId = bill.OwnerId,
            UtilityBills = utilityBills,
            Penalties = penalties,
            BillPayments = billPayments,
            PreviousBillId = bill.PreviousBillId,

        };
    }
    
    public App.Public.DTO.v1.Bill Map(App.BLL.DTO.Bill bill)
    {
        var utilityBills = new List<App.Public.DTO.v1.UtilityBill>();
        var penalties = new List<App.Public.DTO.v1.Penalty>();
        var billPayments = new List<App.Public.DTO.v1.BillPayment>();

        if (bill.UtilityBills != null)
            utilityBills.AddRange(bill.UtilityBills.Select(utilityBill => _utilityMapper.Map(utilityBill)));
        else
        {
            utilityBills = null;
        }
        
        if (bill.Penalties != null)
            penalties.AddRange(bill.Penalties.Select(penalty => _penaltyMapper.Map(penalty)));
        else
        {
            penalties = null;
        }
        
        if (bill.BillPayments != null)
            billPayments.AddRange(bill.BillPayments.Select(billPayment => _billPaymentMapper.Map(billPayment)));
        else
        {
            billPayments = null;
        }
        
        
        return new App.Public.DTO.v1.Bill()
        {
            Id = bill.Id,
            Date = bill.Date,
            DeadLine = bill.DeadLine,
            ApartmentId = bill.ApartmentId,
            OwnerId = bill.OwnerId,
            UtilityBills = null,
            Penalties = null,
            BillPayments = null,
            PreviousBillId = bill.PreviousBillId,
        };
    }
}