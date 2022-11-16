using App.Contracts.DAL.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IAppUserRepository AppUser { get; }
    IApartmentRepository Apartment { get; }
    IAssociationRepository Association { get; }
    IBillRepository Bill { get; }
    IBillPaymentRepository BillPayment { get; }
    IBuildingRepository Building { get; }
    IContractRepository Contract { get; }
    IFundRepository Fund { get; }
    IOwnerRepository Owner { get; }
    IPaymentRepository Payment { get; }
    IPenaltyRepository Penalty { get; }
    IPersonRepository Person { get; }
    IUtilityBillRepository UtilityBill { get; }
    IUtilityRepository Utility { get; }
}
