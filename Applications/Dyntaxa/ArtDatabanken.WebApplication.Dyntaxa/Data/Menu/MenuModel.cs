using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Menu
{
    public class MenuModel
    {
        public List<MenuItem> MenuItems { get; set; }

        public MenuModel()
        {
            MenuItems = new List<MenuItem>();
        }

        public void SetCurrentClassOnMenuItem(string action, string controller)
        {
            foreach (var menuItem in MenuItems)
            {
                if ((menuItem.Action == action && menuItem.Controller == controller) || 
                    FindMenuItem(menuItem, action, controller))
                {
                    menuItem.Current = true;
                    break;
                }
            }
        }

        protected bool FindMenuItem(MenuItem menuItem, string action, string controller)
        {
            if (menuItem == null || menuItem.ChildItems == null)
            {
                return false;
            }

            foreach (var item in menuItem.ChildItems)
            {
                if (item.Action == action && item.Controller == controller)
                {
                    return true;
                }

                if (FindMenuItem(item, action, controller))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
