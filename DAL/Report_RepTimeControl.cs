using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ReportModel;

namespace DAL
{
   public  class Report_RepTimeControl
    {
       private string ReportViewName = "Report_RepTimeControl";
       public Expression<Func<ReportModel.Report_RepTimeControl, bool>> GetList(Model.Sys_UserInfo user, Entities currentDataSource)
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

            var objSet = currentDataSource.Report_RepTimeControl.AsQueryable();

            return o => true;
        }
    }
}
