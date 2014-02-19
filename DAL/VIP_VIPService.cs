using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class VIP_VIPService
    {
        private int _MethodID;
        private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public VIP_VIPService()
        {
            this.MethodID = 0;
        }

        public VIP_VIPService(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
          private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="ID" },
        };
          public List<Model.ReportSqlParams> ParamList
          {
              get { return _paramList; }
              set { _paramList = value; }
          }
        #region 获取会员服务
        /// <summary>
        /// 获取调入实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
          public Model.WebReturn GetServiceByVIPID(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {

                    #region 权限
                    //List<string> ValidHallIDS = new List<string>();
                    ////有权限的商品
                    //List<string> ValidProIDS = new List<string>();

                    //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    //if (ret.ReturnValue != true)
                    //{ return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize != 10)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
                    #region 将传入的参数与指定的参数了左连接

                    var param_join = from b in pageParam.ParamList
                                     join c in this.ParamList
                                     on new { b.ParamName, t = b.GetType() }
                                     equals
                                     new { c.ParamName, t = c.GetType() }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     select new
                                     {
                                         ParamFront = b,
                                         ParamBehind = c1
                                     };

                    #endregion

                    #region 获取数据

                    var inorder_query = from b in lqh.Umsdb.View_VIPService
                                        select b;
                    foreach (var m in param_join)
                    {
                        //此处出现问题拉
                        //if (m.ParamBehind == null)//不存在字段
                        //{
                        //    continue;
                        //}
                        //new Model.ReportSqlParams(){ParamName="InOrderID" },
                        //new Model.ReportSqlParams(){ParamName="Pro_HallID"},
                        //new Model.ReportSqlParams(){ParamName="OldID"}, 
                        //new Model.ReportSqlParams(){ParamName="UserName"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_start"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_end"},
                        //new Model.ReportSqlParams(){ParamName="Note"}

                        switch (m.ParamFront.ParamName)
                        {


                            case "ID":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.ID == int.Parse(mm0.ParamValues)
                                                    select b;
                                    break;
                                }

                            default: break;
                        }
                    }
                    #endregion

                    #region 排序

                    inorder_query = from b in inorder_query
                                    orderby b.SCount descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.View_VIPService> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.View_VIPService> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }
        #endregion
        /// <summary>
        /// 获取客户服务明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="vipid"></param>
        /// <returns></returns>
        public Model.WebReturn GetServiceByVIPID(Model.Sys_UserInfo user, int vipid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                     var results = from detail in lqh.Umsdb.View_VIPService
                                   where detail.VIPID==vipid
                                   select detail;


                     List<Model.View_VIPService> models = results.ToList();
                     return new Model.WebReturn() { ReturnValue=true,Obj=models};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue =false, Message=ex.Message};
                }
            }
        }
    }
}
