using System;
using System.Collections.Generic;
using System.Linq;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApp.Pages.Game
{
    
    public class CustomGame : PageModel
    {
        
        [BindProperty]
        public Domain.GameConfig GameConfigModel { get; set; } = default!;
        public static List<Ship> GameShips { get; set; } = new List<Ship>();

        [TempData] 
        public string GameConfig { get; set; } = null!;

        public static bool ShipNameError { get; set; } = false;
        public static bool ShipXError { get; set; } = false;
        public static bool ShipYError { get; set; } = false;

        public static string ShipNameValue { get; set; } = "";
        public static string ShipXValue { get; set; } = "";
        public static string ShipYValue { get; set; } = "";
        
        public IActionResult OnGet(int id)
        {
            GameShips.RemoveAt(id);

            return Page();
        }
        
        public void OnGetNewCustomGame()
        {
            GameShips = new List<Ship>();
        }
        
        public IActionResult OnPostAddShip()
        {
            ShipNameValue = Request.Form["shipName"];
            ShipXValue = Request.Form["shipX"];
            ShipYValue = Request.Form["shipY"];
            
            
            if (Request.Form["shipName"] == "")
            {
                ShipNameError = true;
            }
            else
            {
                ShipNameError = false;
            }
            
            if (int.TryParse(Request.Form["shipX"], out int x))
            {
                ShipXError = false;
                
                if (int.TryParse(Request.Form["shipY"], out int y))
                {
                    ShipYError = false;

                    if (!ShipNameError)
                    {
                        GameShips.Add(new Ship(ShipNameValue, x, y));   
                        
                        ShipNameValue = "";
                        ShipXValue = "";
                        ShipYValue = "";
                    }
                }
                
            }
            else
            {
                ShipXError = true;
                if (!int.TryParse(Request.Form["shipY"], out var y))
                {
                    ShipYError = true;
                }
            }

            return Page();
        }
        public IActionResult OnPostCreateGame()
        {
            if (!ModelState.IsValid || GameShips.Count == 0)
            {
                return Page();
            }
            
            if (int.TryParse(Request.Form["gameRule"], out var gameRule))
            {
                var gameConfig = new GameConfig()
                {
                    Name = GameConfigModel.GameName,
                    BoardSizeX = GameConfigModel.BoardSizeX,
                    BoardSizeY = GameConfigModel.BoardSizeY,
                    EShipTouchRule = (EShipTouchRule) gameRule,
                    Ships = GameShips
                };
                
                GameConfig = JsonConvert.SerializeObject(gameConfig);
                return RedirectToPage("BattleShip", "CustomGame");
            }

            return Page();
        }

    }
    
    
    
}