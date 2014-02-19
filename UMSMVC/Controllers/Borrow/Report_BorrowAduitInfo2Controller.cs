
using Model;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Http;

using System.Data.Linq;
using System.Web.Http;


namespace UMSMVC.Controllers
{
    public class Report_BorrowAduitInfo2Controller : BasePage
    {
        //
        // GET: /View_BorowAduit/
        [CustomQueryableAttribute]
        public IQueryable<Model.Report_BorrowAduitInfo> Get()
        {
            try
            {

                var imeis = (from b in context.Report_BorrowAduitInfo
                             orderby b.系统主键

                             where (b.一级已审批) == "Y"
                             && (b.已审批 != "Y" || b.已审批 == null)
                             && ((b.二级已审批) == null || b.二级已审批 != "Y")

                             select b);

                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.Report_BorrowAduitInfo>().AsQueryable();

            }
        }
        [HttpPost]
        public void GetView_BorowUnAduitFirst(Model.Report_BorrowAduitInfo BrwAduit)
        {
            try
            {
               

                Model.Pro_BorowAduit brw = new Pro_BorowAduit() { 
                    ID=BrwAduit.系统主键,
                    AduitDate2=DateTime.Now,
                    AduitUser2=user.UserID,
                    Aduited2=true,
                    Note2=BrwAduit.二级备注,
                    Passed2=((BrwAduit.二级已通过+"").ToLower()=="true")
                };
                DAL.BorowAduit2 aduit = new DAL.BorowAduit2(Model.MethodIDListInfo.BorowAduit2_Aduit);
                Model.WebReturn r= aduit.Aduit(user, new List<Pro_BorowAduit>() { brw });
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
        public IQueryable<Model.Report_BorrowAduitListInfo> GetDetailList(int id)
        {
            try
            {

                var imeis = (from b in context.Report_BorrowAduitListInfo
                             where b.系统外键 == id
                             select b);

                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.Report_BorrowAduitListInfo>().AsQueryable();

            }

        }
        [CustomQueryableAttribute]
        public IQueryable<Model.Report_BorrowAduitInfo> GetView_BorowUnAduitFirstNext(int ID)
        {
            try
            {

                var imeis = (from b in context.Report_BorrowAduitInfo
                             orderby b.系统主键
                             where b.系统主键 > ID
                             && (
                             (b.一级已审批) == "Y"
                             && (b.已审批 != "Y" || b.已审批 == null)
                             && ((b.二级已审批) == null || b.二级已审批 != "Y")
                             )
                             select b);
                if (imeis.Count() == 0)
                {
                    imeis = (from b in context.Report_BorrowAduitInfo
                             orderby b.系统主键
                             where
                             (b.一级已审批) == "Y"
                             && (b.已审批 != "Y" || b.已审批 == null)
                             && ((b.二级已审批) == null || b.二级已审批 != "Y")

                             select b);
                }
                imeis = imeis.Take(1);
                return imeis.AsQueryable();

            }
            catch (Exception ex)
            {
                return new List<Model.Report_BorrowAduitInfo>().AsQueryable();

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
    }
}
