using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using ReportModel;

namespace DAL
{
    public class Report_邮储三方报表
    {
        private string ReportViewName = "Report_邮储三方报表";
        public Expression<Func<ReportModel.Report_邮储三方报表, bool>> GetList(Model.Sys_UserInfo user, Entities currentDataSource)
        {
            #region 权限
            IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
            //有权限的商品
            IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

            Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out  ValidHallIDS, out ValidProIDS, currentDataSource);

            if (ret.ReturnValue != true)
            {
                return o => false;
            }

            #endregion




            var objSet = currentDataSource.Report_邮储三方报表.AsQueryable();

            return
                o =>
                ((ValidHallIDS.Any(p => p.HallID == o.门店编码)));


        }
    }
}