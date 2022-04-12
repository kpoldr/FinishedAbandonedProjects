using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Game
{
    
    
    public class LoadGame : PageModel
    {

        public static List<string> GameNames = new List<string>();

        public static bool LoadFromDb = false;

        public void OnGet()
        {
            OnPostListGamesJson();
        }

        public void OnPostListGamesDb()
        {
            GameNames.Clear();
            LoadFromDb = true;
            using var db = new AppDbContext();
            
            var gameConfigsFromDb = db.GameConfigs;
            
            foreach (var gameConfig in gameConfigsFromDb)
            {
                GameNames.Add(gameConfig.GameName);
            }
        }
        
        public void OnPostListGamesJson()
        {
            GameNames.Clear();
            LoadFromDb = false;
            string[] saveFiles =
                Directory.GetFiles("" + System.IO.Path.DirectorySeparatorChar + "Saves", "*.json");

            for (int i = 0; i < saveFiles.Length; i++)
            {
                GameNames.Add(saveFiles[i].Split(System.IO.Path.DirectorySeparatorChar).Last().Split(".")
                    .First());    
            }
            
        }
        
    }
}