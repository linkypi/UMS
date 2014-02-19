using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using DAL;
using System.Reflection;

namespace Common
{
    public class MainCheckHelp
    {
        /// <summary>
        /// 解密时间戳
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static bool CheckTimeTrick(string sign)
        {          
            return true;    
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Model.WebReturn Login(string userName, string password)
        {
            Model.Sys_UserInfo user = new Model.Sys_UserInfo() { UserName = userName, UserPwd = password };

            DAL.Sys_LoginInfo login = new DAL.Sys_LoginInfo();

            return  login.Add(user);
        }


        /// <summary>
        /// 解析头部请求，获取反射组件、类、函数
        /// </summary>
        /// <param name="header">函数的数据库编号</param>
        /// <param name="list">客户端提供的参数</param>
        /// <returns></returns>
        public static Model.WebReturn MainRequest(int header, object[] objs, Model.Sys_UserInfo user)
        {
            
            try
            {
                LinQSqlHelper lqh = new LinQSqlHelper();
                var method = from b in lqh.Umsdb.Sys_MethodInfo
                             where b.MethodID == header
                             select b;
                if (method.Count() == 0)
                {
                    return new WebReturn() {  ReturnValue=false, Message="方法不存在"};
                }
                Model.Sys_MethodInfo model = method.First();
                return MethodLoad(model, objs, user);
                  
            }
            catch (Exception ex)
            {
                return new WebReturn() {  ReturnValue=false , Message="系统出错"+ex.Message};
            }
            
        }

        /// <summary>
        /// 获取报表信息
        /// </summary>
        /// <param name="reportid"></param>
        /// <returns></returns>
        public static Model.WebReturn GetReportViewInfo(string reportName)
        {
            DAL.Demo_ReportViewInfo model = new DAL.Demo_ReportViewInfo();
            return model.GetModel(reportName);
        }

        /// <summary>
        /// 反射调用方法
        /// </summary>
        /// <param name="DllName">组件名称</param>
        /// <param name="ClassName">类名</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="objs">参数</param>
        /// <returns></returns>
        public static Model.WebReturn MethodLoad(Model.Sys_MethodInfo model, object[] objs, Model.Sys_UserInfo user)
        {
            Model.WebReturn web_return = null;
            //return System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("RequestCom", "Maticsoft.BLL"); 
            Assembly assembly = Assembly.LoadFrom(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("Common", model.DllName));
            Type T = assembly.GetType(model.DllName + "." + model.ClassName);

            object classObj = Activator.CreateInstance(T, new object[] { model .MethodID});//创建实例

            MethodInfo method = null;//提取方法信息
            Type[] types = null;
            //如果参数不为空，而且最后一个参数是sqlparaminfo 列表
            //if (objs == null || objs.Count() == 0)
            //    method = T.GetMethod(model.MethodName, new Type[] { });
            //else//带条件语句
            //{
                
            //    else
            //    {
                    if(objs==null) objs=new object[0];
                    types = new Type[objs.Count()+1];
                    types[0] = user.GetType();
                    for (int i = 0; i < objs.Count(); i++)
                    {
                        types[i+1] = objs[i].GetType();
                    }
                //}
                    object[] args = new object[1 + objs.Count()];
                    args[0] = user;
                    for (int i = 0; i < objs.Count(); i++)
                    {
                        args[i + 1] = objs[i];
                    }
                   method = T.GetMethod(model.MethodName, types);
            //}
            web_return = (Model.WebReturn)method.Invoke(classObj, args);//调用方法 
            //if (returnObj is System.Data.DataSet)
            //{
            //    web_return.MyDS = FilterDs(user, model, (System.Data.DataSet)returnObj);
            //    if (objs != null && objs.Length > 0)
            //        web_return.ReturnModel = objs[0];
            //}
            //else
            //{
            //    web_return.ReturnObj = returnObj;
            //    if (objs != null && objs.Length > 0)
            //        web_return.ReturnModel = objs[0];
            //}
            return web_return;
        }
    }
}
