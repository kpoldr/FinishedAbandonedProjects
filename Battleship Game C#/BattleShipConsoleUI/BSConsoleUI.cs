using System;
using System.Collections.Generic;
using System.Text;
using BattleShipBrain;

namespace BattleShipConsoleUI
{
    public static class BSConsoleUI
    {
        public static void DrawBoardBomb(BSBrain brain, string info = "")
        {
            var gameConfig = brain.GameConfig;
            var (cX, cY) = brain.SelectedOption;

            var emptySpaces = "";

            for (int i = 0; i < gameConfig.BoardSizeY / 2; i++)
            {
                emptySpaces += "\t ";
            }


            Console.WriteLine();
            DrawHorizontal(gameConfig.BoardSizeY, true);

            for (int x = 0; x < gameConfig.BoardSizeX; x++)
            {
                for (var y = 0; y < gameConfig.BoardSizeY; y++)
                {
                    if (y == 0)
                    {
                        Console.Write($"{x + 1}");
                        Console.Write(x < 9 ? " " : "");
                    }

                    if (x == cX && y == cY)
                    {
                        CursorSquare(brain.GetBoard(1 - brain.CurrentPlayerInt)[(x, y)]);
                    }
                    else
                    {
                        var sunken = false;
                        var ship = brain.FindSelectedShip(true, x, y);

                        if (ship != null)
                        {
                            sunken = ship.IsShipSunk(brain.EnemyPlayer.Board);
                        }

                        // Console.Write($"| {brain.GetBoard(1 - brain.CurrentPlayer)[(x, y)].EnemyStrings()} ");
                        ColorSquare(brain.GetBoard(1 - brain.CurrentPlayerInt)[(x, y)], true, sunken);
                    }
                }

                Console.Write("|");

                Console.Write("    I  ");

                for (var y = 0; y < gameConfig.BoardSizeY; y++)
                {
                    if (y == 0)
                    {
                        Console.Write($"{x + 1}");
                        Console.Write(x < 9 ? " " : "");
                    }

                    // Console.Write($"| {brain.GetBoard(brain.CurrentPlayer)[(x, y)]} ");
                    var sunken = false;
                    var ship = brain.FindSelectedShip(false, x, y);

                    if (ship != null)
                    {
                        sunken = ship.IsShipSunk(brain.CurPlayer.Board);
                    }

                    // Console.Write($"| {brain.GetBoard(1 - brain.CurrentPlayer)[(x, y)].EnemyStrings()} ");
                    ColorSquare(brain.GetBoard(brain.CurrentPlayerInt)[(x, y)], false, sunken);
                    // ColorSquare(brain.GetBoard(brain.CurrentPlayer)[(x, y)]);
                }

                Console.Write("|");

                Console.WriteLine();
                DrawHorizontal(gameConfig.BoardSizeY);
            }

            Console.WriteLine();
            DisplayInfo(brain, info);
        }

        public static void DrawBoardShip(BSBrain brain, Ship ship)
        {
            var gameConfig = brain.GameConfig;
            var (cX, cY) = brain.SelectedOption;
            List<(int, int)> shipCords = brain.ShipCordsOnBoard(ship);
            Console.WriteLine();
            DrawHorizontalShip(gameConfig.BoardSizeY, true);

            for (int x = 0; x < gameConfig.BoardSizeX; x++)
            {
                for (var y = 0; y < gameConfig.BoardSizeY; y++)
                {
                    if (y == 0)
                    {
                        Console.Write($"{x + 1}");
                        Console.Write(x < 9 ? " " : "");
                    }

                    if (shipCords.Contains((x, y)))
                    {
                        ShipSpace(true, true);
                    }

                    else if (!brain.GetCurrentPlayer().AvailableCoordinates.Contains((x, y)))
                    {
                        if (brain.GetBoard(brain.CurrentPlayerInt)[(x, y)].ToString() != " ")
                        {
                            Console.Write($"| {brain.GetBoard(brain.CurrentPlayerInt)[(x, y)]} ");
                        }
                        else
                        {
                            ShipSpace();
                        }
                    }

                    else
                    {
                        Console.Write($"| {brain.GetBoard(brain.CurrentPlayerInt)[(x, y)]} ");
                    }
                }

                Console.Write("|");
                Console.WriteLine();
                DrawHorizontalShip(gameConfig.BoardSizeY);
            }

            ShipPlacementInfo(ship);
        }

