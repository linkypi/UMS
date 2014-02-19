using System;
using System.Data.Linq;
using System.Linq;
using Model;

namespace DAL
{
    public class Pro_SellListInfo_Temp
    {
        private int MenthodID;
        public Pro_SellListInfo_Temp(int method)
        {
            this.MenthodID = method;
        }
        public Pro_SellListInfo_Temp()
        {
            this.MenthodID = 0;
        }

        public Model.WebReturn GetAllOldID(Model.Sys_UserInfo sysUser)
        {
            using (LinQSqlHelper lqh=new LinQSqlHelper())
            {
                try
                {
                    var query = lqh.Umsdb.Pro_SellListInfo_Temp.GroupBy(temp => temp.OldID);
                    if (query.Any())
                    {
                        return new WebReturn() {ReturnValue = true, Obj = query.Select(temps => temps.Key).ToList()};
                    }
                    else
                    {
                        return new WebReturn() {ReturnValue = false, Message = "无可用数据"};
                    }
                }
                catch (Exception ex)
                {
                    return new WebReturn(){Message = ex.Message,ReturnValue = false};
                    throw;
                }
            }
        }
        public Model.WebReturn GetList(Model.Sys_UserInfo sysUser,string OLDID)
        {
            Model.WebReturn r = new WebReturn();
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions d=new DataLoadOptions();
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_Sell_Yanbao_temp);
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.VIP_VIPInfo_Temp);
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_Sell_JiPeiKa_temp);
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_BillInfo_temp);
                try
                {
                    var query_area = from b in lqh.Umsdb.GetTable<Model.Pro_SellListInfo_Temp>()
                                     //where OLDID==b.OldID
                                     select b;
                    if (query_area == null || query_area.Count() == 0)
                    {
                        r.Obj = null;
                        r.ReturnValue = false;
                        r.Message = "无可用数据";
                        return r;
                    }
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query_area);
                    r.Obj = query_area.ToList();
                    r.ReturnValue = true;
                    return r;

                }
                catch (Exception ex)
                {
                    r.Obj = null;
                    r.ReturnValue = false;
                    r.Message = ex.Message;
                    return r;
                }
            }
        }

        public Model.WebReturn GetList(Model.Sys_UserInfo sysUser)
        {
            Model.WebReturn r =new WebReturn();
            using (LinQSqlHelper lqh = new LinQSqlHelper())


            {
                
              
                DataLoadOptions d = new DataLoadOptions();
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_Sell_Yanbao_temp);
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.VIP_VIPInfo_Temp);
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp=>temp.Pro_Sell_JiPeiKa_temp);
                d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_BillInfo_temp);
                lqh.Umsdb.LoadOptions = d;
                try
                {
                    var query_area = from b in lqh.Umsdb.GetTable<Model.Pro_SellListInfo_Temp>()
                                     select b;
                    if (query_area == null || query_area.Count() == 0)
                    {
                        r.Obj = null;
                        r.ReturnValue = false;
                        r.Message = "无可用数据";
                        return r;
                    }
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query_area);
                    r.Obj = query_area.OrderByDescending(p=>p.ID).ToList();
                    r.ReturnValue = true;
                    return r;

                }
                catch (Exception ex)
                {
                    r.Obj = null;
                    r.ReturnValue = false;
                    r.Message = ex.Message;
                    return r;
                }
            }
        }
    }
}