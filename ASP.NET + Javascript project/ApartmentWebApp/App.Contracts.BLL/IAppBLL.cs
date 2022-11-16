using App.Contracts.BLL.Services;
using App.Contracts.BLL.Services.Identity;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IAppUserService AppUser { get; }
    IApartmentService Apartment { get; }
    IAssociationService Association { get; }
    IBillService Bill { get; }
    IBillPaymentService BillPayment { get; }
    IBuildingService Building { get; }
    IContractService Contract { get; }
    IFundService Fund { get; }
    IOwnerService Owner { get; }
    IPaymentService Payment { get; }
    IPenaltyService Penalty { get; }
    IPersonService Person { get; }
    IUtilityBillService UtilityBill { get; }
    IUtilityService Utility { get; }
}