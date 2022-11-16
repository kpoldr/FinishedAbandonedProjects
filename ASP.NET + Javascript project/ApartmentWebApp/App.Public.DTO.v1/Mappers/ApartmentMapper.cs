namespace App.Public.DTO.v1.Mappers;

public class ApartmentMapper
{
    private readonly PersonMapper _personMapper = new PersonMapper();
    private readonly BillMapper _billMapper = new BillMapper();
    private readonly UtilityMapper _utilityMapper = new UtilityMapper();
    
    public App.BLL.DTO.Apartment Map(App.Public.DTO.v1.Apartment apartment)
    {
        
        var persons = new List<BLL.DTO.Person>();
        var bills = new List<BLL.DTO.Bill>();
        var utilities = new List<BLL.DTO.Utility>();

        if (apartment.Persons != null)
            persons.AddRange(apartment.Persons.Select(person => _personMapper.Map(person)));
        else
        {
            persons = null;
        }
        
        if (apartment.Bills != null)
            bills.AddRange(apartment.Bills.Select(bill => _billMapper.Map(bill)));
        else
        {
            bills = null;
        }
        
        if (apartment.Utilities != null)
            utilities.AddRange(apartment.Utilities.Select(utility => _utilityMapper.Map(utility)));
        else
        {
            utilities = null;
        }
        
        return new App.BLL.DTO.Apartment()
        {
            Number = apartment.Number,
            Floor = apartment.Floor,
            Size = apartment.Size,
            Utilities = utilities,
            Persons = persons,
            Bills = bills,
            OwnerId = apartment.OwnerId,
            BuildingId = apartment.BuildingId,

        };
    }
    
    public App.Public.DTO.v1.Apartment Map(App.BLL.DTO.Apartment apartment)
    {
        var persons = new List<App.Public.DTO.v1.Person>();
        var bills = new List<App.Public.DTO.v1.Bill>();
        var utilities = new List<App.Public.DTO.v1.Utility>();

        if (apartment.Persons != null)
            persons.AddRange(apartment.Persons.Select(person => _personMapper.Map(person)));
        else
        {
            persons = null;
        }
        
        if (apartment.Bills != null)
            bills.AddRange(apartment.Bills.Select(bill => _billMapper.Map(bill)));
        else
        {
            bills = null;
        }
        
        if (apartment.Utilities != null)
            utilities.AddRange(apartment.Utilities.Select(utility => _utilityMapper.Map(utility)));
        else
        {
            utilities = null;
        }
        
        
        return new App.Public.DTO.v1.Apartment()
        {
            Id = apartment.Id,
            Floor = apartment.Floor,
            Size = apartment.Size,
            Utilities = utilities,
            Persons = persons,
            Bills = bills,
            OwnerId = apartment.OwnerId,
            BuildingId = apartment.BuildingId,
        };
    }
}