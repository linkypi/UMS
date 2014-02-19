using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Http;
using System.Web.Mvc;
using APIModel.Models;
using System.Collections;



namespace UMSMVC.Controllers.Borrow
{
    public class Report_BorrowAduitInfoController : BasePage
    {
        
        // GET: /Report_BorrowAduitInfo/
        [CustomQueryableAttribute]
        public IQueryable<Model.Report_BorrowAduitInfo> Get()
        {
            Model.UMSDB t = new Model.UMSDB();
            return t.Report_BorrowAduitInfo.AsQueryable();
            //return _db.Report_BorrowAduitInfo;
        }
        public Model.Report_BorrowAduitInfo GetModel(int id)
        {
            Model.UMSDB t = new Model.UMSDB();
            return t.Report_BorrowAduitInfo.First(p=>p.序号==id);
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
