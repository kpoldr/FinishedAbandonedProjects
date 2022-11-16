using System;
using System.Threading.Tasks;
using App.BLL.DTO.Identity;
using App.Contracts.BLL;
using App.DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Areas.Admin.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace Test.WebApp.UnitTests;

public class UnitTestAssociationController
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AssociationController _associationController;
    private readonly AppDbContext _ctx;
    private readonly Mock<IAppBLL> _bllMock;

    public UnitTestAssociationController(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);
        
        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        // set up logger - it is not mocked, we are not testing logging functionality
        using var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = logFactory.CreateLogger<AssociationController>();

        //set up SUT

        _bllMock = new Mock<IAppBLL>();

        _associationController = new AssociationController(_bllMock.Object, logger);
    }

    private async Task SeedDataAsync()
    {
        _bllMock.Setup(x => x.Association.FirstOrDefaultAsync(new Guid("11111111-1111-1111-1111-111111111112"), true))
            .ReturnsAsync(
                new App.BLL.DTO.Association()
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111112"),
                    Name = "TEST",
                    Email = "TEST@test.com",
                });
        
        _bllMock.Setup(x => x.AppUser.FirstOrDefaultAsync(new Guid("11111111-1111-1111-1111-111111111112"), true))
            .ReturnsAsync(
                new AppUser()
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111112"),
                    FirstName = "FirstTEST",
                    LastName =  "LastTEST",
                });
    }

    [Fact]
    public async Task Create_Action_Returns_NonNull_VM()
    {
        await SeedDataAsync();

        Assert.NotNull(_bllMock);

        var result = await _associationController.Create() as ViewResult;
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);
    }

    [Fact]
    public async Task Detail_Action_Returns_NonNull_VM()
    {
        await SeedDataAsync();

        Assert.NotNull(_bllMock);

        var result =
            (await _associationController.Details(new Guid("11111111-1111-1111-1111-111111111112"))) as ViewResult;
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);
    }
    
    [Fact]
    public async Task Edit_Action_Returns_NonNull_VM()
    {
        await SeedDataAsync();

        Assert.NotNull(_bllMock);

        var result = (await _associationController.Edit(new Guid("11111111-1111-1111-1111-111111111112"))) as ViewResult;
        
        
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);
    }
    
    [Fact]
    public async Task Delete_Action_Returns_NonNull_VM()
    {
        await SeedDataAsync();

        Assert.NotNull(_bllMock);

        var result =
            (await _associationController.Delete(new Guid("11111111-1111-1111-1111-111111111112"))) as ViewResult;
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);
    }
    
}