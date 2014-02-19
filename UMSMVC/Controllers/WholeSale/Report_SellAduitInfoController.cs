
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
    public class Report_SellAduitInfoController : BasePage
    {
         
        // GET: /View_BorowAduit/
        [CustomQueryableAttribute]
        public IQueryable<Model.Report_SellAduitInfo> Get()
        {
            try
            {


                DAL.AllReportInfo_Model all_report = new DAL.AllReportInfo_Model();
                var imeis = all_report.GetList_SellAduit(user, Model.MenuIDListInfo.Pro_SellAduit_Aduit);

                imeis = (from b in imeis
                         where (b.一级已审批) == null || b.一级已审批 != "Y"
                         select b);

                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.Report_SellAduitInfo>().AsQueryable();

            }
        }
        [HttpPost]
        public void GetSellAduitUnAduitFirst(Model.Report_SellAduitInfo BrwAduit)
        {
            try
            {


                Model.Pro_SellAduit brw = new Pro_SellAduit()
                { 
                    ID=BrwAduit.系统主键,
                    AduitDate=DateTime.Now,
                    AduitUser=user.UserID,
                    Aduited1=true,
                    Note1=BrwAduit.一级备注,
                    Passed1=((BrwAduit.一级已通过+"").ToLower()=="true")
                };
                DAL.Pro_SellAduit aduit = new DAL.Pro_SellAduit(Model.MethodIDListInfo.Pro_SellAduit_Aduit);
                Model.WebReturn r= aduit.Aduit(user, new List<Model.Pro_SellAduit>() { brw },false);
                //return imeis.AsQueryable();
                webreturn.JsonReturnDict["error"] =(r.ReturnValue==true?0:1);
                webreturn.JsonReturnDict["msg"] = r.Message;
                this.OutputJson();
            }
            catch (Exception ex)
            {
                //return new List<Model.Report_BorrowAduitInfo>().AsQueryable();
                webreturn.JsonReturnDict["error"] = 1;
                webreturn.JsonReturnDict["msg"] ="系统错误,"+ex.Message;
                this.OutputJson();

            }
        }

        [CustomQueryableAttribute]
        public IQueryable<Model.Report_SellAduitListInfo> GetSellAduitDetail(int ID)
        {
            try
            {



                var imeis = (from b in context.Report_SellAduitListInfo
                             where b.系统外键 == ID
                             select b);

                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.Report_SellAduitListInfo>().AsQueryable();

            }
        }
        [CustomQueryableAttribute]
        public IQueryable<Model.Report_SellAduitInfo> GetSellAduitAduitFirstNext(int ID)
        {
            try
            {



                var imeis = (from b in context.Report_SellAduitInfo
                             orderby b.系统主键
                             where b.系统主键 > ID && ((b.一级已审批) == null || b.一级已审批 != "Y")
                             select b);
                if (imeis.Count() == 0)
                {
                    imeis = (from b in context.Report_SellAduitInfo
                             orderby b.系统主键
                             where ((b.一级已审批) == null || b.一级已审批 != "Y")
                             select b);
                }
                imeis = imeis.Take(1);
                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.Report_SellAduitInfo>().AsQueryable();

            }
        }
    }
}
