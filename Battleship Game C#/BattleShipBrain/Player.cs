using System;
using System.Collections.Generic;

namespace BattleShipBrain
{
    public class Player
    {
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public Dictionary<(int, int) , BoardSquareState> Board = new Dictionary<(int, int), BoardSquareState>();
        public List<(int, int)> AvailableCoordinates = new List<(int, int)>();
    }
}
