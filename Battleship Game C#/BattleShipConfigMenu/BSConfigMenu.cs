using System;
using System.Collections;
using System.Collections.Generic;
using BattleShipBrain;
using MenuSystem;

namespace BattleShipConfigMenu
{
    public static class BSConfigMenu
    {
        private static int _width;
        private static int _heigth;
        private static int _ships;

        private static int _selectedShipX = 1;
        private static int _selectedShipY = 1;
        private static string _selectedShipName = "";
        private static bool _deleteShip = false;
        private static bool _newShip = false;
        private static bool _createGame = false;

        private static List<Ship> _shipsList = new List<Ship>();
        private static GameConfig _gameConfig = new GameConfig();
        private static Menu _menu = new Menu("Custom Game", EMenuLevel.First);
        private static Menu _shipMenu = new Menu("Ships", EMenuLevel.Ship);
        private static Menu _shipCreateEditMenu = new Menu("Ship", EMenuLevel.Ship);

        private static EShipTouchRule[] _rules = new[]
            {EShipTouchRule.NoTouch, EShipTouchRule.CornerTouch, EShipTouchRule.SideTouch};

        private static int _rulePointer = 0;

        public static (GameConfig, bool) StartConfigMenu()
        {
            _width = 10;
            _heigth = 10;
            _ships = 0;
            _createGame = false;

            _shipsList.Clear();

            ConfigMenu();

            _gameConfig.BoardSizeX = _width;
            _gameConfig.BoardSizeY = _heigth;
            _gameConfig.EShipTouchRule = _rules[_rulePointer];
            _gameConfig.Ships = _shipsList;
            return (_gameConfig, _createGame);
        }

        public static void ConfigMenu()
        {
            _menu.DeleteAllMenuItems();
            _menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("Board width: <10>", null, LowerWidth, IncreaseWidth),
                new MenuItem("Board height: <10>", null, LowerHeigth, IncreaseHeigth),
                new MenuItem("Placement rule: NoTouch", null, PreviousRule, NextRule),
                new MenuItem("Create/Edit Ships [0]", Ships, null, null),
                new MenuItem("Create game", CreateGame, null, null, true)
            });

