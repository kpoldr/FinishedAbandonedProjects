using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class Victory : PageModel
    {
        public void OnGet()
        {
            
        }

        public RedirectToPageResult OnPost()
        {
            
            return RedirectToPage("/Index");
            
        }
        
    }
}