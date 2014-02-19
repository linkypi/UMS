using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MenuInfo:Sys_MenuInfo
    {
        private List<Sys_MenuInfo> menus;

        public List<Sys_MenuInfo> Menus
        {
            get { return menus; }
            set { menus = value; }
        }

        public MenuInfo()
        {
        }
        public MenuInfo(Sys_MenuInfo menu)
        {
            this.MenuID = menu.MenuID;
            this.MenuImg = menu.MenuImg;
            this.HasHallRole = menu.HasHallRole;
            this.HasProRole = menu.HasProRole;
            this.Flag = menu.Flag;
            this.MenuText = menu.MenuText;
            this.MenuValue = menu.MenuValue;
            this.Note = menu.Note;
            this.Order = menu.Order;
            this.Parent = menu.Parent;
       
        }
    }
}
