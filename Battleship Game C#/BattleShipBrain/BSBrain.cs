using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;

namespace BattleShipBrain
{
    public class BSBrain
    {
        public Player Player1 = new Player();
        public Player Player2 = new Player();

        public Player CurPlayer;
        public Player EnemyPlayer;
        public readonly Stack<(int, int)> _bombingHistory = new Stack<(int, int)>();
        public readonly Stack<(int, int)> _redoHistory = new Stack<(int, int)>();

        private readonly Random _rndMain = new Random();
        private readonly Random _oneOrTwo = new Random();

        public GameConfig GameConfig { get; set; } = new GameConfig();
        public int SelectedIndexY { get; private set; }
        public int SelectedIndexX { get; private set; }
        public int CurrentPlayerInt { get; set; } = 0;

        public (int, int) SelectedShipSize { get; set; } = (0, 0);

        public void MoveUp() => SelectedIndexX = Math.Max(SelectedIndexX - 1, 0);

        public void MoveDown() => SelectedIndexX =
            Math.Min(SelectedIndexX + 1, GameConfig.BoardSizeX - 1 - SelectedShipSize.Item1 + 1);

        public void MoveLeft() => SelectedIndexY = Math.Max(SelectedIndexY - 1, 0);

        public void MoveRight() => SelectedIndexY =
            Math.Min(SelectedIndexY + 1, GameConfig.BoardSizeY - 1 - SelectedShipSize.Item2 + 1);

        public (int, int) SelectedOption => (SelectedIndexX, SelectedIndexY);

        public BSBrain()
        {
            Player1.Board = new Dictionary<(int, int), BoardSquareState>();
            Player2.Board = new Dictionary<(int, int), BoardSquareState>();
            CurPlayer = Player1;
            EnemyPlayer = Player2;
        }

        public void InitializeBoards()
        {
            var availableCoordinates = new List<(int, int)>();

            for (var x = 0; x < GameConfig.BoardSizeX; x++)
            {
                for (var y = 0; y < GameConfig.BoardSizeY; y++)
                {
                    availableCoordinates.Add((x, y));
                    Player1.Board.Add((x, y), new BoardSquareState
                    {
                        IsBomb = false,
                        IsShip = false
                    });

                    Player2.Board.Add((x, y), new BoardSquareState
                    {
                        IsBomb = false,
                        IsShip = false
                    });
                }
            }

            Player1.AvailableCoordinates = new List<(int, int)>(availableCoordinates);
            Player2.AvailableCoordinates = new List<(int, int)>(availableCoordinates);
        }

        private void RestartIndexes()
        {
            SelectedIndexX = 0;
            SelectedIndexY = 0;
        }

        public void RotateShip(Ship ship)
        {
            if (SelectedIndexX + ship.Y <= GameConfig.BoardSizeX &&
                SelectedIndexY + ship.X <= GameConfig.BoardSizeY)
            {
                ship.Rotate();
            }
        }

        public bool CheckWinCondition()
        {
            int sunkenShips = 0;

            foreach (var ship in CurPlayer.Ships)
            {
                if (ship.IsShipSunk(CurPlayer.Board))
                {
                    sunkenShips++;
                }
            }

            return CurPlayer.Ships.Count == sunkenShips;
        }


        public void UpdatePlayer()
        {
            CurrentPlayerInt = 1 - CurrentPlayerInt;
            CurPlayer = CurPlayer == Player1 ? Player2 : Player1;
            EnemyPlayer = EnemyPlayer == Player2 ? Player1 : Player2;
        }

        public Dictionary<(int, int), BoardSquareState> GetBoard(int playerNo)
        {
            return playerNo == 0 ? Player1.Board : Player2.Board;
        }

