namespace BattleShipBrain
{
    public class BoardSquareState
    {
        public bool IsShip { get; set; }
        public bool IsBomb { get; set; }

        public override string ToString()
        {
            switch (IsEmpty: IsShip, IsBomb)
            {
                case (false, false):
                    return " ";
                case (false, true):
                    return "x";
                case (true, false):
                    return "■";
                case (true, true):
                    return "*";
            }
        }

        public string EnemyStrings()
        {
            switch (IsEmpty: IsShip, IsBomb)
            {
                case (false, false):
                    return " ";
                case (false, true):
                    return "X";
                case (true, false):
                    return " ";
                case (true, true):
                    return "*";
            }
        }
    }
}