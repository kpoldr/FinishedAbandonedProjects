using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    //  logic for selecting specific option
    public class Menu
    {
        public Menu (IEnumerable<string> items)
        {
            Items = items.ToArray ();
        }


        public IReadOnlyList<string> Items { get; }

        public int SelectedIndex { get; private set; } = 0; // nothing selected

        public string SelectedOption =>  Items[SelectedIndex];


        public void MoveUp () => SelectedIndex = Math.Max (SelectedIndex - 1, 0);

        public void MoveDown () => SelectedIndex = Math.Min (SelectedIndex + 1, Items.Count - 1);
    }


    // logic for drawing menu list
    public class ConsoleMenuPainter
    {
        readonly Menu menu;

        public ConsoleMenuPainter (Menu menu)
        {
            this.menu = menu;
        }

        public void Paint (int x, int y)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                Console.SetCursorPosition (x, y + i);

                var color = menu.SelectedIndex == i ? ConsoleColor.Yellow : ConsoleColor.Gray;

                Console.ForegroundColor = color;
                Console.WriteLine (menu.Items[i]);
            }
        }
    }


    internal class Program
    {
        public static void Main (string[] args)
        {
            var menu = new Menu (new string[] { "John", "Bill", "Janusz", "Grażyna", "1500", ":)" });
            var menuPainter = new ConsoleMenuPainter (menu);

            bool done = false;

            do
            {
                menuPainter.Paint (8, 5);

                var keyInfo = Console.ReadKey ();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow   : menu.MoveUp ();   break;
                    case ConsoleKey.DownArrow : menu.MoveDown (); break;
                    case ConsoleKey.Enter     : done = true;      break;
                }
            }
            while (!done);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine ("Selected option: " + (menu.SelectedOption ?? "(nothing)"));
            Console.ReadKey ();
        }
    }
}