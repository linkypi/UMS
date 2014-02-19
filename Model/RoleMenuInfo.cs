using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RoleMenuInfo : Sys_Role_MenuInfo
    {
        private Sys_MenuInfo menuInfo;
 

        public Sys_MenuInfo MenuInfo
        {
            get { return menuInfo; }
            set { menuInfo = value; }
        }

        public RoleMenuInfo()
        {
            
        }
        public RoleMenuInfo(Sys_Role_MenuInfo srm)
        {
            this.menuInfo = srm.Sys_MenuInfo;
            this.ID = srm.ID;
            this.MenuID = srm.MenuID;
            this.Note = srm.Note;
            this.RoleID = srm.RoleID;
           
        }
    }
}
