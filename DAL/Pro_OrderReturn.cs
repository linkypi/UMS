using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_OrderReturn
    {
        private int MethodID;

	    public Pro_OrderReturn()
	    {
		    this.MethodID = 0;
	    }

        public Pro_OrderReturn(int MethodID)
	    {
		    this.MethodID = MethodID;
	    }

        /// <summary>
        /// 检获
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //public Model.WebReturn Get_IMRIModel(Model.Sys_UserInfo user, Model.Pro_BackInfo model)
        //{
        //    if (model == null) return new Model.WebReturn();
        //    using (LinQSqlHelper lqh = new LinQSqlHelper())
        //    {

        //        try
        //        {
        //            //验证是否可退库

        //            //生成单号 存储过程OrderMacker
        //            //插入表头

        //            List<Model.SetSelection> selection = new List<Model.SetSelection>();
        //            foreach (var i in model.Pro_BackListInfo)
        //            {
        //                //是否存在串码                   
        //                if (i.Pro_BackOrderIMEI.Count() != 0 || i.Pro_BackOrderIMEI != null)
        //                {

        //                    string[] s = new string[i.Pro_BackOrderIMEI.Count()];

        //                    for (int next = 0; next < i.Pro_BackOrderIMEI.Count(); next++)
        //                    {
        //                        s[next] = i.Pro_BackOrderIMEI[next].IMEI;
        //                    }
        //                    SelectInlist se = new SelectInlist();
        //                    selection = se.SelectInlist1(s);
        //                }
        //                //不存在串码 检获
        //                else
        //                {
        //                    string proid = i.ProID.ToString();
        //                    string hallid = model.HallID;
        //                    int procount = Convert.ToDecimal(i.ProCount);
        //                    List<Model.SetSelection> sl = SelectInlist.SelectInlist2(proid, hallid, procount);
        //                    selection.AddRange(sl);
        //                }
        //            }
        //            //检获返回
        //            return new Model.WebReturn() { Obj = selection, ReturnValue = true, Message = "已检获" };
        //        }
        //        catch (Exception ex)
        //        {
        //            return new Model.WebReturn() { Obj = null, ReturnValue = false };
        //            throw ex;
        //        }

        //    }

        //}
        /// <summary>
        /// 退货
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_BackInfo model)
        {
            //验证是否可退库 
            //生成单号 存储过程OrderMacker
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证用户操作仓库  商品的权限 
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message =  "仓库无权操作 "+  que.First().HallName};
                        }

                        //验证商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in model.Pro_BackListInfo)
                            {
                                var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                             where p.ProID == item.ProID.ToString()
                                             select p;
                                classids.Add(queryc.First().Pro_ClassID.ToString());
                            }
                            foreach (var child in classids)
                            {
                                if (!ValidProIDS.Contains(child))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                }
                            }
                        }
                        #endregion

                        string msg = null;
                        lqh.Umsdb.OrderMacker(1, "RK", "RK", ref msg);
                        if (msg == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退库单生成出错" };
                        }
                        model.BackID = msg;
                        lqh.Umsdb.Pro_BackInfo.DeleteOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "退库成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "退库失败" };
                        throw ex;
                    }
                }
            }
        }
    }
}