namespace App.Public.DTO.v1.Mappers;


public class PenaltyMapper 
{
    public App.BLL.DTO.Penalty Map(App.Public.DTO.v1.Penalty penalty)
    {
        return new App.BLL.DTO.Penalty()
        {
            PenaltyName = penalty.PenaltyName,
            Value = penalty.Value,
            Multiplier = penalty.Multiplier,
            BillId = penalty.BillId,
            OwnerId = penalty.OwnerId
        };
    }
    
    public App.Public.DTO.v1.Penalty Map(App.BLL.DTO.Penalty penalty)
    {
        return new App.Public.DTO.v1.Penalty()
        {
            Id = penalty.Id,
            PenaltyName = penalty.PenaltyName,
            Value = penalty.Value,
            Multiplier = penalty.Multiplier,
            BillId = penalty.BillId,
            OwnerId = penalty.OwnerId
        };
    }
    
}