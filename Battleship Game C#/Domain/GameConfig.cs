using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameConfig
    { 
        public int GameConfigId { get; set; }
        
        [DisplayName("Game Name")]
        [MaxLength(20)]
        public string GameName { get; set; } = default!;
        
        
        [DisplayName("Board Height")]
        [Range(1,20)]
        public int BoardSizeX { get; set; }
        
        [DisplayName("Board Length")]
        [Range(1,20)]
        public int BoardSizeY { get; set; } 
        public int GameRule { get; set; }
        public int NumberOfShips { get; set; }
        public List<Player>? Players { get; set; }
        
        
        
        
        
    }
}