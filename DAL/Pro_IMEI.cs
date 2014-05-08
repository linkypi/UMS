using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using Model;

namespace DAL
{
    public class Pro_IMEI_Utils
    {

        private List<Model.Pro_RepairReturnListInfo> allrepairs;
        private List<Model.Pro_BorowInfo> allborows;

        public Pro_IMEI_Utils()
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {DataLoadOptions dl=new DataLoadOptions();
                dl.LoadWith<Model.Pro_InOrder>(p=>p.Pro_HallInfo);
                dl.LoadWith<Model.Pro_InOrderList>(p => p.Pro_InOrder);
                dl.LoadWith<Model.Pro_RepairReturnInfo>(p=>p.Pro_RepairInfo);
                dl.LoadWith<Model.Pro_RepairReturnListInfo>(p => p.Pro_RepairReturnInfo);
                dl.LoadWith<Model.Pro_OutInfo>(p=>p.Pro_HallInfo);
                dl.LoadWith<Model.Pro_OutOrderList>(p => p.Pro_OutInfo);
                dl.LoadWith<Model.Pro_OutOrderIMEI>(p => p.Pro_OutOrderList);
                
                dl.LoadWith<Model.Pro_BorowInfo>(p=>p.Pro_BorowListInfo);
                dl.LoadWith<Model.Pro_BorowListInfo>(p => p.Pro_BorowOrderIMEI);
                dl.LoadWith<Model.Pro_BorowInfo>(p => p.Pro_ReturnInfo);
                dl.LoadWith<Model.Pro_ReturnInfo>(p => p.Pro_ReturnListInfo);
                dl.LoadWith<Model.Pro_ReturnListInfo>(p => p.Pro_ReturnOrderIMEI);

                lqh.Umsdb.LoadOptions = dl;
                allrepairs = lqh.Umsdb.Pro_RepairReturnListInfo.ToList();
                allborows = lqh.Umsdb.Pro_BorowInfo.ToList();

            }
        }
        public Model.Pro_InOrderList getInitorder(Model.Pro_InOrderList inlist)
        {
            if (inlist.InitInListID == inlist.InListID)
            {
                return inlist;
            }
            else
            {
                return getInitorder(inlist.InitInList);
            }
        }

