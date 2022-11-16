using App.Contracts.DAL;
using App.Contracts.DAL.Identity;
using App.DAL.EF.Mappers;
using App.DAL.EF.Mappers.Identity;
using App.DAL.EF.Repositories;
using App.DAL.EF.Repositories.Identity;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    private readonly AutoMapper.IMapper _mapper;
    public AppUOW(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
    {
        _mapper = mapper;
    }
    
    private IAppUserRepository? _appUsers;
    public virtual IAppUserRepository AppUser => _appUsers ??= new AppUserRepository(UOWDbContext, new AppUserMapper(_mapper));
    
    
    private IApartmentRepository? _apartments;
    public virtual IApartmentRepository Apartment => _apartments ??= new ApartmentRepository(UOWDbContext, new ApartmentMapper(_mapper));
    
    
    private IAssociationRepository? _associations;
    public virtual IAssociationRepository Association => _associations ??= new AssociationRepository(UOWDbContext, new AssociationMapper(_mapper));
    
    
    private IBillRepository? _bills;
    public virtual IBillRepository Bill => _bills ??= new BillRepository(UOWDbContext, new BillMapper(_mapper));
    
    
    private IBillPaymentRepository? _billPayments;
    public virtual IBillPaymentRepository BillPayment => _billPayments ??= new BillPaymentRepository(UOWDbContext, new BillPaymentMapper(_mapper));
    
    
    private IBuildingRepository? _buildings;
    public virtual IBuildingRepository Building => _buildings ??= new BuildingRepository(UOWDbContext, new BuildingMapper(_mapper));
    
    
    private IContractRepository? _contracts;
    public virtual IContractRepository Contract => _contracts ??= new ContractRepository(UOWDbContext, new ContractMapper(_mapper));
    
    
    private IFundRepository? _funds;
    public virtual IFundRepository Fund => _funds ??= new FundRepository(UOWDbContext, new FundMapper(_mapper));
    
    
    private IOwnerRepository? _owners;
    public virtual IOwnerRepository Owner => _owners ??= new OwnerRepository(UOWDbContext, new OwnerMapper(_mapper));
    
    
    private IPaymentRepository? _payments;
    public virtual IPaymentRepository Payment => _payments ??= new PaymentRepository(UOWDbContext, new PaymentMapper(_mapper));
    
    
    private IPenaltyRepository? _penalties;
    public virtual IPenaltyRepository Penalty => _penalties ??= new PenaltyRepository(UOWDbContext, new PenaltyMapper(_mapper));
    
    
    private IPersonRepository? _persons;
    public virtual IPersonRepository Person => _persons ??= new PersonRepository(UOWDbContext, new PersonMapper(_mapper));
    
    
    private IUtilityBillRepository? _utilityBills;
    public virtual IUtilityBillRepository UtilityBill => _utilityBills ??= new UtilityBillRepository(UOWDbContext, new UtilityBillMapper(_mapper));
    
    
    private IUtilityRepository? _utilities;
    public virtual IUtilityRepository Utility => _utilities ??= new UtilityRepository(UOWDbContext, new UtilityMapper(_mapper));
}