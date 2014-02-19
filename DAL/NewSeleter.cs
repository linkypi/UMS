using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class NewSeleter
    {
              
        private int MenthodID;

	    public NewSeleter()
	    {
		    this.MenthodID = 0;
	    }

        public NewSeleter(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }
        /// <summary>
        /// 统一捡货
        /// </summary>
        /// <param name="user"></param>
        /// <param name="s"></param>
        /// <param name="hallid"></param>
        /// <returns></returns>
        public Model.WebReturn NewSelectInlist(Model.Sys_UserInfo user, List<Model.SeleterModel> selectModel)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                List<string> selectIMEI = new List<string>();
                List<Model.SeleterModel> SucceedModel = new List<Model.SeleterModel>();//成功列表
                List<string> ProIDS = new List<string>();
                bool judge = true;
                bool Unjudge = true;
                foreach (var index in selectModel)
                {
                    if (judge == false)
                        Unjudge = false;
                    if (index.IsIMEI == null && index.ProID == null)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "存在非空集合" };
                    }
                    if (string.IsNullOrEmpty(index.HallID))
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "请填写仓库" };
                    }
                    #region 非串码捡货
                    if (index.IsIMEI == null)
                    {
                        //判断                                          
                        if ( index.Count <= 0)
                        {
                            index.Note = "商品数量必须大于0";
                            judge = false;
                        }
                        else
                        {
                            //双向联接获取商品信息和库存信息
                            var query = (from c in lqh.Umsdb.Pro_StoreInfo
                                         where c.ProID == index.ProID && c.HallID == index.HallID && c.ProCount > 0
                                         group c by c.InListID into g
                                         from temp in g
                                         select new
                                         {
                                             g.Key,
                                             ProCount = temp.ProCount,
                                             ProID = temp.ProID
                                         }).ToList();
                            if (query.Count() == 0)
                            {
                                index.Note = "该仓库不存在此商品";
                                judge = false;
                                continue;
                            }
                            #region 检货操作
                            decimal Count = index.Count;
                            foreach (var gp in query)
                            {
                                Model.SeleterModel sel = new Model.SeleterModel();
                                sel.Count = decimal.Parse(gp.ProCount.ToString("#0.00"));
                                sel.ProInListID = gp.Key;
                                sel.ProID = gp.ProID;
                                if (sel.Count >= Count)
                                {
                                    index.Note = "成功";
                                    sel.Note = "成功";
                                    sel.Count = decimal.Parse(Count.ToString("#0.00"));
                                    SucceedModel.Add(sel);
                                    Count = 0;
                                    break;
                                }
                                SucceedModel.Add(sel);
                                Count -= sel.Count;
                                if (Count == 0)
                                {
                                    index.Note = "成功";
                                    sel.Note = "成功";
                                    break;
                                }
                            }
                            //失败判断
                            if (Count > 0)
                            {
                                index.Note = "数量不足";
                                judge = false;
                            }
                            //成功操作
                            else
                            {
                                index.Note = "成功";
                                judge = true;
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region 串码捡货
                    else
                    {
                        foreach (var next in index.IsIMEI)
                        {
                            if (!string.IsNullOrEmpty(next.IMEI))
                                selectIMEI.Add(next.IMEI);
                        }
                        //获取串码
                        var query = (from b in lqh.Umsdb.Pro_IMEI
                                     where selectIMEI.Contains(b.IMEI)
                                      && (b.OutID == null || b.OutID == 0) && (b.BorowID == null || b.BorowID == 0) 
                                      && (b.RepairID == null || b.RepairID == 0) && b.HallID == index.HallID 
                                      && (b.SellID == null || b.SellID == 0)&&(b.AuditID==null||b.AuditID==0)
                                      &&(b.AssetID==null || b.AssetID==0)
                                     group b by new { b.InListID, b.ProID } into g
                                     select new
                                     {
                                         InListID=g.Key.InListID,
                                         ProCount = g.Count(),
                                         //ProIDList= g.Select(p => p.ProID),
                                         ProID = g.Key.ProID,
                                         IMEIList = g.Select(p => p.IMEI)
                                     }).ToList();
                        if (query.Count() == 0)
                        {
                            foreach (var next in index.IsIMEI)
                            {
                                next.Note = "无此串码或存在其他操作";
                            }
                            judge = false;
                            continue;
                        }
                        //更新初始数据
                        foreach (var next in index.IsIMEI)
                        {
                            var query_exist=(from b in query
                                            where b.IMEIList.Contains(next.IMEI)
                                            select b).ToList();
                            if (query_exist.Count()==0)
                                 next.Note = "无此串码或存在其他操作";                                
                            else
                                next.Note = "成功"; 
                        }
                        //串码列表添加到实体中 
                        int total = 0;
                        for (int i = 0; i < query.Count(); i++)
                        {
                            Model.SeleterModel selection = new Model.SeleterModel();
                            selection.ProInListID = query[i].InListID;
                            selection.Count = query[i].ProCount;
                            selection.ProID = query[i].ProID;
                            //添加总数
                            total += query[i].ProCount;
         
                            //添加串码
                            foreach (var list in query[i].IMEIList)
                            {
                                Model.SelecterIMEI IMEIList = new Model.SelecterIMEI();
                                IMEIList.IMEI = list;
                                IMEIList.Note = "成功";
                                if (selection.IsIMEI == null)
                                {
                                    selection.IsIMEI = new List<Model.SelecterIMEI>();
                                }
                                selection.IsIMEI.Add(IMEIList);
                            }
                            SucceedModel.Add(selection);
                        }
                        //根据是否成功确定添加的实体
                        if (total == index.IsIMEI.Count())
                            judge = true;
                        else
                            judge = false;
                    }
                    #endregion
                }
                 if (judge&&Unjudge)
                 {
                     //获取成功列表的商品信息
                     ProIDS = (from b in SucceedModel
                               select b.ProID).ToList();

                     var GetName =( from b in lqh.Umsdb.Pro_ProInfo
                                   where ProIDS.Contains(b.ProID)
                                   join cl in lqh.Umsdb.Pro_ClassInfo on b.Pro_ClassID equals cl.ClassID
                                   join ty in lqh.Umsdb.Pro_TypeInfo on b.Pro_TypeID equals ty.TypeID
                                   select new
                                   {
                                       b.ISdecimals,
                                       b.ProID,
                                       b.ProName,
                                       b.ProFormat,
                                       b.NeedIMEI,
                                       cl.ClassID,
                                       cl.ClassName,
                                       ty.TypeID,
                                       ty.TypeName
                                   }).ToList();

                      //赋值给实体
                     foreach (var Name in GetName)
                     {
                         foreach (var next in SucceedModel)
                         {
                             if (Name.ProID == next.ProID)
                             {
                                 next.ProName = Name.ProName;
                                 next.ClassID = Convert.ToInt32(Name.ClassID);
                                 next.ProClassName = Name.ClassName;
                                 next.TypeID = Convert.ToInt32(Name.TypeID);
                                 next.ProTypeName = Name.TypeName;
                                 next.ProFormat = Name.ProFormat;
                                 next.IsNeedIMEI = Name.NeedIMEI;
                                 next.ISdecimals = Name.ISdecimals;
                                 //if (next.Note == null)
                                  next.Note = string.Empty;
                                 continue;
                             }
                         }
                     }
                     bool IsSucceed = true;
                     ArrayList Re = new ArrayList();
                     Re.Add(IsSucceed);
                     return new Model.WebReturn() { Obj = SucceedModel, ReturnValue = true, Message = "捡获成功", ArrList = Re };
                 }
                 else
                     return new Model.WebReturn() { Obj = selectModel, ReturnValue = true, Message = "部分商品捡获失败"};
            }
        }
    }
}

