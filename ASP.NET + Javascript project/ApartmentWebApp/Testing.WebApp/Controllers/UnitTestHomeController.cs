// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using App.BLL;
// using App.BLL.DTO;
// using App.Contracts.BLL;
// using App.DAL.EF;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using WebApp.Areas.Admin.Controllers;
// using Moq;
// using WebApp.Controllers;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace Testing.WebApp.Controllers;
//
// public class UnitTestHomeController
// {
//     private readonly ITestOutputHelper _testOutputHelper;
//     private readonly HomeController _homeController;
//     private readonly AppDbContext _ctx;
//     
//     
//     public UnitTestHomeController(ITestOutputHelper testOutputHelper)
//     {
//         _testOutputHelper = testOutputHelper;
//
//         // set up mock db - inmemory
//         var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//         optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
//         _ctx = new AppDbContext(optionsBuilder.Options);
//
//         _ctx.Database.EnsureDeleted();
//         _ctx.Database.EnsureCreated();
//
//         using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
//         var logger = loggerFactory.CreateLogger<HomeController>();
//         
//         _bllMock = new Mock<IAppBLL>();
//         
//         _bllMock.Setup(x => x.Association.GetAllAsync(true)).ReturnsAsync(new List<Association>()
//         {
//             new Association()
//             {
//                 Name = "TEST",
//                 Email = "TEST@TEST.com"
//             }
//         });
//         
//         _homeController = new HomeController(logger, _ctx);
//
//     }
//     
//     [Fact]
//     public void IndexAction_ReturnsNoVm()
//     {
//         var result = _homeController.Index() as ViewResult;
//         _testOutputHelper.WriteLine(result?.ToString());
//         Assert.NotNull(result);
//         Assert.Null(result!.Model);
//         
//     }
//     
//     
// }