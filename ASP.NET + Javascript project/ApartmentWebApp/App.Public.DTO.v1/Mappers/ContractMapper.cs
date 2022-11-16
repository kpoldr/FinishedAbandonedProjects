namespace App.Public.DTO.v1.Mappers;

public class ContractMapper
{
    public App.BLL.DTO.Contract Map(App.Public.DTO.v1.Contract contract)
    {
        return new App.BLL.DTO.Contract()
        {
            Name = contract.Name,
            Description = contract.Description,
            OwnerId = contract.OwnerId,
            AssociationId = contract.AssociationId,
        };
    }
    
    public App.Public.DTO.v1.Contract Map(App.BLL.DTO.Contract contract)
    {
        return new App.Public.DTO.v1.Contract()
        {
            Id = contract.Id,
            Name = contract.Name,
            Description = contract.Description,
            OwnerId = contract.OwnerId,
            AssociationId = contract.AssociationId,
        };
    }
}