        public bool PlaceShip(Ship templateShip, int toPlaceX = -1, int toPlaceY = -1)
        {
            if (toPlaceX == -1 && toPlaceY == -1)
            {
                toPlaceX = SelectedIndexX;
                toPlaceY = SelectedIndexY;
            }

            if (CheckShipPlacement(templateShip, toPlaceX, toPlaceY))
            {
                var ship = new Ship(templateShip.Name, templateShip.X, templateShip.Y);

                for (int x = toPlaceX; x < toPlaceX + templateShip.X; x++)
                {
                    for (int y = toPlaceY; y < toPlaceY + templateShip.Y; y++)
                    {
                        ship.SetCoordinate(x, y);
                        PlaceShipSquare((x, y));
                    }
                }

                CurPlayer.Ships.Add(ship);

                RemoveAvailableCoordinates(templateShip, toPlaceX, toPlaceY);

                RestartIndexes();

                return true;
            }

            return false;
        }

        public void GenerateRandomBoards()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var ship in GameConfig.Ships)
                {
                    while (true)
                    {
                        int r = _rndMain.Next(CurPlayer.AvailableCoordinates.Count);

                        int toRotate = _oneOrTwo.Next(1, 2);

                        if (toRotate == 1)
                        {
                            ship.Rotate();
                        }

                        var (x, y) = CurPlayer.AvailableCoordinates[r];

                        if (PlaceShip(ship, x, y))
                        {
                            break;
                        }

                        ship.Rotate();
                        if (PlaceShip(ship, x, y))
                        {
                            break;
                        }
                    }
                }

