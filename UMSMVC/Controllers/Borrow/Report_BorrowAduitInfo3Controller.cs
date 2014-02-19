
using Model;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Http;
using System.Web.Http;

using System.Data.Linq;


namespace UMSMVC.Controllers
{
    public class Report_BorrowAduitInfo3Controller : BasePage
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

                             where (b.二级已审批) == "Y"
                             && (b.已审批 != "Y" || b.已审批 == null)
                             && ((b.三级已审批) == null || b.三级已审批 != "Y")

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
                    AduitDate3=DateTime.Now,
                    AduitUser3=user.UserID,
                    Aduited3=true,
                    Note3=BrwAduit.三级备注,
                    Passed3=((BrwAduit.三级已通过+"").ToLower()=="true")
                };
                DAL.BorowAduit3 aduit = new DAL.BorowAduit3(Model.MethodIDListInfo.BorowAduit3_Aduit);
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
        public IQueryable<Model.Report_BorrowAduitListInfo> GetDetailList(int ID)
        {
            try
            {



                var imeis = (from b in context.Report_BorrowAduitListInfo
                             where b.系统外键 == ID
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
                             (b.二级已审批) == "Y"
                             && (b.已审批 != "Y" || b.已审批 == null)
                             && ((b.三级已审批) == null || b.三级已审批 != "Y")
                             )
                             select b);
                if (imeis.Count() == 0)
                {
                    imeis = (from b in context.Report_BorrowAduitInfo
                             orderby b.系统主键
                             where
                             (b.二级已审批) == "Y"
                             && (b.已审批 != "Y" || b.已审批 == null)
                             && ((b.三级已审批) == null || b.三级已审批 != "Y")

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
    }
}
