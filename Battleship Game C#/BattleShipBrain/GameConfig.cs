using System.Collections.Generic;
using System.Text.Json;

namespace BattleShipBrain
{
    public class GameConfig
    {
        public int BoardSizeX { get; set; } = 10;
        public int BoardSizeY { get; set; } = 10;
        public EShipTouchRule EShipTouchRule { get; set; } = EShipTouchRule.NoTouch;

        public string Name { get; set; } = "Untitled";
        
        public List<Ship> Ships { get; set; } = new List<Ship>
        {
            new Ship("Carrier", 5, 1),
            new Ship("Battleship", 4, 1),
            new Ship("Submarine", 3, 1),
            new Ship("Cruiser", 2, 1),
            new Ship("Patrol", 1, 1)
        };

        public override string ToString()
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, jsonOptions);
        }
    }
}