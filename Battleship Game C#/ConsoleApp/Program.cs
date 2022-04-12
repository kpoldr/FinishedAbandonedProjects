// using System;
// using System.Collections.Generic;
// using CalculatorBrain;
// using MenuSystem;
//
// namespace ConsoleApp
// {
//     class Program
//     {
//
//         private static readonly Brain Brain = new Brain();
//         
//         
//         static void Main(string[] args)
//         {
//             Console.Clear();
//
//
//             var mainMenu = new Menu("Battleships MainMenu", EMenuLevel.Root);
//             mainMenu.AddMenuItems(new List<MenuItem>()
//             {
//                 new MenuItem("N", "New game", SubmenuNewGame),
//                 // new MenuItem("S", "Unary operations", SubmenuUnary),
//             });
//
//             mainMenu.Run();
//         }
//
//         public static string ReturnCurrentDisplayValue()
//         {
//             return Brain.CurrentValue.ToString();
//         }
//
//         public static string MethodA()
//         {
//             Console.WriteLine("Method A!!!!!");
//             return "";
//         }
//
//         public static string SubmenuNewGame()
//         {
//             var menu = new Menu(ReturnCurrentDisplayValue, "Binary", EMenuLevel.First);
//             menu.AddMenuItems(new List<MenuItem>()
//             {
//                 new MenuItem("+", "+", Add),
//                 // new MenuItem("-", "-", MethodA),
//                 // new MenuItem("/", "/", MethodA),
//                 // new MenuItem("*", "*", Multiplication),
//             });
//             var res = menu.Run();
//             return res;
//         }
//
//         // public static string Multiplication()
//         // {
//         //     Console.WriteLine("Current value: " + Brain.CurrentValue);
//         //     Console.WriteLine("multiply");
//         //     Console.Write("number: ");
//         //     var n = Console.ReadLine()?.Trim();
//         //     double.TryParse(n, out var converted);
//         //
//         //     //Brain.ApplyCustomFunction( CustomMultiply, converted);
//         //     Brain.ApplyCustomFunction( (a,  b) => a * b, converted);
//         //     return "";
//         // }
//
//         // public static double CustomMultiply(double a , double b)
//         // {
//         //     return a * b;
//         // }
//
//         // public static string Add()
//         // {
//         //     // CalculatorCurrentDisplay
//         //     Console.WriteLine("Current value: " + Brain.CurrentValue);
//         //     Console.WriteLine("plus");
//         //     Console.Write("number: ");
//         //     var n = Console.ReadLine()?.Trim();
//         //     double.TryParse(n, out var converted);
//         //
//         //     Brain.Add(converted);
//         //
//         //     return "";
//         // }
//         
//     }
// }