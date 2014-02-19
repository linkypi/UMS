using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Sys_InitDataStatus
    {
        
        private int MenthodID;

	    public Sys_InitDataStatus()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_InitDataStatus(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }



        /// <summary>
        /// 获取已更新的初始数据
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="dt">客户端最后一次更新时间</param>
        /// <returns></returns>
     
        private Assembly myDll;

        private Sys_InitParentInfo parent;
        public Model.WebReturn GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            //获取已更新列表
            //利用反射 Sys_InitParentInfo.GetList()，存入Sys_InitDataStatus_Child.InitArray
            //无数据则返回null
            //返回值_arrList存放Sys_InitDataStatus_Child列表
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
               
                 
                    var methon_type = from b in lqh.Umsdb.GetTable<Model.Sys_InitDataStatus>()
                                      select b;
                    Model.Sys_InitDataStatus_Child child = new Model.Sys_InitDataStatus_Child();
                    child.InitArray = new ArrayList();
                    foreach (var i in methon_type)
                    {
               
                       myDll = Assembly.LoadFrom(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("DAL",i.DLLName.ToString()));
                        parent = (Sys_InitParentInfo)myDll.CreateInstance(i.DLLName+"." + i.ClassName);
                        Type t = parent.GetType();
                      
                        MethodInfo m=t.GetMethod("GetList", new[] {typeof(Model.Sys_UserInfo), typeof(DateTime)});
                        child.InitArray.Add(m.Invoke(parent, new object[] {user, Convert.ToDateTime(i.UpdDate)}));

                    }                                                                        
                    if (child.InitArray == null)
                    {
                        return new Model.WebReturn() { ReturnValue = true, Message = "无数据" };
                    }
                    return new Model.WebReturn() { ReturnValue = true,ArrList = child.InitArray, Message = "已获取" };
                }
            }
        
    }
}