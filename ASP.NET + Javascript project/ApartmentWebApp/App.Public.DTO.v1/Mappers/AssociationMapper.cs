using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

// public class AssociationMapper
// {
//     
// }

public class AssociationMapper 
{
    private readonly BuildingMapper _buildingMapper = new BuildingMapper();
    private readonly FundMapper _fundMapper = new FundMapper();
    private readonly ContractMapper _contractMapper = new ContractMapper();
    
    
    public App.BLL.DTO.Association Map(App.Public.DTO.v1.Association association)
    {
        var buildings = new List<BLL.DTO.Building>();
        var funds = new List<BLL.DTO.Fund>();
        var contracts = new List<BLL.DTO.Contract>();

        if (association.Buildings != null)
            buildings.AddRange(association.Buildings.Select(building => _buildingMapper.Map(building)));
        else
        {
            buildings = null;
        }
        
        if (association.Funds != null)
            funds.AddRange(association.Funds.Select(fund => _fundMapper.Map(fund)));
        else
        {
            funds = null;
        }
        
        if (association.Contracts != null)
            contracts.AddRange(association.Contracts.Select(contract => _contractMapper.Map(contract)));
        else
        {
            contracts = null;
        }
        
        return new App.BLL.DTO.Association()
        {
            Name = association.Name,
            Description = association.Description,
            Email = association.Email,
            Phone = association.Phone,
            BankName = association.BankName,
            BankNumber = association.BankNumber,
            Buildings = buildings,
            Funds = funds,
            Contracts = contracts,
            AppUserId = association.AppUserId
        };
    }
    
    public App.Public.DTO.v1.Association Map(App.BLL.DTO.Association association)
    {
        
        var buildings = new List<App.Public.DTO.v1.Building>();
        var funds = new List<App.Public.DTO.v1.Fund>();
        var contracts = new List<App.Public.DTO.v1.Contract>();

        if (association.Buildings != null)
            buildings.AddRange(association.Buildings.Select(building => _buildingMapper.Map(building)));
        else
        {
            buildings = null;
        }
        
        if (association.Funds != null)
            funds.AddRange(association.Funds.Select(fund => _fundMapper.Map(fund)));
        else
        {
            funds = null;
        }
        
        if (association.Contracts != null)
            contracts.AddRange(association.Contracts.Select(contract => _contractMapper.Map(contract)));
        else
        {
            contracts = null;
        }
        
        
        
        return new App.Public.DTO.v1.Association()
        {
            Id = association.Id,
            Name = association.Name,
            Description = association.Description,
            Email = association.Email,
            Phone = association.Phone,
            BankName = association.BankName,
            BankNumber = association.BankNumber,
            Buildings = buildings,
            Funds = funds,
            Contracts = contracts,
            AppUserId = association.AppUserId
        };
    }

    
}