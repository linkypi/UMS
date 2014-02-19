using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UMSMVC.Controllers.Repair
{
    public class RepairStatisticsController : BasePage
    {
        //
        // GET: /RepairStatistics/

        /// <summary>
        /// 獲取未返庫數量
        /// </summary>
        [HttpGet]
        public void GetUnReturnCount()
        {
            try
            {
                this.webreturn.JsonReturnDict["response"] = context.RepairStatistics().ToList();

                this.OutputJson();
            }
            catch (Exception ex)
            {
                // return new Model.WebReturn() {ReturnValue=false,Message="数据异常！" };
            }
        }

        [HttpGet]
        public void GetDetail()
        {
            try
            {
                int min = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["min"]);
                int max = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["max"]);
                var list = context.GetRepairCountByScope(min, max).ToList();
                this.webreturn.JsonReturnDict["response"] = list;
                this.OutputJson();
            }
            catch (Exception ex)
            {
                // return new Model.WebReturn() {ReturnValue=false,Message="数据异常！" };
            }
        }
    }
}
