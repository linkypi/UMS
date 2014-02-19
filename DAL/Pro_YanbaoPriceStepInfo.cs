using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_YanbaoPriceStepInfo:Sys_InitParentInfo
    {
        private int MethodID;

	    public Pro_YanbaoPriceStepInfo()
	    {
		    this.MethodID = 0;
	    }

        public Pro_YanbaoPriceStepInfo(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }

        /// <summary>
        /// 获取当前延保价格
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<Model.Pro_YanbaoPriceStepInfo> GetList(Model.Sys_UserInfo user,DateTime ds)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var result = from yb in lqh.Umsdb.Pro_YanbaoPriceStepInfo
                                 select yb;
                    if (result.Count() == 0)
                    {
                        return new List<Model.Pro_YanbaoPriceStepInfo>();
                    }
                    return result.ToList() ;
                }
                catch (Exception ex)
                {
                    return new List<Model.Pro_YanbaoPriceStepInfo>();
                }
            }
        }

        public Model.WebReturn GetCurrentPrice(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var result = from yb in lqh.Umsdb.View_YanBoPriceStepInfo
                                 select yb;
                    if (result.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "商品尚未添加延保价格" };
                    }
                    return new Model.WebReturn() { ReturnValue=true,Obj=result.ToList()};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 添加延保价格
        /// </summary>
        /// <param name="user"></param>
        /// <param name="yb"></param>
        /// <param name="ppc"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_PriceChange header, List<Model.Pro_YanbaoPriceStepInfo> models_update)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        //#region "验证用户操作仓库  商品的权限 "
                        //List<string> ValidHallIDS = new List<string>();
                        ////有权限的商品
                        //List<string> ValidProIDS = new List<string>();

                        //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                        //if (ret.ReturnValue != true)
                        //{ return ret; }
                        ////有仓库限制，而且仓库不在权限范围内
                        //if (ValidProIDS.Count > 0)
                        //{
                        //    List<string> proids = new List<string>();
                        //    foreach (var item in header.Pro_YanbaoPriceStepInfo)
                        //    {
                        //        proids.Add(item.ProID);
                        //    }
                        //    foreach (var child in proids)
                        //    {
                        //        if (!ValidProIDS.Contains(child))
                        //        {
                        //            var que = from h in lqh.Umsdb.Pro_ProInfo
                        //                      where h.ProID == child
                        //                      select h;
                        //            return new Model.WebReturn() { ReturnValue = false, Message = "商品" + que.First().ProName + "无权操作" };
                        //        }
                        //    }
                        //}
                        //#endregion
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        {
                            return result;
                        }
                        string changeid = "";
                        lqh.Umsdb.OrderMacker(1, "YB", "YB", ref changeid);
                        if (string.IsNullOrEmpty(changeid))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "延保调价单生成出错" };
                        }
                        header.ChangeID = changeid;
                        lqh.Umsdb.Pro_PriceChange.InsertOnSubmit(header);

                       #region  更新延保价格
                  
                        foreach(var item in models_update)
                        {
                            var query = from y in lqh.Umsdb.Pro_YanbaoPriceStepInfo
                                        where y.ID == item.ID
                                        select y;
                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue=false,Message="更新有误"};
                            }
                            //更新延保单
                            query.First().Name = item.Name;
                            query.First().Note = item.Note;
                            query.First().LowPrice = item.LowPrice;
                            query.First().ProCost = item.ProCost;
                            query.First().ProPrice = item.ProPrice;
                            query.First().StepPrice = item.StepPrice;
                        }

                      #endregion 

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 删除延保价格
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<int> ids)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        {
                            return result;
                        }
                        var yb = from y in lqh.Umsdb.Pro_YanbaoPriceStepInfo
                                 where ids.Contains(y.ID)
                                 select y;
                        if (yb.Count() != ids.Count)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分延保价格不存在，删除失败！" };
                        }

                        foreach (var item in yb)
                        {
                            lqh.Umsdb.Pro_YanbaoPriceStepInfo.DeleteOnSubmit(item);
                        }
                    
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message ="删除成功",Obj=ids };
                    }
                    catch (System.Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                    
                }
            }
                    
        }
    }
}
