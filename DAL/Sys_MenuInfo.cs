using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Sys_MenuInfo : Sys_InitParentInfo
    {
           private int MenthodID;

	    public Sys_MenuInfo()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_MenuInfo(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }


        public List<Model.Sys_MenuInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_MenuInfo>()
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_MenuInfo>();
                }
            }
        }

        public Model.WebReturn GetAllMenu(Model.Sys_UserInfo user)
        {
            List<Model.MenuInfo> menus = new List<Model.MenuInfo>();

            using (LinQSqlHelper lsh = new LinQSqlHelper())
            {
                var query = from menu in lsh.Umsdb.Sys_MenuInfo
                            where menu.Parent == 0
                            select menu;
                foreach (var item in query)
                {
                    menus.Add(GetChildren(user,item.MenuID));
                }
                return new Model.WebReturn() { ReturnValue = true,Obj=menus };
            }
        }

        /// <summary>
        /// 获取菜单子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Model.MenuInfo GetChildren(Model.Sys_UserInfo user, int id)
        {
            Model.MenuInfo parent = null;
            using (LinQSqlHelper lsh = new LinQSqlHelper())
            {
                var children = from menu in lsh.Umsdb.Sys_MenuInfo
                            where menu.Parent == id
                            select menu;

                var pnode = from menu in lsh.Umsdb.Sys_MenuInfo
                         where menu.MenuID == id
                         select menu;

               parent = new Model.MenuInfo(pnode.First());

                if (children.Count() != 0)
                {
                    Model.MenuInfo mi = null;
                    foreach (var item in children)
                    {
                        mi = new Model.MenuInfo(item);
                        Model.MenuInfo model = GetChildren(user,item.MenuID);
                        if (parent.Menus == null)
                        {
                            parent.Menus = new List<Model.Sys_MenuInfo>();
                        }
                        parent.Menus.Add(model);
                    }
                }
                return parent;
            }
        }
    }
}
