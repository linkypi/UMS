

using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Http;
using System.Web.Http;

using System.Data.Linq;



namespace UMSMVC.Controllers
{
    public class View_Pro_SellBackAduitController : BasePage
    {
        [CustomQueryableAttribute]
        public IQueryable<Model.View_Pro_SellBackAduit> Get()
        {
            try
            {

                var imeis = (from b in context.View_Pro_SellBackAduit
                             where (b.Aduited) == null || b.Aduited != "是"
                             select b);

                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.View_Pro_SellBackAduit>().AsQueryable();

            }
           
        }

        [CustomQueryableAttribute]
        public IQueryable<Model.View_ProSellBackAduitDetail> GetDetailList(int id)
        {
            try
            {

                var imeis = (from b in context.View_ProSellBackAduitDetail
                             where b.AduitID==id
                             select b);

                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.View_ProSellBackAduitDetail>().AsQueryable();

            }

        }

        [HttpPost]
        public void GetView_SellBackUnAduitFirst(Model.View_Pro_SellBackAduit BrwAduit)
        {
            try
            {


                Model.Pro_SellBackAduit brw = new Model.Pro_SellBackAduit()
                {
                    ID = BrwAduit.ID,
                    AduitDate = DateTime.Now,
                    AduitUser = user.UserID,
                    Aduited = true,
                    Note = BrwAduit.SellNote,
                    AduitMoney =Convert.ToDecimal( BrwAduit.ApplyMoney),
                    Passed = ((BrwAduit.Passed + "").ToLower() == "true")
                };
                DAL.Pro_SellBackAduit aduit = new DAL.Pro_SellBackAduit(Model.MethodIDListInfo.Pro_SellBack_Aduit);
                Model.WebReturn r = aduit.Aduit(user, brw );
                //return imeis.AsQueryable();
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
        public IQueryable<Model.View_Pro_SellBackAduit> GetView_BorowUnAduitFirstNext(int ID)
        {
            try
            {



                var imeis = (from b in context.View_Pro_SellBackAduit
                             orderby b.ID
                             where b.ID > ID && ((b.Aduited) == null || b.Aduited != "是")
                             select b);
                if (imeis.Count() == 0)
                {
                    imeis = (from b in context.View_Pro_SellBackAduit
                             orderby b.ID
                             where ((b.Aduited) == null || b.Aduited != "是")
                             select b);
                }
                imeis = imeis.Take(1);
                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.View_Pro_SellBackAduit>().AsQueryable();

            }
        }
  
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: /View_BorowAduit/
         
    }
}