        /// <summary>
        /// 獲得該串碼在指定日期是否正在借貸
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool GetIMEIBorrowStatus(string imei,DateTime date)
        {
            var q=allborows.Where(p => p.Pro_BorowListInfo.Any(o => o.Pro_BorowOrderIMEI.Select(i => i.IMEI).Contains(imei))).Where(p=>p.BorowDate<date).OrderByDescending(p=>p.BorowDate);
            if (q.Any())
            {
                var m = q.First();
                var q2 =
                    m.Pro_ReturnInfo.Where(
                        p => p.Pro_ReturnListInfo.Any(o => o.Pro_ReturnOrderIMEI.Select(i => i.IMEI).Contains(imei)))
                        .Where(p => p.ReturnDate < date);
                if (q2.Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public void updateAreaAge(int id)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions dl=new DataLoadOptions();
                dl.LoadWith<Model.Pro_InOrder>(p=>p.Pro_HallInfo);
                dl.LoadWith<Model.Pro_InOrderList>(p => p.Pro_InOrder);
                dl.LoadWith<Model.Pro_RepairReturnInfo>(p=>p.Pro_RepairInfo);
                dl.LoadWith<Model.Pro_RepairReturnListInfo>(p => p.Pro_RepairReturnInfo);
                dl.LoadWith<Model.Pro_OutInfo>(p=>p.Pro_HallInfo);
                dl.LoadWith<Model.Pro_OutOrderList>(p => p.Pro_OutInfo);
                dl.LoadWith<Model.Pro_OutOrderIMEI>(p => p.Pro_OutOrderList);
                


                lqh.Umsdb.LoadOptions = dl;
//                DateTime d=new DateTime(2000,1,1);
//                d.Add(new DateTime(2013, 1, 1) - new DateTime(2012, 12, 1));
                


                var b=lqh.Umsdb.Pro_RepairReturnListInfo.Select(p => p.OLD_IMEI).ToList();
                b.AddRange(lqh.Umsdb.Pro_RepairReturnListInfo.Select(p=>p.NEW_IMEI));
                b = b.Distinct().ToList();

                var allproinorderimei = lqh.Umsdb.Pro_InOrderIMEI.ToList();
                var alltouorderimei = lqh.Umsdb.Pro_OutOrderIMEI.ToList();

//                using (var f = File.AppendText(HttpContext.Current.Server.MapPath(".") + @"\TEST.log"))
//                {
                foreach (var proImei in lqh.Umsdb.Pro_IMEI.Where(p => p.ID > id))
                {
                    try
                    {
                        Console.WriteLine(proImei.ID);
                        proImei.AreaAgeDelta = new DateTime(2000, 1, 1);
                        proImei.AreaAgeInitDate = null;
                        proImei.OutRecDate = null;

                        var q = GetIMEIRepairs(proImei.IMEI);
                        q =
                            q.GroupBy(o => o.RepairReturnListID)
                                .Select(i => i.First())
                                .OrderBy(p => p.Pro_RepairReturnInfo.ID)
                                .ToList();
                        var oldimeis = q.Select(p => p.OLD_IMEI).ToList();
                        oldimeis.AddRange(q.Select(p => p.NEW_IMEI));
                        oldimeis.Add(proImei.IMEI);
                        oldimeis = oldimeis.Distinct().ToList();

                        var inorderquery = allproinorderimei.Where(p => oldimeis.Contains(p.IMEI));
                        if (inorderquery.Any())
                        {
                            var m = inorderquery.OrderBy(p => p.Pro_InOrderList.Pro_InOrderID).First();
                            var initinlist = getInitorder(m.Pro_InOrderList);
                            if (initinlist.Pro_InOrder.Pro_HallID != "1")
                            {
                                proImei.AreaAgeInitDate = initinlist.Pro_InOrder.InDate.Value;
                            }
                        }
                        if (!proImei.AreaAgeInitDate.HasValue || (proImei.AreaAgeInitDate > (new DateTime(2014, 3, 1))))
                        {
                            if (GetIMEIBorrowStatus(proImei.IMEI, new DateTime(2014, 3, 1)))
                            {
                                proImei.AreaAgeInitDate = new DateTime(2014, 3, 1);
                            }
                        }
                        var outorderquery =
                            alltouorderimei.Where(p => oldimeis.Contains(p.IMEI))
                                .OrderBy(o => o.Pro_OutOrderList.Pro_OutInfo.ID);
                        if (outorderquery.Any())
                        {
                            foreach (var proOutOrderImei in outorderquery)
                            {
                                if (proOutOrderImei.Pro_OutOrderList.Pro_OutInfo.Pro_HallInfo.IsAreaAge)
                                {
                                    if (!proImei.AreaAgeInitDate.HasValue)
                                    {
                                        proImei.AreaAgeInitDate = proOutOrderImei.Pro_OutOrderList.Pro_OutInfo.ToDate;
                                    }

                                    if (proImei.OutRecDate.HasValue)
                                    {
                                        proImei.AreaAgeDelta =
                                            proImei.AreaAgeDelta.Value.Add(
                                                proOutOrderImei.Pro_OutOrderList.Pro_OutInfo.ToDate.Value -
                                                proImei.OutRecDate.Value);
                                    }
                                }
                                if (proImei.OutRecDate.HasValue)
                                {
                                    if (proOutOrderImei.Pro_OutOrderList.Pro_OutInfo.Pro_HallInfo.IsAreaAge)
                                    {

                                        proImei.OutRecDate = null;
                                    }
                                    else
                                    {

                                        proImei.OutRecDate = proOutOrderImei.Pro_OutOrderList.Pro_OutInfo.ToDate.Value;


                                    }
                                }
                            }
                        }
                        if (q.Any())
                        {
                            foreach (var proRepairReturnListInfo in q)
                            {
                                if (proRepairReturnListInfo.Pro_RepairReturnInfo.IsReceived == true)
                                {
                                    if (proRepairReturnListInfo.Pro_RepairReturnInfo.Pro_HallInfo.IsAreaAge)
                                    {
                                        proImei.AreaAgeDelta =
                                            proImei.AreaAgeDelta.Value.Add(
                                                proRepairReturnListInfo.Pro_RepairReturnInfo.SysDate.Value -
                                                proRepairReturnListInfo.Pro_RepairReturnInfo.Pro_RepairInfo.RecvTime
                                                    .Value);


                                    }
                                }
                            }
                        }

                        lqh.Umsdb.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                lqh.Umsdb.SubmitChanges();
              
//                foreach (var proImei in lqh.Umsdb.Pro_IMEI.Where(p=>b.Contains(p.IMEI)))
//                {
//                    var q = GetIMEIRepairs(proImei.IMEI);
//                    q = q.GroupBy(o => o.RepairReturnListID).Select(i => i.First()).OrderBy(p => p.Pro_RepairReturnInfo.ID).ToList();
////                    f.WriteLine("IMEI:"+proImei.IMEI);
////                    foreach (var proRepairReturnListInfo in q)
////                    {
////                        f.Write(proRepairReturnListInfo.RepairReturnListID+":");
////                        f.Write(proRepairReturnListInfo.OLD_IMEI+"");
////                        f.Write("=>");
////                        f.WriteLine(proRepairReturnListInfo.NEW_IMEI+"");
////                    }
//                    foreach (var proRepairReturnListInfo in q)
//                    {
//                        if (!proImei.AreaAgeDelta.HasValue || proImei.AreaAgeDelta < new DateTime(2000, 1, 1))
//                        {
//                            proImei.AreaAgeDelta = new DateTime(2000, 1, 1);
//                        }
//                        proImei.AreaAgeDelta =
//                            proImei.AreaAgeDelta.Value.Add(proRepairReturnListInfo.Pro_RepairReturnInfo.RecvTime.Value -
//                                                     proRepairReturnListInfo.Pro_RepairReturnInfo.Pro_RepairInfo
//                                                         .RecvTime.Value);
//                    }
//                }

//                }
                //lqh.Umsdb.Pro_IMEI

            }


        }
        /// <summary>
        /// 获得该串码没有变更串码的送修
        /// </summary>
        /// <param name="imeiModel"></param>
        /// <returns></returns>
        public List<Model.Pro_RepairReturnListInfo> GetRepairNochangeIMEIList(string IMEI)
        {
            List<Model.Pro_RepairReturnListInfo> results = new List<Model.Pro_RepairReturnListInfo>();


            results.AddRange(allrepairs.Where(p => p.OLD_IMEI == IMEI && (p.NEW_IMEI == null || p.NEW_IMEI == "")).Distinct());
                return results;

            
        }
        /// <summary>
        /// 获得变更到该串码的送修
        /// </summary>
        /// <param name="IMEI"></param>
        /// <returns></returns>
        public List<Model.Pro_RepairReturnListInfo> GetRepairChangeToIMEI(string IMEI)
        {
            List<Model.Pro_RepairReturnListInfo> results = new List<Model.Pro_RepairReturnListInfo>();

             results.AddRange(allrepairs.Where(p => p.NEW_IMEI == IMEI));
                return results;

           
        }

        public List<Model.Pro_RepairReturnListInfo> GetIMEIRepairs(string IMEI)
        {
            List<Model.Pro_RepairReturnListInfo> results = new List<Model.Pro_RepairReturnListInfo>();
            results.AddRange(GetRepairNochangeIMEIList(IMEI));
            List<Model.Pro_RepairReturnListInfo> RepTemp = GetRepairChangeToIMEI(IMEI);
            while (RepTemp.Count>0)
            {
                results.AddRange(RepTemp);
                List<Model.Pro_RepairReturnListInfo> temp=new List<Model.Pro_RepairReturnListInfo>();
                foreach (var proRepairReturnInfo in RepTemp)
                {

                    temp.AddRange(GetRepairChangeToIMEI(proRepairReturnInfo.OLD_IMEI));
                    results.AddRange(GetIMEIRepairs(proRepairReturnInfo.OLD_IMEI));
                }
                results.AddRange(temp);
                RepTemp = temp;



            }

            return results;
        }

    }
}