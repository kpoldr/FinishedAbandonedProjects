using System.Diagnostics;
using App.Contracts.BLL;
using App.DAL.EF;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers;

public class  HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // private readonly AppDbContext _context;
    
    private readonly IAppBLL _bll;
    
    public HomeController(ILogger<HomeController> logger, IAppBLL bll)
    {
        _logger = logger;
        _bll = bll;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Test()
    {
        var vm = new TestViewModel
        {
            Associations = await _bll.Association.GetAllAsync()
        };
        
        
        return View(vm);
    }

    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            }
        );
        return LocalRedirect(returnUrl);
    }

}