            _menu.Run();
        }

        public static void NextRule()
        {
            if (_rulePointer == 2)
            {
                _rulePointer = 0;
            }
            else
            {
                _rulePointer++;
            }

            _menu._menuItems[_menu.SelectedIndex].Title = $"Placement rule: {_rules[_rulePointer]}";
        }

        public static void PreviousRule()
        {
            if (_rulePointer == 0)
            {
                _rulePointer = 2;
            }
            else
            {
                _rulePointer--;
            }

            _menu._menuItems[_menu.SelectedIndex].Title = $"Placement rule: {_rules[_rulePointer]}";
        }

        public static void CreateGame()
        {
            _createGame = true;
        }

        public static void CreateEditShip()
        {
            _shipCreateEditMenu.DeleteAllMenuItems();
            if (_shipMenu.SelectedIndex == _shipMenu._menuItems.Count - 2)
            {
                _selectedShipX = 1;
                _selectedShipY = 1;
                _selectedShipName = "Untitled";
                _newShip = true;
            }
            else
            {
                _selectedShipX = _shipsList[_shipMenu.SelectedIndex].Y;
                _selectedShipY = _shipsList[_shipMenu.SelectedIndex].X;
                _selectedShipName = _shipsList[_shipMenu.SelectedIndex].Name;
            }

            _shipCreateEditMenu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem($"Ship Name: {_selectedShipName}", EditName, null, null),
                new MenuItem($"Ship size X: <{_selectedShipY}>", null, DecreaseShipSizeX, IncreaseShipSizeX),
                new MenuItem($"Ship size Y: <{_selectedShipX}>", null, DecreaseShipSizeY, IncreaseShipSizeY),
                new MenuItem("Delete ship", DeleteShip, null, null, true)
            });

            _shipCreateEditMenu.Run();

            if (_newShip)
            {
                if (!_deleteShip)
                {
                    _shipsList.Add(new Ship(_selectedShipName, _selectedShipX, _selectedShipY));
                    _shipMenu.AddMenuItemCustom(new MenuItem($"{_selectedShipName}", CreateEditShip, null, null),
                        _ships);
                    _ships++;
                    _newShip = false;
                }
                else
                {
                    _deleteShip = false;
                }
            }
            else
            {
                if (_deleteShip)
                {
                    _shipMenu.DeleteItem();
                    _deleteShip = false;
                    _ships--;
                }
                else
                {
                    _shipMenu._menuItems[_shipMenu.SelectedIndex].Title = _selectedShipName;
                }
            }
        }


        public static void Ships()
        {
            _shipMenu.DeleteAllMenuItems();

            foreach (var ship in _shipsList)
            {
                _shipMenu.AddMenuItem(new MenuItem(ship.Name, CreateEditShip, null, null));
            }

            _shipMenu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("Create ship", CreateEditShip, null, null)
            });

            _shipMenu.Run();

            _menu._menuItems[3].Title = $"Create/Edit Ships [{_ships}]";
        }

        public static void EditName()
        {
            var input = "";
            Console.Write("New name: ");

            while (true)
            {
                input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(input))
                {
                    break;
                }
            }

            _selectedShipName = input;
            _shipCreateEditMenu._menuItems[_shipCreateEditMenu.SelectedIndex].Title = $"Ship name: {input}";
        }

        public static void DeleteShip()
        {
            _deleteShip = true;
        }

        public static void IncreaseShipSizeX()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_selectedShipX, false, _width);
            _selectedShipX = item1;
            _shipCreateEditMenu._menuItems[_shipCreateEditMenu.SelectedIndex].Title =
                $"{_shipCreateEditMenu.SelectedOption.Split(":")[0]}: {item2}";
        }

        public static void DecreaseShipSizeX()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_selectedShipX, true, _width);
            _selectedShipX = item1;
            _shipCreateEditMenu._menuItems[_shipCreateEditMenu.SelectedIndex].Title =
                $"{_shipCreateEditMenu.SelectedOption.Split(":")[0]}: {item2}";
        }

        public static void IncreaseShipSizeY()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_selectedShipY, false, _heigth);
            _selectedShipY = item1;
            _shipCreateEditMenu._menuItems[_shipCreateEditMenu.SelectedIndex].Title =
                $"{_shipCreateEditMenu.SelectedOption.Split(":")[0]}: {item2}";
        }

        public static void DecreaseShipSizeY()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_selectedShipY, true, _heigth);
            _selectedShipY = item1;
            _shipCreateEditMenu._menuItems[_shipCreateEditMenu.SelectedIndex].Title =
                $"{_shipCreateEditMenu.SelectedOption.Split(":")[0]}: {item2}";
        }

        public static void LowerWidth()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_width, true, 20);
            _width = item1;
            _menu._menuItems[_menu.SelectedIndex].Title = $"{_menu.SelectedOption.Split(":")[0]}: {item2}";
        }

        public static void IncreaseWidth()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_width, false, 20);
            _width = item1;
            _menu._menuItems[_menu.SelectedIndex].Title = $"{_menu.SelectedOption.Split(":")[0]}: {item2}";
        }

        public static void LowerHeigth()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_heigth, true, 20);
            _heigth = item1;
            _menu._menuItems[_menu.SelectedIndex].Title = $"{_menu.SelectedOption.Split(":")[0]}: {item2}";
        }

        public static void IncreaseHeigth()
        {
            var (item1, item2) = IncreaseDecreaseTemplate(_heigth, false, 20);
            _heigth = item1;
            _menu._menuItems[_menu.SelectedIndex].Title = $"{_menu.SelectedOption.Split(":")[0]}: {item2}";
        }

        // Could be made much smaller with pointers
        public static (int, string) IncreaseDecreaseTemplate(int size, bool decrease, int max)
        {
            string number = "";
            if (size == 1)
            {
                number = "|1>";
            }
            else if (size == max)
            {
                number = $"<{max}|";
            }


            if (decrease)
            {
                if (size == 1) return (size, number);
                size--;
                number = size == 1 ? "|1>" : $"<{size}>";
            }
            else
            {
                if (size == max) return (size, number);
                size++;
                number = size == max ? $"<{max}|" : $"<{size}>";
            }

            return (size, number);
        }
    }
}