        private static void DrawHorizontal(int size, bool top = false)
        {
            // Draw horizontal line for the board
            // If top is true, print column headers
            if (top)
            {
                Console.WriteLine();
                Console.WriteLine();

                Console.Write("  ");

                for (int i = 0; i < size; i++)
                {
                    Console.Write(i >= 10 ? " " : "  ");
                    Console.Write($"{i + 1} ");
                }

                Console.Write(size >= 10 ? "    I  " : "     I  ");

                Console.Write("  ");

                for (int i = 0; i < size; i++)
                {
                    Console.Write(i >= 10 ? " " : "  ");
                    Console.Write($"{i + 1} ");
                }


                Console.WriteLine();
            }

            Console.Write("  ");

            for (int i = 0; i < size; i++)
            {
                Console.Write("+---");
            }

            Console.Write("+");

            Console.Write("    I  ");
            Console.Write("  ");

            for (int i = 0; i < size; i++)
            {
                Console.Write("+---");
            }

            Console.Write("+");
            Console.WriteLine();
        }

        private static void DrawHorizontalShip(int size, bool top = false)
        {
            // Draw horizontal line for the board
            // If top is true, print column headers
            if (top)
            {
                Console.Write("  ");

                for (int i = 0; i < size; i++)
                {
                    Console.Write(i >= 10 ? " " : "  ");
                    Console.Write($"{i + 1} ");
                }

                Console.WriteLine();
            }

            Console.Write("  ");

            for (int i = 0; i < size; i++)
            {
                Console.Write("+---");
            }

            Console.Write("+");
            Console.WriteLine();
        }

        public static void ShipPlacementInfo(Ship ship)
        {
            Console.WriteLine();
            Console.WriteLine("  ===========================================");
            Console.WriteLine();
            Console.WriteLine($"  Ship name: {ship.Name}");
            Console.WriteLine($"  Ship Height: {ship.X}");
            Console.WriteLine($"  Ship Length: {ship.Y}");
            Console.WriteLine();
            Console.WriteLine("  Buttons ");
            Console.WriteLine("  -------------------");
            Console.WriteLine("  Arrows: Move Ship");
            Console.WriteLine("  Enter: Place Ship");
            Console.WriteLine("  Esc: Exit");
            Console.WriteLine();
            Console.WriteLine("  ===========================================");
        }

        public static void GameOver(BSBrain brain)
        {
            Console.WriteLine();
            Console.WriteLine("  ===========================================");
            Console.WriteLine();
            Console.WriteLine($"            !!!!!!!Player {brain.CurrentPlayerInt + 1} won!!!!!!!");
            Console.WriteLine();
            Console.WriteLine("  ===========================================");
        }


        public static void DisplayInfo(BSBrain brain, string info = "")
        {
            var enemy = 2 - brain.CurrentPlayerInt;
            Console.WriteLine();
            Console.WriteLine(
                $"         <- Enemy board (Player {brain.CurrentPlayerInt + 1}) | Your board (Player {enemy}) -> ");
            Console.WriteLine();
            Console.WriteLine("  =======================================================");
            Console.WriteLine($"  {info}");
            Console.WriteLine("  =======================================================");
            Console.WriteLine();
            Console.WriteLine($"  Placing cords: ({brain.SelectedIndexX + 1}, {brain.SelectedIndexY + 1})");
            Console.WriteLine();
            Console.WriteLine("  Buttons");
            Console.WriteLine("  ----------------------");
            Console.WriteLine("  Arrows: Move cursor");
            Console.WriteLine("  Enter: Place bomb");
            Console.WriteLine("  U: Undo move");
            Console.WriteLine("  R: Redo move");
            Console.WriteLine("  ----------------------");
            Console.WriteLine("  S: Save game");
            Console.WriteLine("  Esc: Exit");
            Console.WriteLine();
            Console.WriteLine("  =======================================================");
        }

        public static void CursorSquare(BoardSquareState symbol)
        {
            Console.Write("| ");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(symbol.EnemyStrings());
            Console.ResetColor();
            Console.Write(" ");
        }

        public static void ColorSquare(BoardSquareState symbol, bool enemy = false, bool sunken = false)
        {
            Console.Write("| ");
            // Console.Write($"| {brain.GetBoard(1 - brain.CurrentPlayer)[(x, y)].EnemyStrings()} ");
            if (symbol.IsShip && symbol.IsBomb)
            {
                if (enemy)
                {
                    Console.ForegroundColor = sunken ? ConsoleColor.Green : ConsoleColor.Yellow;

                    Console.Write(symbol.EnemyStrings());
                }
                else
                {
                    Console.ForegroundColor = sunken ? ConsoleColor.Red : ConsoleColor.Yellow;
                    Console.Write(symbol);
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                if (enemy)
                {
                    Console.Write(symbol.EnemyStrings());
                }
                else
                {
                    Console.Write(symbol);
                }
            }

            Console.Write(" ");
        }

        public static void ShipSpace(bool ekstra = false, bool selected = false)
        {
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.White;

            if (ekstra)
            {
                if (selected)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }

                Console.Write("*");
            }
            else
            {
                Console.Write("-");
            }

            Console.ResetColor();
            Console.Write(" ");
        }
    }
}