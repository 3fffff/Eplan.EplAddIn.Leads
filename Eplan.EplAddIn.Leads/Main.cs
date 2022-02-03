using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Gui;
using System;

namespace Eplan.EplAddIn.test
{
    public class Main : IEplAddIn
    {
        public bool OnRegister(ref bool bLoadOnStart)
        {
            bLoadOnStart = true;
            return true;
        }

        public bool OnUnregister()
        {
            return true;
        }

        public bool OnInit()
        {
            return true;
        }

        public bool OnInitGui()
        {
            Menu oMenu = new Menu();
            uint num = oMenu.AddMainMenu("Выноски", Menu.MainMenuName.eMainMenuUtilities, "Добавить выноски", "PageAction",
                "Имя проекта, фирмы и дата создания", 1);
            uint num1 = oMenu.AddMenuItem("Удалить выноски", "ActionTest", "Информация", num, 1, true, false);
            return true;
        }

        public bool OnExit()
        {
            return true;
        }
    }
}
