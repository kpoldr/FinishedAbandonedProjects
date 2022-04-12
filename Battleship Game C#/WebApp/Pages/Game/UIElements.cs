using System;
using System.Drawing;
using BattleShipBrain;

namespace WebApp.Pages.Game
{
    public class UIElements
    {
        public static string ColorSquare(BoardSquareState symbol, bool enemy = false, bool sunken = false)
        {
            if (symbol.IsShip && symbol.IsBomb)
            {
                if (enemy)
                {
                    return sunken ? ColorTranslator.ToHtml(Color.Green) : ColorTranslator.ToHtml(Color.Yellow);
                }
              
                return sunken ? ColorTranslator.ToHtml(Color.Red) : ColorTranslator.ToHtml(Color.Orange);
                
            }

            if (!enemy)
            {

                if (symbol.IsShip)
                {
                    return ColorTranslator.ToHtml(Color.Black);    
                    
                } else if (symbol.IsBomb)
                {
                    return ColorTranslator.ToHtml(Color.LightGray);
                }
                
                 
            }  
            
            if (enemy && symbol.IsBomb )
            
            {
                return ColorTranslator.ToHtml(Color.Gray);
            }
            
            return ColorTranslator.ToHtml(Color.White);
            
        }
        
        public static string ShipSpace(bool ekstra = false, bool selected = false)
        {

            if (ekstra)
            {
                return ColorTranslator.ToHtml(selected ? Color.Magenta : Color.DarkGray);
            }
            
            return ColorTranslator.ToHtml(Color.Gray);
        }
        
        
    }
}
