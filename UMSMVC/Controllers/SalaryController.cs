using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UMSMVC.Controllers
{
    public class SalaryController : BasePage
    {
        //
        // GET: /Salary/

        /// <summary>
        /// 获取各个仓库总提成
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <returns></returns>
        [HttpGet]
        public void GetEveryHallSalary()
        {
            try
            {
                string sdate = System.Web.HttpContext.Current.Request.QueryString["sdate"];
                string edate = System.Web.HttpContext.Current.Request.QueryString["edate"];
                edate = edate == "" ? DateTime.Now.ToShortDateString() : edate;
                string hid = @"107,109, 120, 121,122, 126,
                                133,135,136, 137, 139,
                                143,144,146,149,
                                153,156,157,169,
                                171,172, 58,59,62,
                                174,175,176,177,178,
                                180,  52,  54,  55, 56,
                                64,74,76,80,83,87,CK201311150001";
                var list = from a in context.EveryHallTotalSalary(sdate,edate,hid).ToList()
                              // where a.TotalSalary>0
                              select a;
                this.webreturn.JsonReturnDict["response"]= list;
                this.OutputJson();
            }
            catch (Exception ex)
            {
               // return new Model.WebReturn() {ReturnValue=false,Message="数据异常！" };
            }
        }

        [HttpGet]
        public void GetEverySalary()
        {
            try
            {
                string sdate = System.Web.HttpContext.Current.Request.QueryString["sdate"];
                string edate = System.Web.HttpContext.Current.Request.QueryString["edate"];
                string hid = System.Web.HttpContext.Current.Request.QueryString["hid"];
                edate = edate == "" ? DateTime.Now.ToShortDateString() : edate;

                var list = from a in context.PersonSalaryInHall(sdate, edate, hid).ToList()
                           select a;
               
                this.webreturn.JsonReturnDict["response"] = list;
                this.OutputJson();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
