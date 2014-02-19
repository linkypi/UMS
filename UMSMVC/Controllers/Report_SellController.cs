using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace UMSMVC.Controllers
{
    public class Report_SellController : BasePage
    {
        //
        // GET: /Report_Sell/
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpGet]
        public void Report()
        {

            var querystring = Request.RequestUri.ParseQueryString();
            var areaid = querystring["areaid"];
            var startdate = querystring["sdate"];
            var enddate = querystring["edate"];

            DateTime sdate;
            DateTime edate;
            if (!(DateTime.TryParse(startdate, out sdate) && DateTime.TryParse(enddate, out edate)))
            {
                OutputJson();
                return;
            }

            var hallid = querystring["hallid"];
            var mquery = context.Chart_MapReport.Where(p => p.DATE > sdate && p.DATE < edate);
            if (string.IsNullOrEmpty(hallid))
            {
                if (string.IsNullOrEmpty(areaid))
                {
                    var query =
                        mquery.GroupBy(p => new { p.AreaName, p.AreaID })
                            .Select(
                                q =>
                                    new
                                    {
                                        AreaName = q.Key.AreaName,
                                        AreaID = q.Key.AreaID,
                                        Sum = q.Sum(p => p.SellPrice)
                                    });

                    this.webreturn.JsonReturnDict["response"] = query;
                }
                else
                {
                    var query =
                        mquery.Where(e => e.AreaID == int.Parse(areaid))
                            .GroupBy(p => new {p.HallID, p.HallName})
                            .Select(
                                q =>
                                    new
                                    {
                                        AreaName = q.Key.HallName,
                                        AreaID = q.Key.HallID,
                                        Sum = q.Sum(p => p.SellPrice)
                                    });

                    this.webreturn.JsonReturnDict["response"] = query;
                }
            }
            else
            {
                var query =
                        mquery.Where(e => e.HallID == hallid)
                            .GroupBy(p => new { p.TypeName })
                            .Select(
                                q =>
                                    new
                                    {
                                        AreaName = q.Key.TypeName,
                                        AreaID = q.Key.TypeName,
                                        Sum = q.Sum(p => p.SellPrice)
                                    });

                this.webreturn.JsonReturnDict["response"] = query;
            }


            OutputJson();
             


         }

    }
}
