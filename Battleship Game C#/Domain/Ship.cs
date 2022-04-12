using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Ship
    {
        public int ShipId { get; set; } 
        
        [DisplayName("Ship Name")]
        [MaxLength(30)]
        public string ShipName { get; set; } = default!;
        
        
        
        [DisplayName("Ship Height")]
        [Required(ErrorMessage="Minimum ship height is 1")]
        [Range(1,20)]
        public int ShipSizeX { get; set; }
        
        [DisplayName("Ship Length")]
        [Required(ErrorMessage="Minimum is length is 1")]
        [Range(1,20)]
        public int ShipSizeY { get; set; }
        
        public List<BoardSquare>? ShipSquares { get; set; } 

        public int PlayerId { get; set; }
        public Player? Player { get; set; }
    }
}