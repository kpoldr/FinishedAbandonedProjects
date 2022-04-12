#nullable enable
using System;

namespace MenuSystem
{
    public class MenuItem
    {
        public MenuItem(string title, Action? mainMethod, Action? leftMethod, Action? rightMethod, bool returnRun = false)
        {
            
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("title cannot be empty!");
            }
            
            Title = title.Trim();
            MainMethod = mainMethod;
            LeftMethod = leftMethod;
            RightMethod = rightMethod;
            ReturnRun = returnRun;
        }

        public string Title { get; set; }
        public bool ReturnRun { get; set; }
        public Action? MainMethod { get; set; }
        public Action? LeftMethod { get; set; }
        public Action? RightMethod { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}