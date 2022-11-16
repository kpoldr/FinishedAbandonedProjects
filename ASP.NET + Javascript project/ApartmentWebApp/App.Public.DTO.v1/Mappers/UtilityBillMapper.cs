namespace App.Public.DTO.v1.Mappers;


public class UtilityBillMapper 
{
    public App.BLL.DTO.UtilityBill Map(App.Public.DTO.v1.UtilityBill utilityBill)
    {
        return new App.BLL.DTO.UtilityBill()
        {
            Quantity = utilityBill.Quantity,
            Price = utilityBill.Price,
            UtilityId = utilityBill.UtilityId,
            BillId = utilityBill.BillId,
        };
    }
    
    public App.Public.DTO.v1.UtilityBill Map(App.BLL.DTO.UtilityBill utilityBill)
    {
        return new App.Public.DTO.v1.UtilityBill()
        {
            Id = utilityBill.Id,
            Quantity = utilityBill.Quantity,
            Price = utilityBill.Price,
            UtilityId = utilityBill.UtilityId,
            BillId = utilityBill.BillId,
        };
    }
    
}