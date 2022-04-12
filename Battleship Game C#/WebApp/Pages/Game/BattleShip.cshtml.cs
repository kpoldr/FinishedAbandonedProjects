using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BattleShipBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.Pages.Game;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Player = Domain.Player;

namespace WebApp.Pages
{
    public class BattleShip : PageModel
    {
        public static string Info { get; set; } = "";

        public static BSBrain Brain { get; set; } = new BSBrain();

        public static int PlayerNumber { get; set; } = 0;
        public static int ShipIndex { get; set; } = 0;
        public static List<Ship> Ships { get; set; } = new List<Ship>();
        public static Ship TempShip { get; set; } = new Ship("default", 0, 0);
        public static List<(int, int)> ShipCords { get; set; } = new List<(int, int)>();

        public static bool Bombing = false;
        
        public static bool FirsTime = true;

        public static int ContainterSize = 5;
        
        public IActionResult OnGet(int x, int y, bool bombing)
        {
            
            if (Bombing == false)
            {
                Placing(x, y);
                return Page();
            }

            if (FirsTime)
            {
                Info = "Game Start";
                FirsTime = false;
                return Page();
            }
            
            if (!Brain.PlaceBombSquare(x, y))
            {
                Info = "Already placed a bomb there!";
                return Page();
            }

            Info =
                $"Player {Brain.CurrentPlayerInt + 1} placed a bomb at ({x + 1}, {y + 1}) ";

            var ship = Brain.FindSelectedShip(true, x, y);

            if (ship != null)
            {
                if (ship.IsShipSunk(Brain.EnemyPlayer.Board))
                {
                    Info += $"which sunks enemy's {ship.Name}!";
                }
                else
                {
                    Info += "which was a hit!";
                }
            }
            else
            {
                Info += "which was a miss!";
            }
            
            Brain.UpdatePlayer();
            
            if (Brain.CheckWinCondition())
            {
                return RedirectToPage("Victory");
            }
            
            return Page();
        }

        public void Placing(int cordX, int cordY)
        {
            while (cordX + TempShip.X > Brain.GameConfig.BoardSizeX)
            {
                cordX -= 1;

                if (cordX <= 0)
                {
                    break;
                }
            }

            while (cordY + TempShip.Y > Brain.GameConfig.BoardSizeY)
            {
                cordY -= 1;

                if (cordY <= 0)
                {
                    break;
                }
            }

            ShipCords = new List<(int, int)>();

            for (int x = cordX; x < cordX + TempShip.X; x++)
            {
                for (int y = cordY; y < cordY + TempShip.Y; y++)
                {
                    ShipCords.Add((x, y));
                }
            }
        }

        public void OnGetRandomStandard(string gameName)
        {
            Info = "";
            FirsTime = false;
            Brain = new BSBrain();
            Brain.GameConfig.Name = gameName;
            Brain.InitializeBoards();
            Brain.GenerateRandomBoards();
            Bombing = true;
        }
        
        public void OnGetCustomStandard(string gameName)
        {
            Info = "Place ships for player " + (Brain.CurrentPlayerInt + 1);

            Brain = new BSBrain();
            Brain.GameConfig.Name = gameName;
            FirsTime = true;
            ShipCords = new List<(int, int)>();
            Brain.InitializeBoards();
            PlayerNumber = 0;
            ShipIndex = 0;
            Ships = Brain.GameConfig.Ships;
            TempShip = new Ship(Ships[ShipIndex].Name, Ships[ShipIndex].X, Ships[ShipIndex].Y);
            Bombing = false;
        }
        
        public void OnGetCustomGame()
        {
            Info = "Place ships for player " + (Brain.CurrentPlayerInt + 1);
            var brain = new BSBrain();

            GameConfig gameConfig = new GameConfig();
            TempData.TryGetValue("GameConfig", out var jsonGameConfig);

            if (jsonGameConfig != null)
            {
                gameConfig = JsonConvert.DeserializeObject<GameConfig>((string) jsonGameConfig);
            }

            TempData.TryGetValue("GameConfig", out var jsonShips);
            

            if (gameConfig != null)
            {
                gameConfig.Ships.RemoveRange(0, 5);
                
                ShipCords = new List<(int, int)>();
                FirsTime = true;
                Brain = brain;
                PlayerNumber = 0;
                ShipIndex = 0;
                Ships = gameConfig.Ships;
                TempShip = new Ship(Ships[ShipIndex].Name, Ships[ShipIndex].X, Ships[ShipIndex].Y);
                Bombing = false;
                
                Brain.GameConfig = gameConfig;
                Brain.InitializeBoards();
                
            }
            else
            {
                RedirectToPage("/Error");
            }
        }

       

        public void OnPostRotateShip()
        {
            Ship ship = Ships[ShipIndex];

            TempShip.Rotate();
            OnGet(ShipCords[0].Item1, ShipCords[0].Item2, false);
        }

        public IActionResult OnPostPlaceShip()
        {
            Info = "Place ships for player " + (Brain.CurrentPlayerInt + 1);
            
            if (!Brain.PlaceShip(TempShip, ShipCords[0].Item1, ShipCords[0].Item2))
            {
                Info = "Can't place ship there";
                return Page();
            }
            

            ShipIndex += 1;

            if (ShipIndex + 1 > Ships.Count)
            {
                ShipIndex = 0;
                PlayerNumber += 1;
                Brain.UpdatePlayer();

                if (PlayerNumber > 1)
                {
                    ViewData["brain"] = Brain;
                    Bombing = true;
                }
            }

            TempShip = new Ship(Ships[ShipIndex].Name, Ships[ShipIndex].X, Ships[ShipIndex].Y);;

            OnGet(ShipCords[0].Item1, ShipCords[0].Item2, false);
            
            return Page();
        }


        public void OnPostRedoMove()
        {
            if (Brain.RedoMove())
            {
                Info = "Redid the move!";
            }
        }

        public void OnPostUndoMove()
        {
            if (Brain.UndoMove())
            {
                Info = "Undid the previous move!";
            }
        }

        public void OnPostSaveGameDb()
        {
            Brain.SaveGameToDb(true);
        }

        public void OnPostSaveGameJson()
        {
            Info = "Saved a game to " + System.IO.Path.DirectorySeparatorChar + "Saves";
            
            var conf = Brain.GetBrainJson();

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var fileNameStandardConfig = "" + System.IO.Path.DirectorySeparatorChar + "Saves" +
                                         System.IO.Path.DirectorySeparatorChar + $"{Brain.GameConfig.Name}.json";

            var confJsonStr = JsonSerializer.Serialize(conf, jsonOptions);

            if (!System.IO.File.Exists(fileNameStandardConfig))
            {
                System.IO.File.WriteAllText(fileNameStandardConfig, conf);
            }
        }

        public void OnGetLoadGameDb(int id)
        {
            Info = "Saved a game to the database with the name " + Brain.GameConfig.Name;
            Info = "";
            Brain.LoadGameFromDb(id);
            Bombing = true;
        }

        public void OnGetLoadGameJson(int id)
        {
            Info = "";
            string[] saveFiles =
                Directory.GetFiles("" + System.IO.Path.DirectorySeparatorChar + "Saves", "*.json");
            Brain.RestoreBrainFromJson(System.IO.File.ReadAllText(saveFiles[id]));
            Bombing = true;
        }


        public RedirectToPageResult OnPostExitGame()
        {
            return RedirectToPage("/Index");
        }
    }
}