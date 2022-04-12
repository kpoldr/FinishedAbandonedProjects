using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BattleShipBrain;
using BattleShipConsoleUI;
using MenuSystem;
using BattleShipConfigMenu;
using GameConfig = BattleShipBrain.GameConfig;


namespace BattleShipConsoleApp
{
    class Program
    {
        private static string _basePath = "D:\\";
        public static BSBrain _saveBrain = new BSBrain();


        static void Main(string[] args)
        {
            _basePath = args.Length == 1 ? args[0] : System.IO.Directory.GetCurrentDirectory();

            Console.Clear();

            var mainMenu = new Menu("Battleships MainMenu", EMenuLevel.Root);
            mainMenu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("New game", SubmenuNewGame, null, null),
                new MenuItem("Load game", LoadGameMenu, null, null)
            });

            mainMenu.Run();
        }

        public static void SubmenuNewGame()
        {
            var menu = new Menu("New Game", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("Standard game (10x10)", SubmenuRandom, null, null),
                new MenuItem("Custom game (?x?)", CustomGame, null, null)
            });
            menu.Run();
        }

        public static void SubmenuRandom()
        {
            var menu = new Menu("Ship placement", EMenuLevel.Else);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("Choose ship placement", StandardGame, null, null),
                new MenuItem("Random ship placement", RandomGame, null, null)
            });
            menu.Run();
        }

        public static void SaveGameMenu()
        {
            var menu = new Menu("Save game", EMenuLevel.Else);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("Save on a database", SaveGameToDb, null, null),
                new MenuItem("Save as json ", SaveGameAsJson, null, null)
            });

            menu.Run(true);
        }

        public static void LoadGameMenu()
        {
            var menu = new Menu("Load game", EMenuLevel.Else);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("Load from database", LoadGamefromDb, null, null),
                new MenuItem("Load from a local json ", LoadGameFromJson, null, null)
            });

            menu.Run(true);
        }


        public static void StandardGame()
        {
            CreateGame(new GameConfig());
        }

        public static void RandomGame()
        {
            CreateGame(new GameConfig(), true);
        }

        public static void CustomGame()
        {
            var (gameConfig, createGame) = BSConfigMenu.StartConfigMenu();

            if (createGame)
            {
                CreateGame(gameConfig);
            }
        }

        public static void CreateGame(GameConfig gameConfig, bool random = false)
        {
            var brain = new BSBrain();
            brain.GameConfig = gameConfig;
            brain.InitializeBoards();
            Console.Clear();
            GameLoop(brain, true, random);
        }
        

        public static void LoadGameFromJson()
        {
            Console.Clear();
            string[] saveFiles =
                Directory.GetFiles(_basePath + System.IO.Path.DirectorySeparatorChar + "Saves", "*.json");

            Dictionary<int, string> simplifiedFiles = new Dictionary<int, string>();


            for (int i = 0; i < saveFiles.Length; i++)
            {
                simplifiedFiles.Add(i, saveFiles[i]);
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Load Game");
                Console.WriteLine("---------------------");
                Console.WriteLine();

                for (int i = 0; i < saveFiles.Length; i++)
                {
                    string saveFile = saveFiles[i].Split(System.IO.Path.DirectorySeparatorChar).Last().Split(".")
                        .First();
                    Console.WriteLine($"  {i + 1}) {saveFile}");
                }

                Console.WriteLine();
                Console.Write("Load file:");

                var input = Console.ReadLine()?.Trim();

                Console.Clear();

                if (int.TryParse(input, out var fileIndex))
                {
                    if (simplifiedFiles.ContainsKey(fileIndex - 1))
                    {
                        string json = File.ReadAllText(simplifiedFiles[fileIndex - 1]);
                        BSBrain brain = new BSBrain();

                        brain.RestoreBrainFromJson(json);
                        GameLoop(brain, false);
                        break;
                    }


                    Console.WriteLine($"Not one of the options: ({fileIndex}) ");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine($"Invalid input: ({input}) ");
                Console.WriteLine();
            }
        }

        public static void LoadGamefromDb()
        {
            var brain = new BSBrain();
            Console.Clear();
            brain.LoadGameFromDb(brain.LoadGameDbMenu());
            GameLoop(brain, false);
        }


        public static void SaveGameAsJson()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Save File");
            Console.WriteLine("Overrides if file already exist");
            Console.WriteLine("---------------------");
            Console.Write("Save as:");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                input = "Untitled";
            }
            
            _saveBrain.GameConfig.Name = input;
            var conf = _saveBrain.GetBrainJson();
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var fileNameStandardConfig = _basePath + System.IO.Path.DirectorySeparatorChar + "Saves" +
                                         System.IO.Path.DirectorySeparatorChar + $"{input}.json";

            var confJsonStr = JsonSerializer.Serialize(conf, jsonOptions);

            if (!System.IO.File.Exists(fileNameStandardConfig))
            {
                Console.WriteLine("Saving default config!");
                System.IO.File.WriteAllText(fileNameStandardConfig, conf);
            }
        }

        public static void SaveGameToDb()
        {
            _saveBrain.SaveGameToDb();
        }


        public static bool ShipPlacement(BSBrain brain)
        {
            var runEnd = false;
            for (int i = 0; i < 2; i++)
            {
                var ships = brain.GameConfig.Ships;

                foreach (var ship in ships)
                {
                    while (true)
                    {
                        brain.SelectedShipSize = (ship.X, ship.Y);

                        bool placeShip = false;

                        Console.WriteLine();
                        BSConsoleUI.DrawBoardShip(brain, ship);

                        var keyInfo = Console.ReadKey();

                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.UpArrow:
                                brain.MoveUp();
                                break;
                            case ConsoleKey.DownArrow:
                                brain.MoveDown();
                                break;
                            case ConsoleKey.LeftArrow:
                                brain.MoveLeft();
                                break;
                            case ConsoleKey.RightArrow:
                                brain.MoveRight();
                                break;

                            case ConsoleKey.Escape:
                                runEnd = true;
                                break;

                            case ConsoleKey.R:
                                brain.RotateShip(ship);
                                break;

                            case ConsoleKey.Enter:
                                placeShip = true;
                                break;
                        }

                        Console.Clear();

                        if (placeShip)
                        {
                            if (brain.PlaceShip(ship))
                            {
                                break;
                            }
                        }
                        else if (runEnd)
                        {
                            return false;
                        }
                    }
                }

                brain.UpdatePlayer();
            }

            brain.UpdatePlayer();

            return true;
        }


        public static void GameLoop(BSBrain brain, bool newGame = true, bool randomShips = false)
        {
            if (newGame)
            {
                if (randomShips)
                {
                    brain.GenerateRandomBoards();
                }

                else
                {
                    if (!ShipPlacement(brain))
                    {
                        return;
                    }

                    ;
                }
            }

            // Ship placement and bombing use the same cursor. This offsets it for bombing.
            brain.SelectedShipSize = (1, 1);

            var info = "";

            while (true)
            {
                bool placeBomb = false;
                bool undo = false;
                bool redo = false;

                if (brain.CheckWinCondition())
                {
                    Console.Clear();
                    BSConsoleUI.GameOver(brain);
                    Console.ReadLine();
                    break;
                }


                Console.WriteLine();

                BSConsoleUI.DrawBoardBomb(brain, info);

                var runEnd = false;
                var keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        brain.MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        brain.MoveDown();
                        break;
                    case ConsoleKey.LeftArrow:
                        brain.MoveLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        brain.MoveRight();
                        break;
                    case ConsoleKey.U:
                        undo = true;
                        break;
                    case ConsoleKey.R:
                        redo = true;
                        break;
                    case ConsoleKey.Enter:
                        placeBomb = true;
                        break;
                    case ConsoleKey.Escape:
                        runEnd = true;
                        break;
                    case ConsoleKey.S:
                        _saveBrain = brain;
                        SaveGameMenu();
                        break;
                }


                Console.Clear();
                if (placeBomb)
                {
                    if (!brain.PlaceBombSquare())
                    {
                        info = "Already placed a bomb there!";
                        continue;
                    }

                    info =
                        $"Player {brain.CurrentPlayerInt + 1} placed a bomb at ({brain.SelectedIndexY + 1}, {brain.SelectedIndexX + 1}) ";

                    var ship = brain.FindSelectedShip(true);

                    if (ship != null)
                    {
                        if (ship.IsShipSunk(brain.EnemyPlayer.Board))
                        {
                            info += $"which sunks enemy's {ship.Name}!";
                        }
                        else
                        {
                            info += "which was a hit!";
                        }
                    }
                    else
                    {
                        info += "which was a miss!";
                    }

                    brain.UpdatePlayer();
                }

                else if (undo)
                {
                    info = brain.UndoMove() ? "Undo the previous move!" : "There's nothing to undo!";
                }
                else if (redo)
                {
                    info = brain.RedoMove() ? "Redid the previous move!" : "There's nothing to redo!";
                }

                else if (runEnd)
                {
                    break;
                }
            }
        }
    }
}