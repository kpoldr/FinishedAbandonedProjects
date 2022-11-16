namespace App.Public.DTO.v1.Mappers;


public class OwnerMapper 
{
    private readonly PaymentMapper _paymentMapper = new PaymentMapper();
    private readonly BillMapper _billMapper = new BillMapper();
    private readonly PenaltyMapper _penaltyMapper = new PenaltyMapper();
    private readonly ApartmentMapper _apartmentMapper = new ApartmentMapper();
    private readonly ContractMapper _contractMapper = new ContractMapper();
    
    public App.BLL.DTO.Owner Map(App.Public.DTO.v1.Owner owner)
    {
        var payments = new List<BLL.DTO.Payment>();
        var bills = new List<BLL.DTO.Bill>();
        var penalties = new List<BLL.DTO.Penalty>();
        var apartments = new List<BLL.DTO.Apartment>();
        var contracts = new List<BLL.DTO.Contract>();

        if (owner.Payments != null)
            payments.AddRange(owner.Payments.Select(payment => _paymentMapper.Map(payment)));
        else
        {
            payments = null;
        }
        
        if (owner.Bills != null)
            bills.AddRange(owner.Bills.Select(bill => _billMapper.Map(bill)));
        else
        {
            bills = null;
        }
        
        if (owner.Penalties != null)
            penalties.AddRange(owner.Penalties.Select(penalty => _penaltyMapper.Map(penalty)));
        else
        {
            penalties = null;
        }
        
        if (owner.Apartments != null)
            apartments.AddRange(owner.Apartments.Select(apartment => _apartmentMapper.Map(apartment)));
        else
        {
            apartments = null;
        }
        
        if (owner.Contracts != null)
            contracts.AddRange(owner.Contracts.Select(contract => _contractMapper.Map(contract)));
        else
        {
            contracts = null;
        }
        
        return new App.BLL.DTO.Owner()
        {
            Name = owner.Name,
            Email = owner.Email,
            Phone = owner.Phone,
            AdvancedPay = owner.AdvancedPay,
            Payments = payments,
            Bills = bills,
            Penalties = penalties,
            Apartments = apartments,
            Contracts = contracts,
            AppUserId = owner.AppUserId
        };
    }
    
    public App.Public.DTO.v1.Owner Map(App.BLL.DTO.Owner owner)
    {
        var payments = new List<App.Public.DTO.v1.Payment>();
        var bills = new List<App.Public.DTO.v1.Bill>();
        var penalties = new List<App.Public.DTO.v1.Penalty>();
        var apartments = new List<App.Public.DTO.v1.Apartment>();
        var contracts = new List<App.Public.DTO.v1.Contract>();

        if (owner.Payments != null)
            payments.AddRange(owner.Payments.Select(payment => _paymentMapper.Map(payment)));
        else
        {
            payments = null;
        }
        
        if (owner.Bills != null)
            bills.AddRange(owner.Bills.Select(bill => _billMapper.Map(bill)));
        else
        {
            bills = null;
        }
        
        if (owner.Penalties != null)
            penalties.AddRange(owner.Penalties.Select(penalty => _penaltyMapper.Map(penalty)));
        else
        {
            penalties = null;
        }
        
        if (owner.Apartments != null)
            apartments.AddRange(owner.Apartments.Select(apartment => _apartmentMapper.Map(apartment)));
        else
        {
            apartments = null;
        }
        
        if (owner.Contracts != null)
            contracts.AddRange(owner.Contracts.Select(contract => _contractMapper.Map(contract)));
        else
        {
            contracts = null;
        }
        
        
        return new App.Public.DTO.v1.Owner()
        {
            Id = owner.Id,
            Name = owner.Name,
            Email = owner.Email,
            Phone = owner.Phone,
            AdvancedPay = owner.AdvancedPay,
            Payments = null,
            Bills = null,
            Penalties = null,
            Apartments = null,
            Contracts = null,
            AppUserId = owner.AppUserId
        };
    }
    
}