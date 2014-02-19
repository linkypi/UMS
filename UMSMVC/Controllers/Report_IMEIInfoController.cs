
using Model;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Http;
using System.Web.Http;

using System.Data.Linq;


namespace UMSMVC.Controllers
{
    public class Report_IMEIInfoController : BasePage
    {
        //[Queryable(AllowedQueryOptions=AllowedQueryOptions.All)]
        //public IQueryable<Model.View_BorowAduit> Get()
        //{
        //    try
        //    {


        //        var imeis = (from b in context.View_BorowAduit
        //                     select b).ToList();

        //        return imeis.AsQueryable();

        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<Model.View_BorowAduit>().AsQueryable();

        //    }
        //}

 
        [HttpGet]
        public WebReturn Getdata()
        {
            string data = @"[{
                          'year':'2008',
                          'microsoft':'20000',
                          'ibm':'15000',
                          'yahoo':'12000'
                        },
                        {
                          'year':'2009',
                          'microsoft':'18600',
                          'ibm':'13050',
                          'yahoo':'10100'
                        },
                        {
                          'year':'2010',
                          'microsoft':'15050',
                          'ibm':'16050',
                          'yahoo':'11040'
                        },
                        {
                          'year':'2011',
                          'microsoft':'20500',
                          'ibm':'15900',
                          'yahoo':'10500'
                        },
                        {
                          'year':'2012',
                          'microsoft':'19000',
                          'ibm':'16500',
                          'yahoo':'13000'
                        },
                        {
                          'year':'2013',
                          'microsoft':'20200',
                          'ibm':'17100',
                          'yahoo':'18000'
                        }]";
       
            
             return new WebReturn() { Obj = data};
        }

        //
        // GET: /Report_IMEIInfo/
        [HttpGet]
        public WebReturn Index()
        {
            return new Model.WebReturn() { Message = "测试" , ReturnValue = false };
        }


        /// <summary>
        /// 串码查询
        /// </summary>
        /// <param name="name"></param>
        [HttpGet]
        public void FindIMEIOne(string name)
        {
            try
            {
                var imeis = from b in context.Report_IMEIInfo
                            where b.串码.ToLower() == (name + "").ToLower()
                            select b;
                if (imeis.Count() == 0)
                {
                    webreturn.JsonReturnDict["error"] = 1;
                    webreturn.JsonReturnDict["msg"] = "串码不存在";

                }
                else
                {
                    webreturn.JsonReturnDict["error"] = 0;
                    webreturn.JsonReturnDict["msg"] = "查询成功";
                    webreturn.JsonReturnDict["response"] = imeis.Take(100);
                }
                this.OutputJson();
                //return Json(new { Data = imeis.Take(100), Total=1000 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                webreturn.JsonReturnDict["error"] = 1;
                webreturn.JsonReturnDict["msg"] = "串码不存在";
                //return null;
                this.OutputJson();
            }
            
        }

         
       

        /// <summary>
        /// 串码跟踪
        /// </summary>
        /// <param name="name"></param>
        [HttpGet]
        public void TrackIMEIOne(string name)
        {
            try
            {
                var imeis = from b in context.Report_IMEITracksInfo
                            where b.串码.ToLower() == (name + "").ToLower()
                            select b;
                if (imeis.Count() == 0)
                {
                    webreturn.JsonReturnDict["error"] = 1;
                    webreturn.JsonReturnDict["msg"] = "串码不存在";

                }
                else
                {
                    webreturn.JsonReturnDict["error"] = 0;
                    webreturn.JsonReturnDict["msg"] = "查询成功";
                    webreturn.JsonReturnDict["response"] = imeis;
                }
                this.OutputJson();

            }
            catch (Exception ex)
            {
                webreturn.JsonReturnDict["error"] = 1;
                webreturn.JsonReturnDict["msg"] = "串码不存在";

                this.OutputJson();
            }

        }
        [HttpPost]
        public void FindIMEIOne(Model.Report_IMEIInfo imeis)
        {
            try
            {

                //return Json(new Model.WebReturn() { Message = "", ReturnValue = false }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //return Json(new Model.WebReturn() { Message = "" + ex.Message, ReturnValue = false }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
