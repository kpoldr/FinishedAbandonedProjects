namespace App.Public.DTO.v1.Mappers;

public class BuildingMapper
{
    private readonly ApartmentMapper _apartmentMapper = new ApartmentMapper();
    private readonly UtilityMapper _utilityMapper = new UtilityMapper();
    public App.BLL.DTO.Building Map(App.Public.DTO.v1.Building building)
    {

        var apartments = new List<BLL.DTO.Apartment>();
        var utilities = new List<BLL.DTO.Utility>();

        if (building.Apartments != null)
            apartments.AddRange(building.Apartments.Select(apartment => _apartmentMapper.Map(apartment)));
        else
        {
            apartments = null;
        }
        
        if (building.Utilities != null)
            utilities.AddRange(building.Utilities.Select(utility => _utilityMapper.Map(utility)));
        else
        {
            utilities = null;
        }

        
        return new App.BLL.DTO.Building()
        {
            Address = building.Address,
            Utilities = utilities,
            Apartments = apartments,
            AssociationId = building.AssociationId,

        };
    }
    
    public App.Public.DTO.v1.Building Map(App.BLL.DTO.Building building)
    {
        var apartments = new List<App.Public.DTO.v1.Apartment>();
        var utilities = new List<App.Public.DTO.v1.Utility>();

        if (building.Apartments != null)
            apartments.AddRange(building.Apartments.Select(apartment => _apartmentMapper.Map(apartment)));
        else
        {
            apartments = null;
        }
        
        if (building.Utilities != null)
            utilities.AddRange(building.Utilities.Select(utility => _utilityMapper.Map(utility)));
        else
        {
            utilities = null;
        }
        
        
        return new App.Public.DTO.v1.Building()
        {
            Id = building.Id,
            Address = building.Address,
            Utilities = utilities,
            Apartments = apartments,
            AssociationId = building.AssociationId,
        };
    }
}