                UpdatePlayer();
            }

            UpdatePlayer();
        }


        public void RemoveAvailableCoordinates(Ship ship, int toPlaceX, int toPlaceY)
        {
            int buffer = 0;

            if (GameConfig.EShipTouchRule is EShipTouchRule.NoTouch or EShipTouchRule.CornerTouch)
            {
                buffer = 1;
            }

            for (int x = toPlaceX - buffer; x < toPlaceX + ship.X + buffer; x++)
            {
                for (int y = toPlaceY - buffer; y < toPlaceY + ship.Y + buffer; y++)
                {
                    if (GameConfig.EShipTouchRule == EShipTouchRule.CornerTouch)
                    {
                        if (x == toPlaceX - buffer && y == toPlaceY - buffer ||
                            x == toPlaceX + ship.X && y == toPlaceY - buffer ||
                            x == toPlaceX - buffer && y == toPlaceY + ship.Y ||
                            x == toPlaceX + ship.X && y == toPlaceY + ship.Y)
                        {
                            continue;
                        }
                    }

                    CurPlayer.AvailableCoordinates.Remove((x, y));
                }
            }
        }

        public List<(int, int)> ShipCordsOnBoard(Ship ship)
        {
            if (SelectedIndexX + ship.X <= GameConfig.BoardSizeX &&
                SelectedIndexY + ship.Y <= GameConfig.BoardSizeY)
            {
                List<(int, int)> toPlace = new List<(int, int)>();

                for (int x = SelectedIndexX; x < SelectedIndexX + ship.X; x++)
                {
                    for (int y = SelectedIndexY; y < SelectedIndexY + ship.Y; y++)
                    {
                        toPlace.Add((x, y));
                    }
                }

                return toPlace;
            }

            return new List<(int, int)>(new[] {(-1, -1)});
        }


        public bool CheckShipPlacement(Ship ship, int toPlaceX = -1, int toPlaceY = -1)
        {
            if (toPlaceX == -1 && toPlaceY == -1)
            {
                toPlaceX = SelectedIndexX;
                toPlaceY = SelectedIndexY;
            }


            if (toPlaceX + ship.X <= GameConfig.BoardSizeX &&
                toPlaceY + ship.Y <= GameConfig.BoardSizeY)
            {
                var toPlace = new List<(int, int)>();

                for (int x = toPlaceX; x < toPlaceX + ship.X; x++)
                {
                    for (int y = toPlaceY; y < toPlaceY + ship.Y; y++)
                    {
                        toPlace.Add((x, y));
                    }
                }

                foreach (var CordsToCheck in toPlace)
                {
                    if (!CurPlayer.AvailableCoordinates.Contains(CordsToCheck))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }


        public Player GetCurrentPlayer()
        {
            return CurPlayer;
        }

        public Ship? FindSelectedShip(bool enemy = false, int x = -1, int y = -1)
        {
            var player = CurPlayer;

            if (x == -1 && y == -1)
            {
                x = SelectedIndexX;
                y = SelectedIndexY;
            }

            if (enemy)
            {
                player = EnemyPlayer;
            }

            if (player.Board[(x, y)].IsShip)
            {
                foreach (var ship in player.Ships)
                {
                    if (ship._coordinates.Contains(new Tuple<int, int>(x, y)))
                    {
                        return ship;
                    }
                }
            }

            return null;
        }


        public bool PlaceBombSquare(int x = -1, int y = -1)
        {
            if (x == -1 && y == -1)
            {
                x = SelectedIndexX;
                y = SelectedIndexY;
            }
            
            
            if (EnemyPlayer.Board[(x, y)].IsBomb)
            {
                return false;
            }

            EnemyPlayer.Board[(x, y)].IsBomb = true;

            _bombingHistory.Push((x, y));
            _redoHistory.Clear();

            return true;
        }

        public bool UndoMove()
        {
            if (_bombingHistory.Count != 0)
            {
                var cords = _bombingHistory.Pop();

                CurPlayer.AvailableCoordinates.Add(cords);
                CurPlayer.Board[cords].IsBomb = false;

                UpdatePlayer();

                _redoHistory.Push(cords);

                return true;
            }

            return false;
        }

        public bool RedoMove()
        {
            if (_redoHistory.Count != 0)
            {
                var cords = _redoHistory.Pop();

                EnemyPlayer.AvailableCoordinates.Remove(cords);
                EnemyPlayer.Board[cords].IsBomb = true;

                UpdatePlayer();

                _bombingHistory.Push(cords);

                return true;
            }

            return false;
        }


        private void PlaceShipSquare((int, int) coordinates)
        {
            CurPlayer.Board[coordinates].IsShip = true;
        }

        // json converter doesn't recognize a simple (int, int) tuple, so this converts it 
        private static List<Tuple<int, int>> TupleToTupleConverter(List<(int, int)> tupleList)
        {
            var newTupleList = new List<Tuple<int, int>>();
            foreach (var (x, y) in tupleList)
            {
                newTupleList.Add(new Tuple<int, int>(x, y));
            }

            return newTupleList;
        }

        private static List<(int, int)> TupleToTupleConverter(List<Tuple<int, int>> tupleList)
        {
            var newTupleList = new List<(int x, int y)>();

            foreach (var (x, y) in tupleList)
            {
                newTupleList.Add((x, y));
            }

            return newTupleList;
        }


        public Dictionary<string, BoardSquareState> BoardToStringDictionary(
            Player player)

        {
            Dictionary<string, BoardSquareState> stringKeyBoard = new Dictionary<string, BoardSquareState>();

            for (int i = 0; i < GameConfig.BoardSizeX; i++)
            {
                for (int j = 0; j < GameConfig.BoardSizeY; j++)
                {
                    stringKeyBoard.Add($"{i}, {j}", player.Board[(i, j)]);
                }
            }

            return stringKeyBoard;
        }

        public Dictionary<(int, int), BoardSquareState> BoardToTupleDictionary(
            Dictionary<string, BoardSquareState> player)

        {
            Dictionary<(int, int), BoardSquareState> playerBoard = new Dictionary<(int, int), BoardSquareState>();

            Dictionary<string, BoardSquareState>.KeyCollection stringCords = player.Keys;

            foreach (var stringCord in stringCords)
            {
                string[] cords = stringCord.Split(",");

                var x = int.Parse(cords[0]);
                var y = int.Parse(cords[1]);

                playerBoard.Add((x, y), player[stringCord]);
            }

            return playerBoard;
        }

        public string GetBrainJson()
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var dto = new SaveGameDTO();
            dto.CurrentPlayerNo = CurrentPlayerInt;
            dto.GameConfig = GameConfig;

            for (int i = 0; i < 2; i++)
            {
                SaveGameDTO.ProxyPlayer proxyPlayer = new SaveGameDTO.ProxyPlayer();
                proxyPlayer.Ships = CurPlayer.Ships;
                proxyPlayer.Board = BoardToStringDictionary(CurPlayer);
                proxyPlayer.AvailableCoordinates = TupleToTupleConverter(CurPlayer.AvailableCoordinates);
                dto.Players.Add(proxyPlayer);
                UpdatePlayer();
            }

            var jsonStr = JsonSerializer.Serialize(dto, jsonOptions);
            return jsonStr;
        }

        public void RestoreBrainFromJson(string json)
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                AllowTrailingCommas = true,
            };

            var dto = JsonSerializer.Deserialize<SaveGameDTO>(json, jsonOptions);

            if (dto != null)
            {
                CurrentPlayerInt = dto.CurrentPlayerNo;
                GameConfig = dto.GameConfig;

                var i = 0;

                if (CurrentPlayerInt == 1)
                {
                    i = 1;
                }

                Player1.Board = BoardToTupleDictionary(dto.Players[1 - i].Board);
                Player1.Ships = dto.Players[1 - i].Ships;
                Player1.AvailableCoordinates = TupleToTupleConverter(dto.Players[1 - i].AvailableCoordinates);

                Player2.Board = BoardToTupleDictionary(dto.Players[i].Board);
                Player2.Ships = dto.Players[i].Ships;
                Player2.AvailableCoordinates = TupleToTupleConverter(dto.Players[i].AvailableCoordinates);

                CurPlayer = CurrentPlayerInt == 0 ? Player1 : Player2;
                EnemyPlayer = CurrentPlayerInt == 1 ? Player1 : Player2;
            }
        }

        public void LoadGameFromDb(int gameToLoad)
        {
            using var db = new AppDbContext();
            

            var dbGameInfo = db.GameConfigs.Include(p => p.Players)
                .Where(p => p.GameConfigId == gameToLoad);
            
            var gameInfo = dbGameInfo.FirstOrDefault();
            
            if (gameInfo == null)
            {
                Console.WriteLine("Corrupted info");
                return;
            }
            
            GameConfig.BoardSizeX = gameInfo.BoardSizeX;
            GameConfig.BoardSizeY = gameInfo.BoardSizeY;
            GameConfig.EShipTouchRule = (EShipTouchRule) gameInfo.GameRule;
            
            CurrentPlayerInt = gameInfo.Players.FirstOrDefault()!.PlayerNumber;

            var dbPlayerInfo = db.Players.Include(p => p.Ships)
                .Include(p => p.BoardSquares).Where(p => p.GameConfigId == gameToLoad);


            foreach (var playerInfo in dbPlayerInfo)
            {
                
                {
                    if (playerInfo.Ships != null)
                        foreach (var dbShip in playerInfo.Ships)
                        {
                            var ship = new Ship(dbShip.ShipName, dbShip.ShipSizeX, dbShip.ShipSizeY);

                            foreach (var boardSquare in dbShip.ShipSquares!)
                            {
                                ship._coordinates.Add(new Tuple<int, int>(boardSquare.CordX, boardSquare.CordY));
                            }

                            if (playerInfo.PlayerNumber == 0)
                            {
                                Player1.Ships.Add(ship);    
                            }
                            else
                            {
                                Player2.Ships.Add(ship);
                            }
                            
                        }
                    
                    var board = new Dictionary<(int, int), BoardSquareState>();
                    
                    foreach (var dbBoardSquare in playerInfo.BoardSquares!)
                    {
                        var boardSquareState = new BoardSquareState();
                        
                        boardSquareState.IsBomb = dbBoardSquare.IsBomb;
                        boardSquareState.IsShip = dbBoardSquare.IsShip;
                        
                        board.Add((dbBoardSquare.CordX, dbBoardSquare.CordY), boardSquareState);

                        if (playerInfo.PlayerNumber == 0)
                        {
                            Player1.Board = board;
                        }
                        else
                        {
                            Player2.Board = board;
                        }
                    }
                }
            }

            if (CurrentPlayerInt == 0)
            {
                CurPlayer = Player1;
                EnemyPlayer = Player2;
            }
            else
            {
                CurPlayer = Player2;
                EnemyPlayer = Player1;
            }

        }
        
        public void SaveGameToDb(bool useGameName = false)
        {

            string? input = null;
            
            if (!useGameName)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Save File");
                Console.WriteLine("Overrides if file already exist");
                Console.WriteLine("---------------------");
                Console.Write("Save as:");
            
                input = Console.ReadLine()?.Trim() ?? "Untitled";
            }

            using var db = new AppDbContext();

            if (input == null)
            {
                input = GameConfig.Name;
            }

            var gameConfig = new Domain.GameConfig
            {
                GameName = input, 
                BoardSizeX = GameConfig.BoardSizeX,
                BoardSizeY = GameConfig.BoardSizeY,
                GameRule = (int) GameConfig.EShipTouchRule,
                NumberOfShips = GameConfig.Ships.Count,
                Players = new List<Domain.Player>()
            };


            for (int i = 0; i < 2; i++)
            {
                var playerBoard = new Dictionary<(int, int), BoardSquareState>(CurPlayer.Board);

                var dbPlayer = new Domain.Player
                {
                    PlayerNumber = CurrentPlayerInt,
                    Ships = new List<Domain.Ship>(),
                    BoardSquares = new List<Domain.BoardSquare>(),
                    GameConfig = gameConfig
                };
                
                foreach (var ship in CurPlayer.Ships)
                {
                    var dbShip = new Domain.Ship
                    {
                        ShipName = ship.Name,
                        ShipSizeX = ship.X,
                        ShipSizeY = ship.Y,
                        ShipSquares = new List<Domain.BoardSquare>(),
                        Player = dbPlayer
                    };

                    foreach (var (cordX, cordY) in ship._coordinates)
                    {
                        var boardSquareShip = new Domain.BoardSquare
                        {
                            CordX = cordX,
                            CordY = cordY,
                            IsShip = true,
                            IsBomb = playerBoard[(cordX, cordY)].IsBomb,
                            Ship = dbShip,
                            Player = dbPlayer
                        };

                        playerBoard.Remove((cordX, cordY));

                        db.BoardSquares.Add(boardSquareShip);
                        dbPlayer.BoardSquares.Add(boardSquareShip);
                        dbShip.ShipSquares.Add(boardSquareShip);
                    }

                    dbPlayer.Ships.Add(dbShip);
                    db.Ships.Add(dbShip);
                }

                foreach (var ((cordX, cordY), square) in playerBoard)
                {
                    var boardSquare = new Domain.BoardSquare
                    {
                        CordX = cordX,
                        CordY = cordY,
                        IsShip = false,
                        IsBomb = square.IsBomb,
                        Ship = null,
                        PlayerId = dbPlayer.PlayerId,
                        Player = dbPlayer
                    };

                    dbPlayer.BoardSquares.Add(boardSquare);
                    db.BoardSquares.Add(boardSquare);
                }

                db.Players.Add(dbPlayer);
                UpdatePlayer();
            }

            db.GameConfigs.Add(gameConfig);
            db.SaveChanges();
        }
        
        public int LoadGameDbMenu() {
            
            Console.Clear();
            
            using var db = new AppDbContext();

            var gameConfigsFromDb = db.GameConfigs;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Load Game");
                Console.WriteLine("---------------------");
                Console.WriteLine();

                var options = new ArrayList();


                foreach (var gameConfig in gameConfigsFromDb)
                {

                    Console.WriteLine($"{gameConfig.GameConfigId}) {gameConfig.GameName}");
                    options.Add(gameConfig.GameConfigId);
                }

                Console.WriteLine();
                Console.Write("Load file:");

                var input = Console.ReadLine()?.Trim();

                Console.Clear();

                if (int.TryParse(input, out var gameIndex))
                {
                    if (options.Contains(gameIndex))
                    {
                        return gameIndex;
                    }


                    Console.WriteLine($"Not one of the options: ({gameIndex}) ");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine($"Invalid input: ({input}) ");
                Console.WriteLine();
            }
        }

        
        
    }
    
    
    
}