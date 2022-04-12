using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
    
    public RedirectToPageResult OnPostStandardGame()
    {
        return RedirectToPage("Game/StandardGame");
    }
        
    public RedirectToPageResult OnPostCustomGame()
    {
        return RedirectToPage("Game/CustomGame", "NewCustomGame");
    }

}