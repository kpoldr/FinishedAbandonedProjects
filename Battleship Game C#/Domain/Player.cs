using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }
        public int PlayerNumber { get; set; } = default!;
        public List<Ship>? Ships { get; set; }
        public List<BoardSquare>? BoardSquares { get; set; } 
        
        public int GameConfigId { get; set; }
        public GameConfig GameConfig { get; set; } = default!;
        
    }
}