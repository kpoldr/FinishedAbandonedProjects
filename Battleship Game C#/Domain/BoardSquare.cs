namespace Domain
{
    public class BoardSquare
    {
        public int BoardSquareId { get; set; }
        public int CordX { get; set; }
        public int CordY { get; set; }
        public bool IsShip { get; set; }
        public bool IsBomb { get; set; }

        public int? ShipId { get; set; }
        public Ship? Ship { get; set; }
        
        public int PlayerId { get; set; }
        public Player? Player { get; set; } 
        
    }
}