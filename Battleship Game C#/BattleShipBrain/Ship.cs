using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BattleShipBrain
{
    public class Ship
    {
        public string Name { get; set; }
        public int Y { get; set; }
        public int X { get; set; }

        public List<Tuple<int, int>> _coordinates { get; set; } = new List<Tuple<int, int>>();
        
        public Ship(string name, int x, int y)
        {
            Name = name;
            Y = y;
            X = x;
        }
        
        public void Rotate()
        {
            (Y, X) = (X, Y);
        }
        
        public void SetCoordinate(int x, int y)
        {
            _coordinates.Add(new Tuple<int, int>(x, y));
        }
        
        public bool IsShipSunk(Dictionary<(int, int), BoardSquareState> board)
        {
            return _coordinates.All(coordinate => board[(coordinate.Item1, coordinate.Item2)].IsBomb);
        }
    }
    
}