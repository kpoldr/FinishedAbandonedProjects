using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace Test.WebApp;

public class UnitTestHomeController
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HomeController _homeController;
    private readonly AppDbContext _ctx;
    private readonly Mock<IAppBLL> _bllMock;

    public UnitTestHomeController(ITestOutputHelper testOutputHelper)
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
        var logger = logFactory.CreateLogger<HomeController>();

        //set up SUT
        
        
        _bllMock = new Mock<IAppBLL>();

        _homeController = new HomeController(logger, _bllMock.Object);
    }
    
    private async Task SeedDataAsync()
    {
        _bllMock.Setup(x => x.Association.GetAllAsync(true)).ReturnsAsync(new List<App.BLL.DTO.Association>()
        {
            new App.BLL.DTO.Association()
            {
                Name = "TEST",
                Email = "TEST@test.com"
            }
        });
        
        
        await _ctx.SaveChangesAsync();
    }
    
    private async Task SeedDataAsync(int count)
    {
        
        var associations = new List<App.BLL.DTO.Association>();
        
        for (int i = 0; i < count; i++)
        {
            associations.Add(new App.BLL.DTO.Association()
            {
                Name = $"TEST{i}",
                Email = $"TEST{i}@TEST.com"
            });

        }
        
        _bllMock.Setup(x => x.Association.GetAllAsync(true)).ReturnsAsync(new List<App.BLL.DTO.Association>(associations));

        await _ctx.SaveChangesAsync();
    }
    
    [Fact]
    public async Task TestAction_ReturnsVm()
    {
        _bllMock.Setup(x => x.Association.GetAllAsync(true)).ReturnsAsync(new List<App.BLL.DTO.Association>()
        {
            new App.BLL.DTO.Association()
            {
                Name = "TEST",
                Email = "TEST@test.com"
            }
        });
        // ACT
        var result = await _homeController.Test() as ViewResult;
        
        // _testOutputHelper.WriteLine(result?.ToString());
        // ASSERT
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);
        Assert.IsType<TestViewModel>(result.Model);
    }
    
    [Fact]
    public async Task TestAction_ReturnsVm_WithData()
    {
        await SeedDataAsync();

        // ACT
        var result = await _homeController.Test() as ViewResult;

        // ASSERT
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);
        Assert.IsType<TestViewModel>(result.Model);
        var vm = result.Model as TestViewModel;
        Assert.NotNull(vm!.Associations);
        Assert.Single(vm!.Associations!);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public async Task TestAction_ReturnsVm_WithDataCounts(int count)
    {
        await SeedDataAsync(count);

        // ACT
        var result = await _homeController.Test() as ViewResult;
        
        result.Should().NotBeNull();
        result!.Model.Should().NotBeNull();
        result.Model.Should().BeOfType<TestViewModel>();
        (result.Model as TestViewModel)!.Associations.Should().NotBeNull().And.HaveCount(count);
    }
    
}