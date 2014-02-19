using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Sys_Role_Menu_HallInfo : DAL.Sys_InitParentInfo
    {
        private int MenthodID;

	    public Sys_Role_Menu_HallInfo()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_Role_Menu_HallInfo(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }

        public List<Model.Sys_Role_Menu_HallInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {

                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_Role_Menu_HallInfo>()
                                where b.RoleID == user.RoleID
                                select b;
                    
                    //    System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query);
                    // arr.Add(query);


                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_Role_Menu_HallInfo>();
                }
            }
        }
        public Model.WebReturn GetList(Model.Sys_UserInfo user,int roleid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {

                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_Role_Menu_HallInfo>()
                                where b.RoleID == roleid
                                select b;
                     

                    return new Model.WebReturn(){ Obj =query.ToList(), ReturnValue=true};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() {  ReturnValue=false, Message="系统出错"};
                }
            }
        }
    }
}
