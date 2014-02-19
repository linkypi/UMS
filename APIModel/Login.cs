using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace APIModel
{
    public class MenuList
    {
        public MenuList()
        {
            this.child = new List<MenuList>();
        }
        public string menuid { get; set; }
        public string menuname { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public List<MenuList> child { get; set; }
    }
    public class Login
    {
        private Model.Sys_UserInfo userinfo;

        public Login(Model.Sys_UserInfo userinfo)
        {
            this.userinfo = userinfo;

           menu=  JsonConvert.DeserializeObject<List<MenuList>>(userinfo.Sys_RoleInfo.MobileMenuJson);
        }

        public MenuList genmenu(Model.Sys_MenuInfo menuinfo)
        {
            MenuList result=new MenuList();
            result.menuid = menuinfo.MenuID+"";
            result.menuname = menuinfo.MenuText + "";
            result.image = menuinfo.MenuImg;
            result.child=new List<MenuList>();
            foreach (var VARIABLE in menuinfo.Menu.Where(p=>p.DisplayMobile))
            {
                result.child.Add(genmenu(VARIABLE));
            }
            return result;
        }
        public string username{get { return userinfo.UserName; }}

        public string realname
        {
            get
            {
                return userinfo.RealName;
                
            }
        }

        public List<MenuList> menu { get; set; }
    }
}