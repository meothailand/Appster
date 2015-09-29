using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Models.Business
{
    public class SiteMainMenu
    {

        public static List<Menu> MainMenu { get; private set; }
        static SiteMainMenu()
        {
            GenerateMenu();
        }

        private static void GenerateMenu()
        {
            MainMenu = new List<Menu>();
            MainMenu.Add(new Menu()
            {
                MenuText = "Dashboard",
                MenuLink = "/Dashboard"
            });
            MainMenu.Add(new Menu()
            {
                MenuText = "Users",
                MenuLink = "/Users"
            });
            MainMenu.Add(new Menu()
            {
                MenuText = "Manage Push Notifications",
                MenuLink = "/PushNotifications"
            });
        }
    }

    public class Menu
    {
        public bool IsCurrent { get; set; }
        public string MenuText { get; set; }
        public string MenuLink { get; set; }
        public List<Menu> SubMenu { get; set; }
    }
}