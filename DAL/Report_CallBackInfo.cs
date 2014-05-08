using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ReportModel;

namespace DAL
{
    public class Report_CallBackInfo
    {
        private string ReportViewName = "Report_CallBackInfo";
        public Expression<Func<ReportModel.Report_CallBackInfo, bool>> GetList(Model.Sys_UserInfo user, Entities currentDataSource)
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

            var objSet = currentDataSource.Report_CallBackInfo.AsQueryable();

            return o => true;
        }
    }
 }

