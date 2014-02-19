using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 区域
    /// </summary>
    public class Demo_ReportViewInfo : Sys_InitParentInfo
    {
        private int _MethodID;
        //private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Demo_ReportViewInfo()
        {
            this.MethodID = 0;
        }

        public Demo_ReportViewInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        //private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
        //    new Model.ReportSqlParams_String(){ParamName="ID" },
        //    new Model.ReportSqlParams_String(){ParamName="Name"},
        //    new Model.ReportSqlParams_String(){ParamName="Flag"}, 
        //};
        //public List<Model.ReportSqlParams> ParamList
        //{
        //    get { return _paramList; }
        //    set { _paramList = value; }
        //}
 
        public Model.WebReturn GetModel(int Reportid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Demo_ReportViewInfo>(c => c.Demo_ReportViewColumnInfo);
                    dataload.AssociateWith<Model.Demo_ReportViewInfo>(c => c.Demo_ReportViewColumnInfo.OrderBy(x => x.OrderBy));
                    lqh.Umsdb.LoadOptions = dataload;

                    var query_area = from b in lqh.Umsdb.Demo_ReportViewInfo 
                                     where b.ID==Reportid
                                     select b;
                    if (query_area.Count() == 0)
                    {
                        return new Model.WebReturn(){ ReturnValue=false , Message="报表不存在"};
                    }
                    return new Model.WebReturn() { Obj = query_area.First(), ReturnValue=true};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Message = "服务器出错", ReturnValue = true };
                }
            }
        }
        public Model.WebReturn GetModel(string ReportName)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Demo_ReportViewInfo>(c => c.Demo_ReportViewColumnInfo);
                    dataload.AssociateWith<Model.Demo_ReportViewInfo>(c => c.Demo_ReportViewColumnInfo.OrderBy(x=>x.OrderBy));
                    lqh.Umsdb.LoadOptions = dataload;

                    var query_area = from b in lqh.Umsdb.Demo_ReportViewInfo
                                     where b.ReportViewName == ReportName
                                     select b;
                    if (query_area.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "报表不存在" };
                    }
                    return new Model.WebReturn() { Obj = query_area.First(), ReturnValue = true };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Message = "服务器出错", ReturnValue = true };
                }
            }
        }
    }
}
