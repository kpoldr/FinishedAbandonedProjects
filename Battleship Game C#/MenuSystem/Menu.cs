using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public class Menu
    {
        public int SelectedIndex { get; set; } = 0;
        public string SelectedOption => _menuItems[SelectedIndex].Title;
        public void MoveUp() => SelectedIndex = Math.Max(SelectedIndex - 1, 0);
        public void MoveDown() => SelectedIndex = Math.Min(SelectedIndex + 1, _menuItems.Count - 1);

        private readonly EMenuLevel _menuLevel;

        public List<MenuItem> _menuItems = new List<MenuItem>();
        private readonly MenuItem _menuItemExit = new MenuItem("Exit", null, null, null);
        private readonly MenuItem _menuItemReturn = new MenuItem("Return", null, null, null, true);
        private readonly MenuItem _menuItemOk = new MenuItem("Ok", null, null, null, true);

        private readonly HashSet<string> _menuShortCuts = new HashSet<string>();
        private readonly HashSet<string> _menuSpecialShortCuts = new HashSet<string>();

        private readonly string _title;


        public Menu(string title, EMenuLevel menuLevel)
        {
            _title = title;
            _menuLevel = menuLevel;
            AddCoreMenuItems();
        }


        public void AddCoreMenuItems()
        {
            switch (_menuLevel)
            {
                case EMenuLevel.Root:
                    _menuItems.Add(_menuItemExit);
                    break;
                case EMenuLevel.First:
                    _menuItems.Add(_menuItemReturn);
                    _menuItems.Add(_menuItemExit);
                    break;
                case EMenuLevel.Ship:
                    _menuItems.Add(_menuItemOk);
                    break;
                case EMenuLevel.Else:
                    _menuItems.Add(_menuItemReturn);
                    break;
            }
        }

        public void AddMenuItem(MenuItem item, int position = -1)
        {
            int indexMover = 1;

            if (_menuLevel == EMenuLevel.First)
            {
                indexMover = 2;
            }

            _menuItems.Insert(_menuItems.Count - indexMover, item);
        }

        public void AddMenuItemCustom(MenuItem item, int position)
        {
            _menuItems.Insert(position, item);
        }

        public void DeleteItem()
        {
            _menuItems.RemoveAt(SelectedIndex);
        }

        public void DeleteAllMenuItems()
        {
            _menuItems.Clear();
            AddCoreMenuItems();
        }

        public void AddMenuItems(List<MenuItem> items)
        {
            foreach (var menuItem in items)
            {
                AddMenuItem(menuItem);
            }
        }

        public void RestartIndex()
        {
            SelectedIndex = 0;
        }

        public void ClearMenuItems()
        {
            _menuItems = new List<MenuItem>();
        }

        public string Run(bool breakEarly = false)
        {
            do
            {
                Console.Clear();
                OutputMenu();

                var readInput = false;
                MenuItem? item;
                var keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        MoveUp();
                        continue;

                    case ConsoleKey.DownArrow:
                        MoveDown();
                        continue;

                    case ConsoleKey.Enter:
                        readInput = true;
                        break;

                    case ConsoleKey.LeftArrow:
                        item = _menuItems.FirstOrDefault(t => t.Title == SelectedOption);
                        item?.LeftMethod?.Invoke();
                        continue;

                    case ConsoleKey.RightArrow:
                        item = _menuItems.FirstOrDefault(t => t.Title == SelectedOption);
                        item?.RightMethod?.Invoke();
                        continue;
                }

                if (!readInput) continue;

                if (SelectedOption == "Exit")
                {
                    Environment.Exit(0);
                }


                item = _menuItems.FirstOrDefault(t => t.Title == SelectedOption);
                item?.MainMethod?.Invoke();
                RestartIndex();

                if (item != null && item.ReturnRun || breakEarly)
                {
                    break;
                }
            } while (true);

            return SelectedOption == "Return" ? "" : SelectedOption;
        }


        public void Paint(int x, int y)
        {
            for (int i = 0; i < _menuItems.Count; i++)
            {
                Console.SetCursorPosition(x, y + i);

                var color = SelectedIndex == i ? ConsoleColor.Yellow : ConsoleColor.Gray;

                Console.ForegroundColor = color;
                Console.Write("  ");
                Console.WriteLine(_menuItems[i]);
            }
        }

        private void OutputMenu()
        {
            Console.WriteLine("====> " + _title + " <====");

            Console.WriteLine("-------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            Paint(0, 3);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("=====================");
        }
    }
}