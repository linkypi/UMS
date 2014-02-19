using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class SelectInlist
    {
        
        private int MenthodID;

	    public SelectInlist()
	    {
		    this.MenthodID = 0;
	    }

        public SelectInlist(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }

        #region "借贷归还拣货'

        /// <summary>
        /// 借贷归还拣货
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imeilist"></param>
        /// <param name="models1"></param>
        /// <returns></returns>
        public Model.WebReturn CheckReturn(Model.Sys_UserInfo user,int borowid,List<string> imeilist ,List<Model.NoIMEIModel> models1)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                  bool flag = false;
                 List<Model.SetSelection> list = new List<Model.SetSelection>();
               
                  ArrayList arr = new ArrayList();
                  if (imeilist.Count != 0)
                  {
                      list =  CheckIMEIReturn(borowid, imeilist, ref flag);
                      arr.Add(flag);
                  }
                  else
                  {
                      arr.Add(false);
                      flag = true;
                  }

                  arr.Add(list);

                  bool flag2 = false;
                  list = new List<Model.SetSelection>();
                  if (models1.Count() != 0)
                  {
                      list.Clear();
                      foreach (var item in models1)
                      {
                          list.AddRange(CheckNoIMEIReturn(borowid, item.ProID, item.ProCount, ref flag2));
                          flag = flag && flag2;
                      }
                      arr.Add(flag);
                  }
                  else
                  {
                      arr.Add(false);
                      flag2 = true;
                  }
                  flag = flag && flag2;

                  arr.Add(list);
                  return new Model.WebReturn() { ReturnValue= flag,ArrList=arr};
            }
        }

        /// <summary>
        /// 有串码归还拣货 
        /// </summary>
        /// <param name="imeiList"></param>
        /// <returns></returns>
        private List<Model.SetSelection> CheckIMEIReturn(int borowid,List<string> listStr,ref  bool flag)
        {
            if (listStr.Count == 0)
            {
                return new List<Model.SetSelection>();
            }
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                List<string> imeiList = new List<string>();
                //foreach (var item in listStr)
                //{
                //    imeiList.Add(item.OldIMEI);
                //}

                flag = true;
                var qIMEI = from blist in lqh.Umsdb.GetTable<Model.Pro_BorowListInfo>()
                            join b in lqh.Umsdb.GetTable<Model.Pro_BorowInfo>() on blist.BorowID equals borowid
                            join bo in lqh.Umsdb.GetTable<Model.Pro_BorowOrderIMEI>() on blist.BorowListID equals bo.BorowListID
                            where  imeiList.Contains(bo.IMEI) 
                            select new 
                            {
                                IMEI = bo.IMEI,
                                ProID = blist.ProID,
                                ProCount = 1,
                                InListID = blist.InListID,
                                BorowListID = blist.BorowListID
                            };

                if (qIMEI.Count() == 0)
                {
                    flag = false;
                    return new List<Model.SetSelection>() ;
                }

                List<Model.SetSelection> sets = new List<Model.SetSelection>();
                Model.SetSelection selec = null;
           

                int count = imeiList.Count;
                foreach (var gp in qIMEI)
                {
                     selec = new Model.SetSelection();
                     if (selec.ReturnIMEI == null)
                     {
                         selec.ReturnIMEI = new List<string>();
                     }
                     selec.ReturnIMEI.Add(gp.IMEI);
                     selec.Proid = gp.ProID;
                     selec.Countnum =Convert.ToDecimal(gp.ProCount.ToString());
                     selec.BorowListID = gp.BorowListID;
                     selec.InList = gp.InListID;
                     sets.Add(selec);
                     count--;
                     if (count == 0)
                     {
                         break;
                     }
              }
                if (count != 0)
                {
                    flag = false;
                }
                return sets;
            }
        }

        /// <summary>
        /// 无串码归还拣货 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<Model.SetSelection> CheckNoIMEIReturn(int borowid,string pid, decimal  count,ref  bool sucess)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                sucess = true;
                var qIMEI = from bo in lqh.Umsdb.GetTable<Model.Pro_BorowListInfo>()
                            join b in lqh.Umsdb.GetTable<Model.Pro_BorowInfo>() on bo.BorowID equals b.ID
                            where bo.ProID == pid && b.ID == borowid orderby bo.BorowListID ascending
                            select bo;

                if (qIMEI.Count() == 0)
                {
                    return new List<Model.SetSelection>() ;
                }

                List<Model.SetSelection> selections = new List<Model.SetSelection>();
                Model.SetSelection selec = null;
              
                foreach (var gp in qIMEI)
                {
                    selec = new Model.SetSelection();
                    selec.Proid = pid;
                    selec.InList = gp.InListID;
                    selec.BorowListID = gp.BorowListID;
                    if (count <= gp.ProCount)
                    {
                        selec.Countnum = count;
                        selections.Add(selec);
                        count = 0;
                        break;
                    }
                    selec.Countnum =Convert.ToDecimal( gp.ProCount);
                    
                    count -= selec.Countnum;
                    selections.Add(selec);
                }
                if (count > 0)
                {
                    sucess = false;
                }
                return selections;
            }
        }

        #endregion 

        #region "拣货（送修和借贷）"

        /// <summary>
        /// 送修拣货
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imeiList"></param>
        /// <param name="hid"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn Check(Model.Sys_UserInfo user,List<string> imeiList,string hid,List<Model.NoIMEIModel> models)
        {
               using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                  bool flag = false;
                 List<Model.SetSelection> list = new List<Model.SetSelection>();
               
                  ArrayList arr = new ArrayList();

                  if (imeiList.Count != 0)
                  {
                      list = CheckIMEI(imeiList, hid, ref flag);
                      arr.Add(flag);
                  }
                  else
                  {
                      arr.Add(false);
                      flag = true;
                  }

                   arr.Add(list);
                      
                  bool flag2= false;
                  list = new List<Model.SetSelection>();
                  if (models.Count() != 0)
                  {
                      list.Clear();
                      foreach (var item in models)
                      {
                          list.AddRange(CheckNoIMEI(item.ProID, item.ProCount, hid, ref  flag2));
                          flag = flag && flag2;
                      }
                      arr.Add(flag);
                  }
                  else
                  {
                      arr.Add(false);
                      flag2 = true;
                  }
                  flag = flag && flag2;

                  arr.Add(list);

                  //if (imeiList.Count != 0 && models.Count!=0)
                  //{
                  //    sucess = 
                  //}
                  return new Model.WebReturn() { ReturnValue= flag,ArrList=arr};
            }
        }

        /// <summary>
        /// 借贷拣货
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imeiList"></param>
        /// <param name="hid"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn BorowCheck(Model.Sys_UserInfo user, List<string> imeiList, string hid, string aduitid,List<Model.NoIMEIModel> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                bool flag = false;
                List<Model.SetSelection> list = new List<Model.SetSelection>();

                ArrayList arr = new ArrayList();

                if (imeiList.Count != 0)
                {
                    list = BorowCheckIMEI(imeiList, hid,aduitid, ref flag);
                    arr.Add(flag);
                }
                else
                {
                    arr.Add(false);
                    flag = true;
                }

                arr.Add(list);

                bool flag2 = false;
                list = new List<Model.SetSelection>();
                if (models.Count() != 0)
                {
                    list.Clear();
                    foreach (var item in models)
                    {
                        list.AddRange(CheckNoIMEI(item.ProID, item.ProCount, hid, ref  flag2));
                        flag = flag && flag2;
                    }
                    arr.Add(flag);
                }
                else
                {
                    arr.Add(false);
                    flag2 = true;
                }
                flag = flag && flag2;

                arr.Add(list);

                //if (imeiList.Count != 0 && models.Count!=0)
                //{
                //    sucess = 
                //}
                return new Model.WebReturn() { ReturnValue = flag, ArrList = arr };
            }
        }


        /// <summary>
        /// 串码拣货
        /// </summary>
        /// <param name="imeiList"></param>
        /// <param name="hallid"></param>
        /// <param name="sucess"></param>
        /// <returns></returns>
        private List<Model.SetSelection> CheckIMEI(List<string> imeiList, string hallid,ref bool sucess)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                sucess = true;
                List<Model.SetSelection> models = new List<Model.SetSelection>();


                var query = from b in lqh.Umsdb.Pro_IMEI
                            where imeiList.Contains(b.IMEI)
                           && b.OutID == null && b.BorowID == null &&b.VIPID==null&&
                           b.RepairID == null && b.SellID == null && b.HallID == hallid && (b.AssetID == null || b.AssetID == 0)
                            group b by new { b.InListID,b.ProID } into g
                            select new
                            {
                                InList =g.Select(p=>p.InListID),
                                count = g.Count(),
                                proid = g.Select(p => p.ProID),
                                table = g.Select(p => p.IMEI.ToUpper())
                            };
                if (query.Count() == 0 || query == null)
                {
                    sucess = false;
                    return new List<Model.SetSelection> () ;
                }
                decimal count = 0;

                foreach (var gp in query)
                {
                    Model.SetSelection selection = new Model.SetSelection();
                    selection.InList = gp.InList.First();
                    selection.Countnum = gp.count;
                    count += selection.Countnum;
                    selection.Proid = gp.proid.First().ToString();
                    selection.Note = "已成功";
                    if (selection.ReturnIMEI == null)
                    {
                        selection.ReturnIMEI = new List<string>();
                    }

                    selection.ReturnIMEI.AddRange(gp.table);
                    models.Add(selection);
                }
                if (count != imeiList.Count)
                {
                    sucess = false;
                }
                
                return models;
            }
        }

        /// <summary>
        /// 借贷有串码拣货
        /// </summary>
        /// <param name="imeiList"></param>
        /// <param name="hallid"></param>
        /// <param name="sucess"></param>
        /// <returns></returns>
        private List<Model.SetSelection> BorowCheckIMEI(List<string> imeiList, string hallid,string aduitid, ref bool sucess)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                sucess = true;
                List<Model.SetSelection> models = new List<Model.SetSelection>();

                var aduit = from a in lqh.Umsdb.Pro_BorowAduit
                            join list in lqh.Umsdb.Pro_BorowAduitList
                             on a.ID equals list.BAduitID
                            join p in lqh.Umsdb.Pro_ProInfo
                            on list.ProID equals p.ProID
                            where a.AduitID == aduitid && p.NeedIMEI
                            select new
                            {
                                list.ProID,
                                list.ProCount,
                            };

                var query = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                            where imeiList.Contains(b.IMEI)
                           && b.OutID == null && b.BorowID == null && b.VIPID == null &&
                           b.RepairID == null && b.HallID == hallid
                            group b by b.InListID into g
                            select new
                            {
                                g.Key,
                                count = g.Count(),
                                proid = g.Select(p => p.ProID),
                                table = g.Select(p => p.IMEI)
                            };
              
                if (query.Count() == 0 || query == null)
                {
                    sucess = false;
                    return new List<Model.SetSelection>();
                }
                decimal count = 0;

                foreach (var gp in query)
                {
                    if (aduit.First().ProID == gp.proid.First().ToString())
                    {
                        Model.SetSelection selection = new Model.SetSelection();
                        selection.InList = gp.Key;
                        selection.Countnum = gp.count;
                        count += selection.Countnum;
                        selection.Proid = gp.proid.First().ToString();
                        selection.Note = "已成功";
                        if (selection.ReturnIMEI == null)
                        {
                            selection.ReturnIMEI = new List<string>();
                        }
                        selection.ReturnIMEI.AddRange(gp.table);
                        models.Add(selection);
                    }
                }
                if (count != imeiList.Count)
                {
                    sucess = false;
                }

                return models;
            }
        }


        /// <summary>
        /// 无串码商品拣货
        /// </summary>
        /// <param name="proID"></param>
        /// <param name="procount"></param>
        /// <param name="hallID"></param>
        /// <param name="sucess"></param>
        /// <returns></returns>
        private List<Model.SetSelection> CheckNoIMEI(string proID, decimal procount, string hallID,ref bool sucess)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                sucess = true;
                List<Model.SetSelection> models = new List<Model.SetSelection>();

                Dictionary<String, decimal[]> result = new Dictionary<string, decimal[]>();
                var query_store = from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                  where b.ProID == proID && b.HallID == hallID
                                  && b.ProCount>0
                                  group b by b.InListID into g
                                  orderby g.Key ascending
                                  select new
                                  {
                                      g.Key,
                                      Count = g.Select(p => p.ProCount),
                                      pid = g.Select(p => p.ProID)
                                  };

                if (query_store.Count() == 0)
                {
                    sucess = false;
                    return new List<Model.SetSelection>() ;
                }

                foreach (var gp in query_store)
                {
                    if (gp.Count.First() != 0)
                    {
                        Model.SetSelection sel = new Model.SetSelection();

                        sel.Countnum = gp.Count.First();
                        sel.InList = gp.Key;
                        sel.Proid = proID;

                        if (gp.Count.First() >= procount)
                        {
                            sel.Countnum = procount;
                            models.Add(sel);
                            procount = 0;
                            break;
                        }
                        models.Add(sel);

                        procount -= sel.Countnum;
                        if (procount == 0)
                        {
                            break;
                        }

                    }
                }
                if (procount > 0)
                {
                    sucess = false;
                    models.Clear();
                }
                return  models;
            }
        }

        #endregion 


        #region "废弃"

        /// <summary>
        /// 串码检获
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Model.WebReturn SelectInlist1(Model.Sys_UserInfo user, List<string> s, string hallid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                List<Model.SetSelection>  Selection=new List<Model.SetSelection>();

                var query = lqh.Umsdb.GetTable<Model.Pro_IMEI>().Where(b => s.Contains(b.IMEI)
                                                                             && b.OutID == null && b.BorowID == null &&
                                                                             b.RepairID == null && b.HallID == hallid)
                    .GroupBy(b => b.InListID)
                    .Select(g => new
                    {
                        g.Key,
                        count = g.Count(),
                        proid = g.First().ProID,
                        table = g.Select(p => p.IMEI)
                    });
                if (query.Count() == 0 || query == null)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue =false,Message = "串码均不存在"}; ;
                }
                 
                foreach (var gp in query)
                {
                    Model.SetSelection selection = new Model.SetSelection();
                    selection.InList = gp.Key;
                    selection.Countnum = gp.count;
                    selection.Proid = gp.proid;
                    selection.Note = "已成功";
                    if (selection.ReturnIMEI == null)
                    {
                        selection.ReturnIMEI = new List<string>();
                    }
                
                    selection.ReturnIMEI.AddRange(gp.table);
                    Selection.Add(selection);
                }
                return new Model.WebReturn() { Obj = Selection , ReturnValue=true};
            }
        }

        /// <summary>
        /// 非串码检获
        /// </summary>
        /// <param name="proID"></param>
        /// <param name="hallID"></param>
        /// <param name="procount"></param>
        /// <returns></returns>
        public Model.WebReturn SelectInlist2(Model.Sys_UserInfo user, string proID, string hallID, decimal procount)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                List<Model.SetSelection> selection = new List<Model.SetSelection>();

                  var query_store = from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                  where b.ProID==proID&&b.HallID==hallID
                                  group b by b.InListID into g
                                  orderby g.Key  ascending
                                  select new
                                  {
                                      g.Key,
                                      Count = g.Select(p=>p.ProCount),
                                      pid=g.Select(p=>p.ProID)
                                  };

                if (query_store.Count()==0)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false}; ;
                }
               
                foreach (var gp in query_store)
                {
                    if (gp.Count.First() != 0)
                    {
                        Model.SetSelection sel = new Model.SetSelection();

                        sel.Countnum = gp.Count.First();
                        sel.InList = gp.Key;
                        sel.Proid = proID;

                        if (gp.Count.First() >= procount)
                        {   
                            sel.Countnum = procount;
                            selection.Add(sel);
                            procount = 0;
                            break;
                        }
                        selection.Add(sel);

                        procount -= sel.Countnum;
                        if (procount == 0)
                        {
                            break;
                        }
                        
                    }
                }
                if (procount > 0)
                    {
                        selection.Clear();
                    }
                return new Model.WebReturn() { Obj = selection, ReturnValue = true };
            }
        }

        #endregion

    }
}
