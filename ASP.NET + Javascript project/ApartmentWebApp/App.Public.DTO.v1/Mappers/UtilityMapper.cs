namespace App.Public.DTO.v1.Mappers;


public class UtilityMapper 
{
    private readonly UtilityBillMapper _utilityMapper = new UtilityBillMapper();
    
    public App.BLL.DTO.Utility Map(App.Public.DTO.v1.Utility utility)
    {
        var utilityBills = new List<BLL.DTO.UtilityBill>();

        if (utility.UtilityBills != null)
            utilityBills.AddRange(utility.UtilityBills.Select(utilityBill => _utilityMapper.Map(utilityBill)));
        else
        {
            utilityBills = null;
        }
        
        return new App.BLL.DTO.Utility()
        {
            Name = utility.Name,
            ApartmentId = utility.ApartmentId,
            BuildingId = utility.BuildingId,
            UtilityBills = utilityBills,
        };
    }
    
    public App.Public.DTO.v1.Utility Map(App.BLL.DTO.Utility utility)
    {
        var utilityBills = new List<App.Public.DTO.v1.UtilityBill>();

        if (utility.UtilityBills != null)
            utilityBills.AddRange(utility.UtilityBills.Select(utilityBill => _utilityMapper.Map(utilityBill)));
        else
        {
            utilityBills = null;
        }

        return new App.Public.DTO.v1.Utility()
        {
            Id = utility.Id,
            Name = utility.Name,
            ApartmentId = utility.ApartmentId,
            BuildingId = utility.BuildingId,
            UtilityBills = utilityBills,
        };
    }
    
}