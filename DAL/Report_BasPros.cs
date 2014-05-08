using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ReportModel;

namespace DAL
{
    public  class Report_BasPros
    {
        private string ReportViewName = "Report_BasPros";
        public Expression<Func<ReportModel.Report_BadPros, bool>> GetList(Model.Sys_UserInfo user, Entities currentDataSource)
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
          //return  o =>
          //   ((ValidHallIDS.Any(p => p.HallID == )) &&
          //    ValidProIDS.Any(p => p.ClassID == o.类别编码));
                   return o => ((ValidHallIDS.Any(p =>  p.HallName==o.网点)));
            //var objSet = currentDataSource.Report_BasPros.AsQueryable();

            //return o => true;
        }
    }
}
