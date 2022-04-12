using System;
using System.Collections.Generic;
using System.IO;

namespace BattleShipBrain
{
    // DTO - Data Transfer Object
    public class SaveGameDTO
    {
        
        public int CurrentPlayerNo { get; set; } = 0;
        public GameConfig GameConfig { get; set; } = new GameConfig();
        public List<ProxyPlayer> Players { get; set; } = new List<ProxyPlayer>();
        
        public class ProxyPlayer
        {
            
            public Dictionary<string, BoardSquareState> Board { get; set; } = new Dictionary<string, BoardSquareState>();
            public List<Ship> Ships { get; set; } = new List<Ship>();
            public List<Tuple<int, int>> AvailableCoordinates { get; set; } = new List<Tuple<int, int>>();
            
        }
        
        
    }

    
}