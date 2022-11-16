using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL;
using App.BLL.DTO;
using App.Contracts.BLL;
using App.DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Areas.Admin.Controllers;
using Xunit;
using Xunit.Abstractions;


namespace Testing.WebApp.Controllers;

public class UnitTestAssociationsService
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AssociationController _controller;
    private readonly AppDbContext _ctx;

    private readonly Mock<IAppBLL> _bllMock;
    // private readonly Mock<IAppBLL> _bllMock;

    public UnitTestAssociationsService(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        // set up mock db - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);

        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<AssociationController>();
        //
        _bllMock = new Mock<IAppBLL>();

        _bllMock.Setup(x => x.Association.GetAllAsync(true)).ReturnsAsync(new List<Association>()
        {
            new Association()
            {
                Name = "TEST",
                Email = "TEST@test.com"
            }
        });

        _controller = new AssociationController(_bllMock.Object, logger);
    }

    [Fact]
    public async Task IndexAction_ReturnsNonNullVm()
    {
        var result = (await _controller.Index()) as ViewResult;
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);
    }

    [Fact]
    public async Task IndexAction_ReturnsVmWithData()
    {
        // Arrange
        var testFooBar = new App.Domain.Association()
        {
            Name = "TEST",
            Email = "TEST@test.com"
        };
        
        _ctx.Associations.Add(testFooBar);
        await _ctx.SaveChangesAsync();

        // Act
        var result = (await _controller.Index()) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result!.Model);

        var model = result.Model as List<Association>;
        Assert.NotNull(model);

        Assert.NotEmpty(model);
        Assert.Single(model);
        Assert.Equal(testFooBar.Name, model!.First().Name);
    }
}