using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class View_Pro_InOrder
    {
        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="InOrderID" },
            new Model.ReportSqlParams_String(){ParamName="Pro_HallID"},
            new Model.ReportSqlParams_String(){ParamName="ProName"},
            new Model.ReportSqlParams_String(){ParamName="OldID"}, 
            new Model.ReportSqlParams_String(){ParamName="UserName"},
            new Model.ReportSqlParams_DataTime(){ParamName="SysDate_start"},
            new Model.ReportSqlParams_DataTime(){ParamName="SysDate_end"},
            new Model.ReportSqlParams_String(){ParamName="Note"}
        };
        private int _MethodID;

        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }

        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }

        public View_Pro_InOrder()
        {
        
        }
        public View_Pro_InOrder(int MethodID)
        {
            this.MethodID = MethodID;
        }
        /// <summary>
        /// 获取入库记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user,Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                    try
                    {
                      
                        #region "验证用户操作仓库  商品的权限 "
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        //if (ValidHallIDS.Count > 0 && !ValidHallIDS.Contains(model.HallID))
                        //{
                        //    var que = from h in lqh.Umsdb.Pro_HallInfo
                        //              where h.HallID == model.HallID
                        //              select h;
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                        //}

                        #endregion

                        if(pageParam==null || pageParam.PageIndex <0 || pageParam.PageSize !=20 )
                        {
                            return new Model.WebReturn(){ ReturnValue=false, Message="参数错误"};
                        }
                        if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
                        #region 将传入的参数与指定的参数了左连接

                        var param_join = from b in pageParam.ParamList
                                         join c in this.ParamList
                                         on new { b.ParamName,t=b.GetType() }
                                         equals
                                         new { c.ParamName,t=c.GetType() }
                                         into temp1
                                         from c1 in temp1.DefaultIfEmpty()
                                         select new
                                         {
                                             ParamFront = b,
                                             ParamBehind = c1
                                         };

                        #endregion

                        #region 获取数据

                        var inorder_query = from b in lqh.Umsdb.View_InOrderView
                                            select b;
                        foreach (var m in param_join)
                        {
                            if (m.ParamBehind == null)//不存在字段
                            {
                                continue;
                            }
                            //new Model.ReportSqlParams(){ParamName="InOrderID" },
                            //new Model.ReportSqlParams(){ParamName="Pro_HallID"},
                            //new Model.ReportSqlParams(){ParamName="OldID"}, 
                            //new Model.ReportSqlParams(){ParamName="UserName"},
                            //new Model.ReportSqlParams(){ParamName="SysDate_start"},
                            //new Model.ReportSqlParams(){ParamName="SysDate_end"},
                            //new Model.ReportSqlParams(){ParamName="Note"}

                            switch (m.ParamFront.ParamName)
                            {
                                case "InOrderID":
                                    Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                    if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.InOrderID.Contains(mm.ParamValues)
                                                        select b;
                                        break;
                                    }
                                case "Pro_HallID":
                                    Model.ReportSqlParams_ListString mm2 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                    if (mm2.ParamValues==null || mm2.ParamValues.Count()==0)
                                        break;
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where mm2.ParamValues.Contains(b.Pro_HallID)
                                                        select b;
                                        break;
                                    }
                                case "OldID":
                                    Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)m.ParamFront;
                                    if (string.IsNullOrEmpty(mm3.ParamValues))
                                        break;
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OldID.Contains(mm3.ParamValues)
                                                        select b;
                                        break;
                                    }
                                case "UserName":
                                    Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                   if (string.IsNullOrEmpty(mm4.ParamValues))
                                        break;
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where mm4.ParamValues.Contains(b.UserID)
                                                        select b;
                                        break;
                                    }
                                case "SysDate_start":
                                   Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                   if (mm5.ParamValues == null)
                                        break;
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.SysDate>=mm5.ParamValues
                                                        select b;
                                        break;
                                    }
                                case "SysDate_end":
                                   Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                   if (mm6.ParamValues == null)
                                       break;
                                   else
                                   {
                                       inorder_query = from b in inorder_query
                                                       where b.SysDate <= mm6.ParamValues
                                                       select b;
                                       break;
                                   }
                                //case "Note":
                                //   Model.ReportSqlParams_String mm7 = (Model.ReportSqlParams_String)m.ParamFront;
                                //   if (string.IsNullOrEmpty(mm7.ParamValues))
                                //       break;
                                //   else
                                //   {
                                //       inorder_query = from b in inorder_query
                                //                       where b.Note.Contains(mm7.ParamValues)
                                //                       select b;
                                //       break;
                                //   }
                                default: break;
                            }
                        }

                        #endregion

                        #region 过滤仓库 
                        if(ValidHallIDS.Count()>0)
                            inorder_query = from b in inorder_query
                                            where ValidHallIDS.Contains(b.Pro_HallID)
                                            orderby b.SysDate descending
                                            select b;
                        
                        else 
                            inorder_query = from b in inorder_query
                                        orderby b.SysDate descending
                                        select b;
                        #endregion
                        pageParam.RecordCount = inorder_query.Count();

                        #region 判断是否超过总页数

                        int pagecount = pageParam.RecordCount / pageParam.PageSize;

                        if (pageParam.PageIndex > pagecount)
                        {
                            pageParam.PageIndex = 0;
                            List<Model.View_InOrderView> list = inorder_query.Take(pageParam.PageSize).ToList();
                            pageParam.Obj = list;
                            return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                        }

                        else
                        {
                            //pageParam.PageIndex = 0;
                            List<Model.View_InOrderView> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
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
        /// <summary>
        /// 获取入库记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetExportList(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                try
                {

                    #region 权限
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null )
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

                    var inorder_query = from b in lqh.Umsdb.View_Pro_InOrder
                                        select b;
                    foreach (var m in param_join)
                    {
                        if (m.ParamBehind == null)//不存在字段
                        {
                            continue;
                        }
                        //new Model.ReportSqlParams(){ParamName="InOrderID" },
                        //new Model.ReportSqlParams(){ParamName="Pro_HallID"},
                        //new Model.ReportSqlParams(){ParamName="OldID"}, 
                        //new Model.ReportSqlParams(){ParamName="UserName"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_start"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_end"},
                        //new Model.ReportSqlParams(){ParamName="Note"}

                        switch (m.ParamFront.ParamName)
                        {
                            case "InOrderID":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.InOrderID.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "Pro_HallID":
                                Model.ReportSqlParams_ListString mm2 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm2.ParamValues == null || mm2.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm2.ParamValues.Contains(b.Pro_HallID)
                                                    select b;
                                    break;
                                }
                            case "OldID":
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm3.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OldID.Contains(mm3.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "UserName":
                                Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm4.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm4.ParamValues.Contains(b.UserName)
                                                    select b;
                                    break;
                                }
                            case "SysDate_start":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm5.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.SysDate >= mm5.ParamValues
                                                    select b;
                                    break;
                                }
                            case "SysDate_end":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm6.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.SysDate <= mm6.ParamValues
                                                    select b;
                                    break;
                                }
                            case "Note":
                                Model.ReportSqlParams_String mm7 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm7.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Note.Contains(mm7.ParamValues)
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }

                    #endregion

                    #region 过滤仓库
                    if (ValidHallIDS.Count() > 0)
                        inorder_query = from b in inorder_query
                                        where ValidHallIDS.Contains(b.Pro_HallID)
                                        orderby b.SysDate descending
                                        select b;

                    else
                        inorder_query = from b in inorder_query
                                        orderby b.SysDate descending
                                        select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                        //pageParam.PageIndex = 0;
                    List<Model.View_Pro_InOrder> list = inorder_query.ToList();
                    pageParam.Obj = list;
                    return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }
    }
}
