// using App.Contracts.BLL;
// using App.DAL.EF;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Moq;
// using WebApp.Areas.Admin.Controllers;
// using Xunit.Abstractions;
//
// namespace Testing.WebApp.UnitTests;
//
// public class UnitTestsController
// {
//     private readonly AssociationController _testController;
//     private readonly ITestOutputHelper _testOutputHelper;
//     private readonly AppDbContext _ctx;
//     private readonly Mock<IAppBLL> _bllMock;
//
//     public TestControllerUnitTests(ITestOutputHelper testOutputHelper)
//     {
//         _testOutputHelper = testOutputHelper;
//             
//         // set up db context for testing - using InMemory db engine
//         var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
//         // provide new random database name here
//         // or parallel test methods will conflict each other
//         optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
//         _ctx = new AppDbContext(optionBuilder.Options);
//         _ctx.Database.EnsureDeleted();
//         _ctx.Database.EnsureCreated();
//
//         using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
//         var logger = loggerFactory.CreateLogger<AssociationController>();
//         
//     }
//
// }