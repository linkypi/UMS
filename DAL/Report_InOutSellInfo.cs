using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Report_InOutSellInfo
    {
       public string ReportViewName = "Report_StoreInfo";
       private static Func<Model._UMSDB, IEnumerable<ReportModel.Report_InOutSellInfo>> _compiledQuery;
       public static Func<Model._UMSDB, IEnumerable<ReportModel.Report_InOutSellInfo>> GetQuery()
        {
            if (_compiledQuery == null)
            {

                _compiledQuery = CompiledQuery.Compile((Model._UMSDB db) =>
                       db.Report_InOutSellInfo.Select(
                           (p) => new ReportModel.Report_InOutSellInfo { 
                      
                           }
                       )


                    );
               
                 
            }
            return _compiledQuery;
        }

       /// <summary>
       /// 2013 5 24
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_InOutSellInfo> GetListNew(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {

           #region 权限
           IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
           //有权限的商品
           IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

           Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

           if (ret.ReturnValue != true)
           { return new List<ReportModel.Report_InOutSellInfo>().AsQueryable(); }

           #endregion


           var objSet = ObjectContext.Report_InOutSellInfo.AsQueryable();

           objSet = from b in objSet 
                            join c in ValidHallIDS
                            on b.门店编码 equals c.HallID
                            into temp2
                            from c1 in temp2 
                            select b;

                   //if(ValidProIDS.Count()>0)
                   objSet = from b in objSet 
                            join c in ValidProIDS
                            on b.类别编码 equals c.ClassID
                            into temp2
                            from c1 in temp2
                            select b;
             
           return objSet.OrderBy(p=>p.序号);


         

       }

       public static IQueryable<ReportModel.Report_InOutSellInfo> GetList(Model._UMSDB umsdb)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               //using (TransactionScope ts = new TransactionScope())
               //{
                
                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   //if (ret.ReturnValue != true)
                   //{ return new List<ReportModel.Report_InOutSellInfo>().AsQueryable(); }

                   #endregion


                   DateTime dt1=DateTime.Now.AddDays(-30);
                   DateTime dt2=DateTime.Now;
                  
                   #region 过滤仓库
                   //if (ValidHallIDS.Count() > 0)
                   var Store_ = from b in lqh.Umsdb.View_StoreWithNum select b;
                   #region 入库记录 InOrder_



                   var InOrder_ = from b in lqh.Umsdb.Pro_InOrderList
                                  group b by new { b.ProID, b.Pro_InOrder.Pro_HallID }
                                      into temp1
                                      select new
                                      {
                                          ProID = temp1.Key.ProID,
                                          HallID = temp1.Key.Pro_HallID,
                                          InCount_1 = temp1.Where(p => p.Pro_InOrder.InDate < dt1).Sum(p => p.ProCount),
                                          InCount_2 = temp1.Where(p => p.Pro_InOrder.InDate >= dt1 && p.Pro_InOrder.InDate <= dt2).Sum(p => p.ProCount),
                                          InCount_3 = temp1.Where(p => p.Pro_InOrder.InDate > dt2).Sum(p => p.ProCount)
                                      };
                   
                   #endregion
                   #region 退库 BackOrder_
                   var BackOrder_ = from b in lqh.Umsdb.Pro_BackListInfo
                                    
                                     group b by new { b.ProID, b.Pro_BackInfo.HallID }
                                         into temp1
                                         select new
                                         {
                                             ProID = temp1.Key.ProID,
                                             HallID = temp1.Key.HallID,
                                             BackCount_1 = temp1.Where(p => p.Pro_BackInfo.BackDate < dt1).Sum(p => p.ProCount),
                                             BackCount_2 = temp1.Where(p => p.Pro_BackInfo.BackDate >= dt1 && p.Pro_BackInfo.BackDate <= dt2).Sum(p => p.ProCount),
                                             BackCount_3 = temp1.Where(p => p.Pro_BackInfo.BackDate > dt2).Sum(p => p.ProCount)
                                      
                                         };
                   #endregion

                   #region 类别转入 转出
                   
                   #endregion
                   #region 调拨入库 OutInOrder_

                   var OutInOrder_ = from b in lqh.Umsdb.Pro_OutOrderList
                                     where b.Pro_OutInfo.IsDelete == false || b.Pro_OutInfo.IsDelete==null
                                     group b by new { b.ProID, b.Pro_OutInfo.Pro_HallID }
                                         into temp1
                                         select new
                                         {
                                             ProID = temp1.Key.ProID,
                                             HallID = temp1.Key.Pro_HallID,
                                             OutInCount_1 = temp1.Where(p => p.Pro_OutInfo.OutDate < dt1).Sum(p => p.ProCount),
                                             OutInCount_2 = temp1.Where(p => p.Pro_OutInfo.OutDate >= dt1 && p.Pro_OutInfo.OutDate <= dt2).Sum(p => p.ProCount),
                                             OutInCount_3 = temp1.Where(p => p.Pro_OutInfo.OutDate > dt2).Sum(p => p.ProCount)
                                      
                                         };

                   #endregion

                   #region 调拨出库 OutOrder_

                   var OutOrder_ = from b in lqh.Umsdb.Pro_OutOrderList
                                   where ( b.Pro_OutInfo.IsDelete == false || b.Pro_OutInfo.IsDelete == null)
                                   &&( b.Pro_OutInfo.Audit==true)
                                     group b by new { b.ProID, b.Pro_OutInfo.FromHallID }
                                         into temp1
                                         select new
                                         {
                                             ProID = temp1.Key.ProID,
                                             HallID = temp1.Key.FromHallID,
                                             OutCount_1 = temp1.Where(p => p.Pro_OutInfo.ToDate < dt1).Sum(p => p.ProCount),
                                             OutCount_2 = temp1.Where(p => p.Pro_OutInfo.ToDate >= dt1 && p.Pro_OutInfo.ToDate <= dt2).Sum(p => p.ProCount),
                                             OutCount_3 = temp1.Where(p => p.Pro_OutInfo.ToDate > dt2).Sum(p => p.ProCount)

                                         };

                   #endregion

                   #region 销售 SellOrder_

                   var SellOrder_ = from b in lqh.Umsdb.Pro_SellListInfo
                                    where b.Pro_SellInfo !=null && b.Pro_SellBack==null
                                   group b by new { b.ProID, b.Pro_SellInfo.HallID }
                                       into temp1
                                       select new
                                       {
                                           ProID = temp1.Key.ProID,
                                           HallID = temp1.Key.HallID,
                                           SellCount_1 = temp1.Where(p => p.Pro_SellInfo.SellDate < dt1).Sum(p => p.ProCount),
                                           SellCount_2 = temp1.Where(p => p.Pro_SellInfo.SellDate >= dt1 && p.Pro_SellInfo.SellDate <= dt2).Sum(p => p.ProCount),
                                           SellCount_3 = temp1.Where(p => p.Pro_SellInfo.SellDate > dt2).Sum(p => p.ProCount)

                                       };

                   #endregion
                   #region 换货 ChangeOrder_

                   var ChangeOrder_ = from b in lqh.Umsdb.Pro_SellListInfo
                                      where b.Pro_SellInfo == null && b.Pro_SellBack != null 
                                      && (b.OldSellListID==0  || b.OldSellListID==null)
                                      
                                    group b by new { b.ProID, b.Pro_SellBack.Pro_SellInfo.HallID }
                                        into temp1
                                        select new
                                        {
                                            ProID = temp1.Key.ProID,
                                            HallID = temp1.Key.HallID,
                                            ChangeCount_1 = temp1.Where(p => p.Pro_SellBack.UpdDate < dt1).Sum(p => p.ProCount),
                                            ChangeCount_2 = temp1.Where(p => p.Pro_SellBack.UpdDate >= dt1 && p.Pro_SellBack.UpdDate <= dt2).Sum(p => p.ProCount),
                                            ChangeCount_3 = temp1.Where(p => p.Pro_SellBack.UpdDate > dt2).Sum(p => p.ProCount)

                                        };

                   #endregion

                   #region 退货 SellBackOrder_

                   var SellBackOrder_ = from b in lqh.Umsdb.Pro_SellBackList
                                        where b.Pro_SellBack!=null
                                        group b by new { b.ProID, b.Pro_SellBack.Pro_SellInfo.HallID }
                                        into temp1
                                        select new
                                        {
                                            ProID = temp1.Key.ProID,
                                            HallID = temp1.Key.HallID,
                                            SellBackCount_1 = temp1.Where(p => p.Pro_SellBack.UpdDate < dt1).Sum(p => p.ProCount),
                                            SellBackCount_2 = temp1.Where(p => p.Pro_SellBack.UpdDate >= dt1 && p.Pro_SellBack.UpdDate <= dt2).Sum(p => p.ProCount),
                                            SellBackCount_3 = temp1.Where(p => p.Pro_SellBack.UpdDate > dt2).Sum(p => p.ProCount)

                                        };

                   #endregion
                   #region 送修 RepairOrder_

                   var RepairOrder_ = from b in lqh.Umsdb.Pro_RepairListInfo
                                        where b.Pro_RepairInfo != null 
                                        && (b.Pro_RepairInfo.IsDelete==false || b.Pro_RepairInfo.IsDelete==null)
                                      group b by new { b.ProID, b.Pro_RepairInfo.HallID }
                                            into temp1
                                            select new
                                            {
                                                ProID = temp1.Key.ProID,
                                                HallID = temp1.Key.HallID,
                                                RepairCount_1 = temp1.Where(p => p.Pro_RepairInfo.RepairDate < dt1).Sum(p => p.ProCount),
                                                RepairCount_2 = temp1.Where(p => p.Pro_RepairInfo.RepairDate >= dt1 && p.Pro_RepairInfo.RepairDate <= dt2).Sum(p => p.ProCount),
                                                RepairCount_3 = temp1.Where(p => p.Pro_RepairInfo.RepairDate > dt2).Sum(p => p.ProCount),
                                                RepairCount_4 = temp1.Sum(p => p.ProCount)

                                            };

                   #endregion
                   #region 送修返库 RepairedOrder_

                   var RepairedOrder_ = from b in lqh.Umsdb.Pro_RepairReturnListInfo
                                      where b.Pro_RepairReturnInfo != null
                                      &&(b.Pro_RepairReturnInfo.IsDelete==false || b.Pro_RepairReturnInfo.IsDelete==null)
                                        group b by new { b.ProID, b.Pro_RepairReturnInfo.HallID }
                                          into temp1
                                          select new
                                          {
                                              ProID = temp1.Key.ProID,
                                              HallID = temp1.Key.HallID,
                                              RepairReturnCount_1 = temp1.Where(p => p.Pro_RepairReturnInfo.RepairReturnDate < dt1).Sum(p => p.ProCount),
                                              RepairReturnCount_2 = temp1.Where(p => p.Pro_RepairReturnInfo.RepairReturnDate >= dt1 && p.Pro_RepairReturnInfo.RepairReturnDate <= dt2).Sum(p => p.ProCount),
                                              RepairReturnCount_3 = temp1.Where(p => p.Pro_RepairReturnInfo.RepairReturnDate > dt2).Sum(p => p.ProCount),
                                              RepairReturnCount_4 = temp1.Sum(p => p.ProCount)

                                          };

                   #endregion
                   #region 借贷 BorrowOrder_

                   var BorrowOrder_ = from b in lqh.Umsdb.Pro_BorowListInfo
                                        where b.Pro_BorowInfo != null
                                        && (b.Pro_BorowInfo.IsDelete == false || b.Pro_BorowInfo.IsDelete == null)
                                        group b by new { b.ProID, b.Pro_BorowInfo.HallID }
                                            into temp1
                                            select new
                                            {
                                                ProID = temp1.Key.ProID,
                                                HallID = temp1.Key.HallID,
                                                BorrowCount_1 = temp1.Where(p => p.Pro_BorowInfo.BorowDate < dt1).Sum(p => p.ProCount),
                                                BorrowCount_2 = temp1.Where(p => p.Pro_BorowInfo.BorowDate >= dt1 && p.Pro_BorowInfo.BorowDate <= dt2).Sum(p => p.ProCount),
                                                BorrowCount_3 = temp1.Where(p => p.Pro_BorowInfo.BorowDate > dt2).Sum(p => p.ProCount),
                                                BorrowCount_4 = temp1.Sum(p => p.ProCount)

                                            };

                   #endregion
                   #region 归还 ReturnOrder_

                   var ReturnOrder_ = from b in lqh.Umsdb.Pro_ReturnListInfo
                                      where b.Pro_ReturnInfo != null
                                      && (b.Pro_ReturnInfo.IsDelete == false || b.Pro_ReturnInfo.IsDelete == null)
                                      group b by new { b.ProID, b.Pro_ReturnInfo.Pro_BorowInfo.HallID }
                                          into temp1
                                          select new
                                          {
                                              ProID = temp1.Key.ProID,
                                              HallID = temp1.Key.HallID,
                                              BrwReturnCount_1 = temp1.Where(p => p.Pro_ReturnInfo.ReturnDate < dt1).Sum(p => p.ProCount),
                                              BrwReturnCount_2 = temp1.Where(p => p.Pro_ReturnInfo.ReturnDate >= dt1 && p.Pro_ReturnInfo.ReturnDate <= dt2).Sum(p => p.ProCount),
                                              BrwReturnCount_3 = temp1.Where(p => p.Pro_ReturnInfo.ReturnDate > dt2).Sum(p => p.ProCount),
                                              BrwReturnCount_4 = temp1.Sum(p => p.ProCount)

                                          };

                   #endregion

                   var objSet =from b in Store_ 
                               join c in InOrder_//入库
                               on new {b.ProID,b.HallID} 
                               equals new {c.ProID,c.HallID}
                               into temp1
                               from c1 in temp1.DefaultIfEmpty()
                               join d in BackOrder_//退库
                               on new {b.ProID,b.HallID} 
                               equals new {d.ProID,d.HallID}
                               into temp2
                               from d1 in temp2.DefaultIfEmpty()
                               join e in OutInOrder_//调入
                               on new {b.ProID,b.HallID} 
                               equals new {e.ProID,e.HallID}
                               into temp3
                               from e1 in temp3.DefaultIfEmpty()
                               join f in OutOrder_//调出
                               on new {b.ProID,b.HallID} 
                               equals new {f.ProID,f.HallID}
                               into temp4
                               from f1 in temp4.DefaultIfEmpty()
                               join g in SellOrder_//销售
                               on new {b.ProID,b.HallID} 
                               equals new {g.ProID,g.HallID}
                               into temp5
                               from g1 in temp5.DefaultIfEmpty()
                               join h in ChangeOrder_//换货
                               on new {b.ProID,b.HallID} 
                               equals new {h.ProID,h.HallID}
                               into temp6
                               from h1 in temp6.DefaultIfEmpty()
                               join i in BackOrder_//退货
                               on new {b.ProID,b.HallID} 
                               equals new {i.ProID,i.HallID}
                               into temp7
                               from i1 in temp7.DefaultIfEmpty()
                               join j in RepairOrder_//送修
                               on new {b.ProID,b.HallID} 
                               equals new {j.ProID,j.HallID}
                               into temp8
                               from j1 in temp8.DefaultIfEmpty()
                               join k in RepairedOrder_//返库
                               on new {b.ProID,b.HallID} 
                               equals new {k.ProID,k.HallID}
                               into temp9
                               from k1 in temp9.DefaultIfEmpty()
                               join l in BorrowOrder_//借贷
                               on new {b.ProID,b.HallID} 
                               equals new {l.ProID,l.HallID}
                               into temp10
                               from l1 in temp10.DefaultIfEmpty()
                               join m in ReturnOrder_//归还
                               on new {b.ProID,b.HallID} 
                               equals new {m.ProID,m.HallID}
                               into temp11
                               from m1 in temp11.DefaultIfEmpty()
                               select new ReportModel.Report_InOutSellInfo{
                                   序号=b.Num,
                                   类别=b.ClassName,
                                   品牌=b.TypeName,
                                   商品名称=b.ProName,
                                   商品属性=b.ProFormat,
                                   期初库存 = Convert.ToDecimal(c1.InCount_1) - Convert.ToDecimal(d1.BackCount_1)+
                                   Convert.ToDecimal(e1.OutInCount_1) - Convert.ToDecimal(f1.OutCount_1) -
                                   Convert.ToDecimal(g1.SellCount_1) - Convert.ToDecimal(h1.ChangeCount_1) +
                                   Convert.ToDecimal(i1.BackCount_1) - Convert.ToDecimal(j1.RepairCount_1)+
                                   Convert.ToDecimal(k1.RepairReturnCount_1) - Convert.ToDecimal(l1.BorrowCount_1)+
                                   Convert.ToDecimal(m1.BrwReturnCount_1),
                                   本期初始入库=Convert.ToDecimal(c1.InCount_2),
                                   本期退库 = Convert.ToDecimal(d1.BackCount_2),
                                   本期类别转入=Convert.ToDecimal(0.00),
                                   本期类别转出=Convert.ToDecimal(0.00),
                                   本期调入= Convert.ToDecimal(e1.OutInCount_2),
                                   本期调出 = Convert.ToDecimal(f1.OutCount_2),
                                   本期销售 = Convert.ToDecimal(g1.SellCount_2) + Convert.ToDecimal(h1.ChangeCount_2),
                                   本期退货 = Convert.ToDecimal(i1.BackCount_2 ),
                                   本期送修 = Convert.ToDecimal(j1.RepairCount_2),
                                   本期返库 = Convert.ToDecimal(k1.RepairReturnCount_2),
                                   本期借贷 = Convert.ToDecimal(l1.BorrowCount_2),
                                   本期归还 = Convert.ToDecimal(m1.BrwReturnCount_2),
                                   期末库存 = Convert.ToDecimal(b.ProCount) - Convert.ToDecimal(c1.InCount_3)+
                                   Convert.ToDecimal(d1.BackCount_3) - Convert.ToDecimal(e1.OutInCount_3) +
                                   Convert.ToDecimal(f1.OutCount_3) + Convert.ToDecimal(g1.SellCount_3)+
                                   Convert.ToDecimal(h1.ChangeCount_3) - Convert.ToDecimal(i1.BackCount_3)+
                                   Convert.ToDecimal(j1.RepairCount_3) - Convert.ToDecimal(k1.RepairReturnCount_3)+
                                   Convert.ToDecimal(l1.BorrowCount_3) - Convert.ToDecimal(m1.BrwReturnCount_3),
                                   送修累计=Convert.ToDecimal(j1.RepairCount_4)-Convert.ToDecimal(k1.RepairReturnCount_4),
                                   借贷累计 = Convert.ToDecimal(l1.BorrowCount_4) - Convert.ToDecimal(m1.BrwReturnCount_4),
                                   类别编码=b.Pro_ClassID,
                                   门店=b.HallName,
                                   门店编码=b.HallID,
                                   区域=b.AreaName

                               };

                 
               //objSet = from b in objSet
               //             //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
               //             join c in ValidHallIDS
               //             on b.门店编码 equals c.HallID
               //             into temp2
               //             from c1 in temp2
               //             //orderby b.SysDate descending
               //             select b;

               //    //if(ValidProIDS.Count()>0)
               //    objSet = from b in objSet
               //             //where ValidProIDS.Contains(b.商品编码) || string.IsNullOrEmpty(b.商品编码)
               //             join c in ValidProIDS
               //             on b.类别编码 equals c.ClassID
               //             into temp2
               //             select b;

                   #endregion 
                   return objSet;
            
               //}
           }
       }
       
       public IQueryable<ReportModel.Report_InOutSellInfo> GetList(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               //using (TransactionScope ts = new TransactionScope())
               //{
                
                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   //if (ret.ReturnValue != true)
                   //{ return new List<ReportModel.Report_InOutSellInfo>().AsQueryable(); }

                   #endregion


                   DateTime dt1=DateTime.Now.AddDays(-30);
                   DateTime dt2=DateTime.Now;
                  
                   #region 过滤仓库
                   //if (ValidHallIDS.Count() > 0)
                   var Store_ = from b in lqh.Umsdb.View_StoreWithNum select b;
                   #region 入库记录 InOrder_



                   var InOrder_ = from b in lqh.Umsdb.Pro_InOrderList
                                  group b by new { b.ProID, b.Pro_InOrder.Pro_HallID }
                                      into temp1
                                      select new
                                      {
                                          ProID = temp1.Key.ProID,
                                          HallID = temp1.Key.Pro_HallID,
                                          InCount_1 = temp1.Where(p => p.Pro_InOrder.InDate < dt1).Sum(p => p.ProCount),
                                          InCount_2 = temp1.Where(p => p.Pro_InOrder.InDate >= dt1 && p.Pro_InOrder.InDate <= dt2).Sum(p => p.ProCount),
                                          InCount_3 = temp1.Where(p => p.Pro_InOrder.InDate > dt2).Sum(p => p.ProCount)
                                      };
                   
                   #endregion
                   #region 退库 BackOrder_
                   var BackOrder_ = from b in lqh.Umsdb.Pro_BackListInfo
                                    
                                     group b by new { b.ProID, b.Pro_BackInfo.HallID }
                                         into temp1
                                         select new
                                         {
                                             ProID = temp1.Key.ProID,
                                             HallID = temp1.Key.HallID,
                                             BackCount_1 = temp1.Where(p => p.Pro_BackInfo.BackDate < dt1).Sum(p => p.ProCount),
                                             BackCount_2 = temp1.Where(p => p.Pro_BackInfo.BackDate >= dt1 && p.Pro_BackInfo.BackDate <= dt2).Sum(p => p.ProCount),
                                             BackCount_3 = temp1.Where(p => p.Pro_BackInfo.BackDate > dt2).Sum(p => p.ProCount)
                                      
                                         };
                   #endregion

                   #region 类别转入 转出
                   
                   #endregion
                   #region 调拨入库 OutInOrder_

                   var OutInOrder_ = from b in lqh.Umsdb.Pro_OutOrderList
                                     where b.Pro_OutInfo.IsDelete == false || b.Pro_OutInfo.IsDelete==null
                                     group b by new { b.ProID, b.Pro_OutInfo.Pro_HallID }
                                         into temp1
                                         select new
                                         {
                                             ProID = temp1.Key.ProID,
                                             HallID = temp1.Key.Pro_HallID,
                                             OutInCount_1 = temp1.Where(p => p.Pro_OutInfo.OutDate < dt1).Sum(p => p.ProCount),
                                             OutInCount_2 = temp1.Where(p => p.Pro_OutInfo.OutDate >= dt1 && p.Pro_OutInfo.OutDate <= dt2).Sum(p => p.ProCount),
                                             OutInCount_3 = temp1.Where(p => p.Pro_OutInfo.OutDate > dt2).Sum(p => p.ProCount)
                                      
                                         };

                   #endregion

                   #region 调拨出库 OutOrder_

                   var OutOrder_ = from b in lqh.Umsdb.Pro_OutOrderList
                                   where ( b.Pro_OutInfo.IsDelete == false || b.Pro_OutInfo.IsDelete == null)
                                   &&( b.Pro_OutInfo.Audit==true)
                                     group b by new { b.ProID, b.Pro_OutInfo.FromHallID }
                                         into temp1
                                         select new
                                         {
                                             ProID = temp1.Key.ProID,
                                             HallID = temp1.Key.FromHallID,
                                             OutCount_1 = temp1.Where(p => p.Pro_OutInfo.ToDate < dt1).Sum(p => p.ProCount),
                                             OutCount_2 = temp1.Where(p => p.Pro_OutInfo.ToDate >= dt1 && p.Pro_OutInfo.ToDate <= dt2).Sum(p => p.ProCount),
                                             OutCount_3 = temp1.Where(p => p.Pro_OutInfo.ToDate > dt2).Sum(p => p.ProCount)

                                         };

                   #endregion

                   #region 销售 SellOrder_

                   var SellOrder_ = from b in lqh.Umsdb.Pro_SellListInfo
                                    where b.Pro_SellInfo !=null && b.Pro_SellBack==null
                                   group b by new { b.ProID, b.Pro_SellInfo.HallID }
                                       into temp1
                                       select new
                                       {
                                           ProID = temp1.Key.ProID,
                                           HallID = temp1.Key.HallID,
                                           SellCount_1 = temp1.Where(p => p.Pro_SellInfo.SellDate < dt1).Sum(p => p.ProCount),
                                           SellCount_2 = temp1.Where(p => p.Pro_SellInfo.SellDate >= dt1 && p.Pro_SellInfo.SellDate <= dt2).Sum(p => p.ProCount),
                                           SellCount_3 = temp1.Where(p => p.Pro_SellInfo.SellDate > dt2).Sum(p => p.ProCount)

                                       };

                   #endregion
                   #region 换货 ChangeOrder_

                   var ChangeOrder_ = from b in lqh.Umsdb.Pro_SellListInfo
                                      where b.Pro_SellInfo == null && b.Pro_SellBack != null 
                                      && (b.OldSellListID==0  || b.OldSellListID==null)
                                      
                                    group b by new { b.ProID, b.Pro_SellBack.Pro_SellInfo.HallID }
                                        into temp1
                                        select new
                                        {
                                            ProID = temp1.Key.ProID,
                                            HallID = temp1.Key.HallID,
                                            ChangeCount_1 = temp1.Where(p => p.Pro_SellBack.UpdDate < dt1).Sum(p => p.ProCount),
                                            ChangeCount_2 = temp1.Where(p => p.Pro_SellBack.UpdDate >= dt1 && p.Pro_SellBack.UpdDate <= dt2).Sum(p => p.ProCount),
                                            ChangeCount_3 = temp1.Where(p => p.Pro_SellBack.UpdDate > dt2).Sum(p => p.ProCount)

                                        };

                   #endregion

                   #region 退货 SellBackOrder_

                   var SellBackOrder_ = from b in lqh.Umsdb.Pro_SellBackList
                                        where b.Pro_SellBack!=null
                                        group b by new { b.ProID, b.Pro_SellBack.Pro_SellInfo.HallID }
                                        into temp1
                                        select new
                                        {
                                            ProID = temp1.Key.ProID,
                                            HallID = temp1.Key.HallID,
                                            SellBackCount_1 = temp1.Where(p => p.Pro_SellBack.UpdDate < dt1).Sum(p => p.ProCount),
                                            SellBackCount_2 = temp1.Where(p => p.Pro_SellBack.UpdDate >= dt1 && p.Pro_SellBack.UpdDate <= dt2).Sum(p => p.ProCount),
                                            SellBackCount_3 = temp1.Where(p => p.Pro_SellBack.UpdDate > dt2).Sum(p => p.ProCount)

                                        };

                   #endregion
                   #region 送修 RepairOrder_

                   var RepairOrder_ = from b in lqh.Umsdb.Pro_RepairListInfo
                                        where b.Pro_RepairInfo != null 
                                        && (b.Pro_RepairInfo.IsDelete==false || b.Pro_RepairInfo.IsDelete==null)
                                      group b by new { b.ProID, b.Pro_RepairInfo.HallID }
                                            into temp1
                                            select new
                                            {
                                                ProID = temp1.Key.ProID,
                                                HallID = temp1.Key.HallID,
                                                RepairCount_1 = temp1.Where(p => p.Pro_RepairInfo.RepairDate < dt1).Sum(p => p.ProCount),
                                                RepairCount_2 = temp1.Where(p => p.Pro_RepairInfo.RepairDate >= dt1 && p.Pro_RepairInfo.RepairDate <= dt2).Sum(p => p.ProCount),
                                                RepairCount_3 = temp1.Where(p => p.Pro_RepairInfo.RepairDate > dt2).Sum(p => p.ProCount),
                                                RepairCount_4 = temp1.Sum(p => p.ProCount)

                                            };

                   #endregion
                   #region 送修返库 RepairedOrder_

                   var RepairedOrder_ = from b in lqh.Umsdb.Pro_RepairReturnListInfo
                                      where b.Pro_RepairReturnInfo != null
                                      &&(b.Pro_RepairReturnInfo.IsDelete==false || b.Pro_RepairReturnInfo.IsDelete==null)
                                        group b by new { b.ProID, b.Pro_RepairReturnInfo.HallID }
                                          into temp1
                                          select new
                                          {
                                              ProID = temp1.Key.ProID,
                                              HallID = temp1.Key.HallID,
                                              RepairReturnCount_1 = temp1.Where(p => p.Pro_RepairReturnInfo.RepairReturnDate < dt1).Sum(p => p.ProCount),
                                              RepairReturnCount_2 = temp1.Where(p => p.Pro_RepairReturnInfo.RepairReturnDate >= dt1 && p.Pro_RepairReturnInfo.RepairReturnDate <= dt2).Sum(p => p.ProCount),
                                              RepairReturnCount_3 = temp1.Where(p => p.Pro_RepairReturnInfo.RepairReturnDate > dt2).Sum(p => p.ProCount),
                                              RepairReturnCount_4 = temp1.Sum(p => p.ProCount)

                                          };

                   #endregion
                   #region 借贷 BorrowOrder_

                   var BorrowOrder_ = from b in lqh.Umsdb.Pro_BorowListInfo
                                        where b.Pro_BorowInfo != null
                                        && (b.Pro_BorowInfo.IsDelete == false || b.Pro_BorowInfo.IsDelete == null)
                                        group b by new { b.ProID, b.Pro_BorowInfo.HallID }
                                            into temp1
                                            select new
                                            {
                                                ProID = temp1.Key.ProID,
                                                HallID = temp1.Key.HallID,
                                                BorrowCount_1 = temp1.Where(p => p.Pro_BorowInfo.BorowDate < dt1).Sum(p => p.ProCount),
                                                BorrowCount_2 = temp1.Where(p => p.Pro_BorowInfo.BorowDate >= dt1 && p.Pro_BorowInfo.BorowDate <= dt2).Sum(p => p.ProCount),
                                                BorrowCount_3 = temp1.Where(p => p.Pro_BorowInfo.BorowDate > dt2).Sum(p => p.ProCount),
                                                BorrowCount_4 = temp1.Sum(p => p.ProCount)

                                            };

                   #endregion
                   #region 归还 ReturnOrder_

                   var ReturnOrder_ = from b in lqh.Umsdb.Pro_ReturnListInfo
                                      where b.Pro_ReturnInfo != null
                                      && (b.Pro_ReturnInfo.IsDelete == false || b.Pro_ReturnInfo.IsDelete == null)
                                      group b by new { b.ProID, b.Pro_ReturnInfo.Pro_BorowInfo.HallID }
                                          into temp1
                                          select new
                                          {
                                              ProID = temp1.Key.ProID,
                                              HallID = temp1.Key.HallID,
                                              BrwReturnCount_1 = temp1.Where(p => p.Pro_ReturnInfo.ReturnDate < dt1).Sum(p => p.ProCount),
                                              BrwReturnCount_2 = temp1.Where(p => p.Pro_ReturnInfo.ReturnDate >= dt1 && p.Pro_ReturnInfo.ReturnDate <= dt2).Sum(p => p.ProCount),
                                              BrwReturnCount_3 = temp1.Where(p => p.Pro_ReturnInfo.ReturnDate > dt2).Sum(p => p.ProCount),
                                              BrwReturnCount_4 = temp1.Sum(p => p.ProCount)

                                          };

                   #endregion

                   var objSet =from b in Store_ 
                               join c in InOrder_//入库
                               on 1 equals 1
                               into temp1
                               from c1 in temp1.DefaultIfEmpty()
                               join d in BackOrder_//退库
                               on new {b.ProID,b.HallID} 
                               equals new {d.ProID,d.HallID}
                               into temp2
                               from d1 in temp2.DefaultIfEmpty()
                               join e in OutInOrder_//调入
                               on 1 equals 1
                               into temp3
                               from e1 in temp3.DefaultIfEmpty()
                               join f in OutOrder_//调出
                               on 1 equals 1
                               into temp4
                               from f1 in temp4.DefaultIfEmpty()
                               join g in SellOrder_//销售
                               on new {b.ProID,b.HallID} 
                               equals new {g.ProID,g.HallID}
                               into temp5
                               from g1 in temp5.DefaultIfEmpty()
                               join h in ChangeOrder_//换货
                               on new {b.ProID,b.HallID} 
                               equals new {h.ProID,h.HallID}
                               into temp6
                               from h1 in temp6.DefaultIfEmpty()
                               join i in BackOrder_//退货
                               on new {b.ProID,b.HallID} 
                               equals new {i.ProID,i.HallID}
                               into temp7
                               from i1 in temp7.DefaultIfEmpty()
                               join j in RepairOrder_//送修
                               on new {b.ProID,b.HallID} 
                               equals new {j.ProID,j.HallID}
                               into temp8
                               from j1 in temp8.DefaultIfEmpty()
                               join k in RepairedOrder_//返库
                               on new {b.ProID,b.HallID} 
                               equals new {k.ProID,k.HallID}
                               into temp9
                               from k1 in temp9.DefaultIfEmpty()
                               join l in BorrowOrder_//借贷
                               on new {b.ProID,b.HallID} 
                               equals new {l.ProID,l.HallID}
                               into temp10
                               from l1 in temp10.DefaultIfEmpty()
                               join m in ReturnOrder_//归还
                               on new {b.ProID,b.HallID} 
                               equals new {m.ProID,m.HallID}
                               into temp11
                               from m1 in temp11.DefaultIfEmpty()
                               select new ReportModel.Report_InOutSellInfo{
                                   序号=b.Num,
                                   类别=b.ClassName,
                                   品牌=b.TypeName,
                                   商品名称=b.ProName,
                                   商品属性=b.ProFormat,
                                   期初库存 = Convert.ToDecimal(c1.InCount_1) - Convert.ToDecimal(d1.BackCount_1)+
                                   Convert.ToDecimal(e1.OutInCount_1) - Convert.ToDecimal(f1.OutCount_1) -
                                   Convert.ToDecimal(g1.SellCount_1) - Convert.ToDecimal(h1.ChangeCount_1) +
                                   Convert.ToDecimal(i1.BackCount_1) - Convert.ToDecimal(j1.RepairCount_1)+
                                   Convert.ToDecimal(k1.RepairReturnCount_1) - Convert.ToDecimal(l1.BorrowCount_1)+
                                   Convert.ToDecimal(m1.BrwReturnCount_1),
                                   本期初始入库=Convert.ToDecimal(c1.InCount_2),
                                   本期退库 = Convert.ToDecimal(d1.BackCount_2),
                                   本期类别转入=Convert.ToDecimal(0.00),
                                   本期类别转出=Convert.ToDecimal(0.00),
                                   本期调入= Convert.ToDecimal(e1.OutInCount_2),
                                   本期调出 = Convert.ToDecimal(f1.OutCount_2),
                                   本期销售 = Convert.ToDecimal(g1.SellCount_2) + Convert.ToDecimal(h1.ChangeCount_2),
                                   本期退货 = Convert.ToDecimal(i1.BackCount_2 ),
                                   本期送修 = Convert.ToDecimal(j1.RepairCount_2),
                                   本期返库 = Convert.ToDecimal(k1.RepairReturnCount_2),
                                   本期借贷 = Convert.ToDecimal(l1.BorrowCount_2),
                                   本期归还 = Convert.ToDecimal(m1.BrwReturnCount_2),
                                   期末库存 = Convert.ToDecimal(b.ProCount) - Convert.ToDecimal(c1.InCount_3)+
                                   Convert.ToDecimal(d1.BackCount_3) - Convert.ToDecimal(e1.OutInCount_3) +
                                   Convert.ToDecimal(f1.OutCount_3) + Convert.ToDecimal(g1.SellCount_3)+
                                   Convert.ToDecimal(h1.ChangeCount_3) - Convert.ToDecimal(i1.BackCount_3)+
                                   Convert.ToDecimal(j1.RepairCount_3) - Convert.ToDecimal(k1.RepairReturnCount_3)+
                                   Convert.ToDecimal(l1.BorrowCount_3) - Convert.ToDecimal(m1.BrwReturnCount_3),
                                   送修累计=Convert.ToDecimal(j1.RepairCount_4)-Convert.ToDecimal(k1.RepairReturnCount_4),
                                   借贷累计 = Convert.ToDecimal(l1.BorrowCount_4) - Convert.ToDecimal(m1.BrwReturnCount_4),
                                   类别编码=b.Pro_ClassID,
                                   门店=b.HallName,
                                   门店编码=b.HallID,
                                   区域=b.AreaName

                               };

              
               //objSet = from b in objSet
               //             //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
               //             join c in ValidHallIDS
               //             on b.门店编码 equals c.HallID
               //             into temp2
               //             from c1 in temp2
               //             //orderby b.SysDate descending
               //             select b;

               //    //if(ValidProIDS.Count()>0)
               //    objSet = from b in objSet
               //             //where ValidProIDS.Contains(b.商品编码) || string.IsNullOrEmpty(b.商品编码)
               //             join c in ValidProIDS
               //             on b.类别编码 equals c.ClassID
               //             into temp2
               //             select b;

                   #endregion 
                   return objSet;
            
               //}
           }
       }
       


       private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="类别" },
            new Model.ReportSqlParams_String(){ParamName="品牌"},
            new Model.ReportSqlParams_String(){ParamName="商品名称"}, 
            new Model.ReportSqlParams_String(){ParamName="门店"},
            new Model.ReportSqlParams_String(){ParamName="区域"}
        };
        private int _MethodID;

        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }

        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }

        public Report_InOutSellInfo()
        {
        
        }
        public Report_InOutSellInfo(int MethodID)
        {
            this.MethodID = MethodID;
        }
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam,DateTime dt1,DateTime dt2)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                try
                {

                    #region 权限
                    IQueryable<Model.Pro_HallInfo> ValidHallIDS =null;
                    //有权限的商品
                    IQueryable<Model.Pro_ClassInfo> ValidProIDS =null;

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, out ValidHallIDS,out ValidProIDS,lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize != 20)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
                    #region 将传入的参数与指定的参数了左连接

                    var param_join = from b in pageParam.ParamList
                                     join c in this.ParamList
                                     on new { b.ParamName, t = b.GetType() }
                                     equals
                                     new { c.ParamName, t = c.GetType() }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     select new
                                     {
                                         ParamFront = b,
                                         ParamBehind = c1
                                     };

                    #endregion

                    #region 获取数据

                    var x = lqh.Umsdb.GetInOutSellInfo(dt1, dt2,this.ReportViewName,user.RoleID);
                    var x2 = lqh.Umsdb.GetInOutSellInfo(dt1, dt2, this.ReportViewName, user.RoleID);
                    var inorder_query = from b in x
                                        join c in ValidHallIDS
                                        on b.门店编码 equals c.HallID
                                        into temp1
                                        from c1 in temp1
                                        join d in ValidProIDS
                                        on b.类别编码 equals d.ClassID
                                        into temp2
                                        from d1 in temp2
                                        select b;
                    var inorder_query2 = from b in x2
                                        join c in ValidHallIDS
                                        on b.门店编码 equals c.HallID
                                        into temp1
                                        from c1 in temp1
                                        join d in ValidProIDS
                                        on b.类别编码 equals d.ClassID
                                        into temp2
                                        from d1 in temp2
                                        select b;
                    foreach (var m in param_join)
                    {
                        if (m.ParamBehind == null)//不存在字段
                        {
                            continue;
                        }
             

                        switch (m.ParamFront.ParamName)
                        {
                            case "类别":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.类别.Contains(mm.ParamValues)
                                                    select b;
                                    inorder_query2 = from b in inorder_query2
                                                    where b.类别.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "品牌":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm2.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.品牌.Contains(mm2.ParamValues)
                                                    select b;
                                    inorder_query2 = from b in inorder_query2
                                                    where b.品牌.Contains(mm2.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "商品名称":
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm3.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.商品名称.Contains(mm3.ParamValues)
                                                    select b;
                                    inorder_query2 = from b in inorder_query2
                                                    where b.商品名称.Contains(mm3.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "门店":
                                Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm4.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.门店.Contains(mm4.ParamValues)
                                                    select b;
                                    inorder_query2 = from b in inorder_query2
                                                    where b.门店.Contains(mm4.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "区域":
                                Model.ReportSqlParams_String mm5 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm5.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.区域.Contains(mm5.ParamValues)
                                                    select b;
                                    inorder_query2 = from b in inorder_query2
                                                    where b.区域.Contains(mm5.ParamValues)
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }

                    #endregion

                    
                    pageParam.RecordCount = inorder_query2.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.GetInOutSellInfoResult> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.GetInOutSellInfoResult> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }

        /// <summary>
        /// 获取入库记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetExportList(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam,DateTime dt1,DateTime dt2)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                try
                {

                    #region 权限
                    IQueryable<Model.Pro_HallInfo> ValidHallIDS = null;
                    //有权限的商品
                    IQueryable<Model.Pro_ClassInfo> ValidProIDS = null;

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, out ValidHallIDS, out ValidProIDS, lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
                    #region 将传入的参数与指定的参数了左连接

                    var param_join = from b in pageParam.ParamList
                                     join c in this.ParamList
                                     on new { b.ParamName, t = b.GetType() }
                                     equals
                                     new { c.ParamName, t = c.GetType() }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     select new
                                     {
                                         ParamFront = b,
                                         ParamBehind = c1
                                     };

                    #endregion

                    #region 获取数据

                    var x = lqh.Umsdb.GetInOutSellInfo(dt1, dt2,this.ReportViewName,user.RoleID);
                  
                    var inorder_query = from b in x
                                        join c in ValidHallIDS
                                        on b.门店编码 equals c.HallID
                                        into temp1
                                        from c1 in temp1
                                        join d in ValidProIDS
                                        on b.类别编码 equals d.ClassID
                                        into temp2
                                        from d1 in temp2
                                        select b;
                    
                    foreach (var m in param_join)
                    {
                        if (m.ParamBehind == null)//不存在字段
                        {
                            continue;
                        }


                        switch (m.ParamFront.ParamName)
                        {
                            case "类别":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.类别.Contains(mm.ParamValues)
                                                    select b;
                                   
                                    break;
                                }
                            case "品牌":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm2.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.品牌.Contains(mm2.ParamValues)
                                                    select b;
                                   
                                    break;
                                }
                            case "商品名称":
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm3.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.商品名称.Contains(mm3.ParamValues)
                                                    select b;
                                    
                                    break;
                                }
                            case "门店":
                                Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm4.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.门店.Contains(mm4.ParamValues)
                                                    select b;
                                     
                                    break;
                                }
                            case "区域":
                                Model.ReportSqlParams_String mm5 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm5.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.区域.Contains(mm5.ParamValues)
                                                    select b;
                                    
                                    break;
                                }
                            default: break;
                        }
                    }

                    #endregion


                    

                    #region 判断是否超过总页数

                    List<Model.GetInOutSellInfoResult> list = inorder_query.ToList();
                    pageParam.Obj = list;
                    return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }
    }
}
