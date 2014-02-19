
using Model;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Http;
using System.Web.Http;

using System.Data.Linq;
using System.Threading.Tasks;


namespace UMSMVC.Controllers
{
    public class View_VIPOffListAduitHeaderController : BasePage
    {
         
        // GET: /View_BorowAduit/
        [CustomQueryableAttribute]
        public IQueryable<ReportModel.View_VIPOffListAduitHeader> Get()
        {
            try
            {


                DAL.AllReportInfo_Model all_report = new DAL.AllReportInfo_Model();
                var imeis = all_report.GetList_SellOffAduit(user, Model.MenuIDListInfo.Pro_SellOff_Aduit);

                imeis = (from b in imeis
                         where (b.Aduited1) == null || b.Aduited1 != "是"
                         select b);

                return imeis.AsQueryable();


               
            }
            catch (Exception ex)
            {
                return new List<ReportModel.View_VIPOffListAduitHeader>().AsQueryable();

            }
        }
     
        [HttpPost]
        public void GetSellOffUnAduitFirst()
        {
            try
            {
                int ID = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["ID"]);
                bool passed1 = Convert.ToBoolean(System.Web.HttpContext.Current.Request.Form["passed1"]);
                string Note1 = System.Web.HttpContext.Current.Request.Form["Note1"] + "";
                
                DAL.VIP_OffList aduit = new DAL.VIP_OffList(Model.MethodIDListInfo.Pro_SellOff_Aduit1);
                Model.WebReturn r = aduit.Aduit(user, ID, passed1, Note1);
                ////return imeis.AsQueryable();
                webreturn.JsonReturnDict["error"] = (r.ReturnValue == true ? 0 : 1);
                webreturn.JsonReturnDict["msg"] = r.Message;
                this.OutputJson();
            }
            catch (Exception ex)
            {
                //return new List<Model.Report_BorrowAduitInfo>().AsQueryable();
                webreturn.JsonReturnDict["error"] = 1;
                webreturn.JsonReturnDict["msg"] = "系统错误," + ex.Message;
                this.OutputJson();

            }
        }


        [CustomQueryableAttribute]
        public IQueryable<ReportModel.View_VIPOffListAduitHeader> GetSellOffAduitFirstNext(int ID)
        {
            try
            {
                DAL.AllReportInfo_Model all_report = new DAL.AllReportInfo_Model();
                var imeis = all_report.GetList_SellOffAduit(user, Model.MenuIDListInfo.Pro_SellOff_Aduit);





                imeis = (from b in imeis
                             orderby b.ID
                         where b.ID > ID && ((b.Aduited1) == null || b.Aduited1 != "是")
                             select b);
                if (imeis.Count() == 0)
                {
                    imeis = (from b in imeis
                             orderby b.ID
                             where ((b.Aduited1) == null || b.Aduited1 != "是")
                             select b);
                }
                imeis = imeis.Take(1);
                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<ReportModel.View_VIPOffListAduitHeader>().AsQueryable();

            }
        }
    }
}
