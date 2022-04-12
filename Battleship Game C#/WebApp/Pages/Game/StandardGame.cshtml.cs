using System;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class StandardGame : PageModel
    {
        
        public void OnGet()
        {
            
        }
        
        public RedirectToPageResult OnPostRandomGame()
        {
            return RedirectToPage("BattleShip", "RandomStandard", new {gameName = Request.Form["Name"]});
        }
        
        public RedirectToPageResult OnPostCustomGame()
        {
            return RedirectToPage("BattleShip", "CustomStandard", new {gameName = Request.Form["Name"]});
        }
        
    }
}