using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Threading;
using Model;

namespace Common
{
    /// <summary>
    /// Function: 数据扩展类
    /// (
    ///    使用前引用该命名空间，使用：
    ///    DataRow dr = new BUser().GetUser();  
    ///    MO_user mo_user = dr.ToModel<MO_user>();
    ///    DataTable dt = new BUser().GetUsers();
    ///    List<MO_user> list = dt.ToList<Mo_user>();
    /// )
    /// Date:   2009-9-25
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// DataRow扩展方法：将DataRow类型转化为指定类型的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static List<string> reciever_list=new List<string>();
        public static DataTable dttemp;
        public static T ToModel<T>(this DataRow dr) where T : class, new()
        {
            return ToModel<T>(dr, true);
        }
        /// <summary>
        /// DataRow扩展方法：将DataRow类型转化为指定类型的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        /// <returns></returns>
        /// <summary>
        public static T ToModel<T>(this DataRow dr, bool dateTimeToString) where T : class, new()
        {
            if (dr != null)
                return ToList<T>(dr.Table, dateTimeToString).First();

            return null;
        }
    
        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            return ToList<T>(dt, true);
        }
        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt,string colName,int single) where T : class, new()
        {
            return ToList<T>(dt, true, colName, single);
        }
        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt, bool dateTimeToString) where T : class, new()
        {
            List<T> list = new List<T>();

            if (dt != null)
            {
                List<PropertyInfo> infos = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
                {
                    if (dt.Columns.Contains(p.Name) == true)
                    {
                        infos.Add(p);
                    }
                });

                SetList<T>(list, infos, dt, dateTimeToString);
            }

            return list;
        }
        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt, bool dateTimeToString, string colName, int single) where T : class, new()
        {
            
       
            List<T> list = new List<T>();

            if (dt != null)
            {
                List<PropertyInfo> infos = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
                {
                    if (dt.Columns.Contains(p.Name) == true)
                    {
                        infos.Add(p);
                    }
                });

              SetList<T>(list, infos, dt, dateTimeToString, colName, single);
            }
          
            return list;
        }
        #region 私有方法

        private static void SetList<T>(List<T> list, List<PropertyInfo> infos, DataTable dt, bool dateTimeToString) where T : class, new()
        {
            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();

                infos.ForEach(p =>
                {
                    if (dr[p.Name] != DBNull.Value)
                    {
                        object tempValue = dr[p.Name];
                        if (dr[p.Name].GetType() == typeof(DateTime) && dateTimeToString == true)
                        {
                            tempValue = dr[p.Name].ToString();
                        }
                        try
                        {
                            p.SetValue(model, tempValue, null);
                        }
                        catch { }
                    }
                });
                list.Add(model);
            }
        }

        private static string SetList<T>(List<T> list, List<PropertyInfo> infos, DataTable dt, bool dateTimeToString,string colName,int single) where T : class, new()
        {
            List<string> strs = new List<string>();
            string receivers="";
            int i=0;
            dttemp = dt.Copy();
            dttemp.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                //if (reciever_list.Count > 0 && reciever_list.FindIndex(delegate(string p) { return p.IndexOf(dr[colName].ToString()) >= 0; }) >= 0) continue;
                //foreach (S_Log_MsgReceiverInfo model2 in list2)
                //{
                //    if (model2.Receiver == dr[colName].ToString()) continue; 
                //}
                if (i > 0)
                {
                    if (dr[colName].ToString() == dt.Rows[i - 1][colName].ToString())
                    {
                        i++;
                        continue;
                       
                    }
                }
                dttemp.ImportRow(dr);
                if (list.Count % single == 0)
                {
                    if(receivers.Trim()!="")
                        reciever_list.Add(receivers);
                    receivers = "";

                }
                receivers += dr[colName] + ",";

               
                T model = new T();

                infos.ForEach(p =>
                {
                    if (dr[p.Name] != DBNull.Value)
                    {
                        object tempValue = dr[p.Name];
                        if (dr[p.Name].GetType() == typeof(DateTime) && dateTimeToString == true)
                        {
                            tempValue = dr[p.Name].ToString();
                        }
                        try
                        {
                            p.SetValue(model, tempValue, null);

                        }
                        catch { }
                    }
                });
                list.Add(model);
                i++;
            }
            reciever_list.Add(receivers);
            //reciever_list.Clear();
            //reciever_list.AddRange(strs);
            //strs.Clear();
            dt = dttemp.Copy();
            dttemp.Dispose();
            return "";
        }


        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        /// <returns></returns>
        public static List<Object> ToObjList<T>(this DataTable dt, bool dateTimeToString) where T : class, new()
        {
            List<Object> list = new List<Object>();

            if (dt != null)
            {
                List<PropertyInfo> infos = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
                {
                    if (dt.Columns.Contains(p.Name) == true)
                    {
                        infos.Add(p);
                    }
                });

                SetList<T>(list, infos, dt, dateTimeToString);
            }

            return list;
        }

     

        private static void SetList<T>(List<Object> list, List<PropertyInfo> infos, DataTable dt, bool dateTimeToString) where T : class, new()
        {
            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();

                infos.ForEach(p =>
                {
                    if (dr[p.Name] != DBNull.Value)
                    {
                        object tempValue = dr[p.Name];
                        if (dr[p.Name].GetType() == typeof(DateTime) && dateTimeToString == true)
                        {
                            tempValue = dr[p.Name].ToString();
                        }
                        try
                        {
                            p.SetValue(model, tempValue, null);
                        }
                        catch { }
                    }
                });
                list.Add(model);
            }
        }

        #endregion
        ///// <summary>
        ///// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <returns></returns>
        //public static List<T> ToList<T>(this DataTable dt,string colName) where T : class, new()
        //{
        //    return ToList<T>(dt, true,receivers);
        //}
        ///// <summary>
        ///// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        ///// <returns></returns>
        //public static List<T> ToList<T>(this DataTable dt, bool dateTimeToString, string colName) where T : class, new()
        //{
        //    List<T> list = new List<T>();

        //    if (dt != null)
        //    {
        //        List<PropertyInfo> infos = new List<PropertyInfo>();

        //        Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
        //        {
        //            if (dt.Columns.Contains(p.Name) == true)
        //            {
        //                infos.Add(p);
                        
        //            }
        //        });

        //        SetList<T>(list, infos, dt, dateTimeToString);
        //    }

        //    return list;
        //}
    }
}