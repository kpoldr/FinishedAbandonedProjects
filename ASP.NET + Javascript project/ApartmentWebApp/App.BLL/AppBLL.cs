using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using AutoMapper;
using Base.BLL;
using App.BLL.Mappers;
using App.BLL.Mappers.Identity;
using App.BLL.Services.Identity;
using App.Contracts.BLL.Services.Identity;
using App.Contracts.DAL;

namespace App.BLL;

public class AppBLL : BaseBll<IAppUnitOfWork>, IAppBLL
{
    protected IAppUnitOfWork UnitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    
    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public override async Task<int> SaveChangesAsync()
    {
        return await UnitOfWork.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return UnitOfWork.SaveChanges();
    }

    private IAppUserService? _appuser;
    public IAppUserService AppUser => _appuser ??= new AppUserService(UnitOfWork.AppUser, new AppUserMapper(_mapper));
    
    private IApartmentService? _apartments;
    public IApartmentService Apartment => _apartments ??= new ApartmentService(UnitOfWork.Apartment, new ApartmentMapper(_mapper));

    private IAssociationService? _associations;
    public IAssociationService Association  => _associations ??= new AssociationService(UnitOfWork.Association, new AssociationMapper(_mapper));
    
    private IBillService? _bills;
    public IBillService Bill  => _bills ??= new BillService(UnitOfWork.Bill, new BillMapper(_mapper));
    
    private IBillPaymentService? _billpayments;
    public IBillPaymentService BillPayment  => _billpayments ??= new BillPaymentService(UnitOfWork.BillPayment, new BillPaymentMapper(_mapper));
    
    private IBuildingService? _buildings;
    public IBuildingService Building  => _buildings ??= new BuildingService(UnitOfWork.Building, new BuildingMapper(_mapper));
    
    private IContractService? _contracts;
    public IContractService Contract  => _contracts ??= new ContractService(UnitOfWork.Contract, new ContractMapper(_mapper));
    
    private IFundService? _funds;
    public IFundService Fund  => _funds ??= new FundService(UnitOfWork.Fund, new FundMapper(_mapper));
    
    private IOwnerService? _owners;
    public IOwnerService Owner  => _owners ??= new OwnerService(UnitOfWork.Owner, new OwnerMapper(_mapper));
    
    private IPaymentService? _payments;
    public IPaymentService Payment  => _payments ??= new PaymentService(UnitOfWork.Payment, new PaymentMapper(_mapper));
    
    private IPenaltyService? _penalties;
    public IPenaltyService Penalty  => _penalties ??= new PenaltyService(UnitOfWork.Penalty, new PenaltyMapper(_mapper));
    
    private IPersonService? _persons;
    public IPersonService Person  => _persons ??= new PersonService(UnitOfWork.Person, new PersonMapper(_mapper));
    
    private IUtilityBillService? _utilityBills;
    public IUtilityBillService UtilityBill => _utilityBills ??= new UtilityBillService(UnitOfWork.UtilityBill, new UtilityBillMapper(_mapper));
    
    private IUtilityService? _utilities;
    public IUtilityService Utility => _utilities ??= new UtilityService(UnitOfWork.Utility, new UtilityMapper(_mapper));
    
}