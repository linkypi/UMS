using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Model;

namespace DAL
{
    public class SMS_SignInfo
    {
        private int MethodID;

        public SMS_SignInfo()
        {
            MethodID = 0;
        }

        public SMS_SignInfo(int M)
        {
            MethodID = M;
        }
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebReturn Add(Model.Sys_UserInfo sysUser, Model.SMS_SignInfo model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            
            model.UserID = sysUser.UserID;
            model.SysDate = DateTime.Now;
            if (model == null)
            {

                return new  WebReturn(){ReturnValue = false};

            }
            if (model.ID == 0)
            {
                //新增
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Any() && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" };
                    #endregion

                    model.IsOver = false;
                    model.SMS_SignSendPayInfo_Delete=new EntitySet<SMS_SignSendPayInfo_Delete>();
                    model.SMS_SignSendPayInfo=new EntitySet<Model.SMS_SignSendPayInfo>();
                    #region 生成单号
                    string SellID = "";
                    lqh.Umsdb.OrderMacker2(model.HallID, ref SellID);
                    if (SellID == "")
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
                    }
                    model.SellID = SellID;
                    #endregion
                    lqh.Umsdb.SMS_SignInfo.InsertOnSubmit(model);
                    lqh.Umsdb.SubmitChanges();
                    return new WebReturn() {ReturnValue = true,Obj = model};

                }
                
            }
            else
            {//修改

                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    if (lqh.Umsdb.SMS_SignInfo.Any(p => p.ID == model.ID))
                    {
                        #region 验证仓库 、商品权限
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                        if (r.ReturnValue != true)
                            return r;
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Any() && !ValidHallIDS.Contains(model.HallID))
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" };
                        #endregion
                        var old = lqh.Umsdb.SMS_SignInfo.First(p => p.ID == model.ID);
                        old.Industry = model.Industry;
                        old.SignDate = model.SignDate;
                        old.CpcName = model.CpcName;
                        old.CpcAdd = model.CpcAdd;
                        old.CusPhone = model.CusPhone;
                        old.SMSContent = model.SMSContent;
                        old.PayAllDate = model.PayAllDate;
                        old.RealPayAllDate = model.RealPayAllDate;
                        old.PayBack = model.PayBack;
                        old.CusName = model.CusName;
                        old.BillHeader = model.BillHeader;
                        old.BillNum = model.BillNum;
                        old.BillDate = model.BillDate;
                        old.RatePay = model.RatePay;
                        old.Note = model.Note;
                        lqh.Umsdb.SubmitChanges();
                        return new WebReturn(){ReturnValue = true,Obj=old};
                    }
                    else
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "无该合同" };
                    }

                }

                return new WebReturn() { ReturnValue = false };
            }
        }

        public WebReturn GetModel(Model.Sys_UserInfo sysUser, string sellid)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            if (string.IsNullOrEmpty(sellid))return new WebReturn(){ReturnValue = false,Message = "SellID空"};
            using (LinQSqlHelper lqh=new LinQSqlHelper())
            {
                DataLoadOptions dl=new DataLoadOptions();
                dl.LoadWith<Model.SMS_SignInfo>(info => info.SMS_SignSendPayInfo);
                dl.LoadWith<Model.SMS_SignInfo>(info => info.SMS_SignSendPayInfo_Delete);
                dl.LoadWith<Model.SMS_SignSendPayInfo>(info => info.SMS_SignPayInListInfo);
                lqh.Umsdb.LoadOptions = dl;
                if (lqh.Umsdb.SMS_SignInfo.Any(p => p.SellID == sellid))
                {
                    var model = lqh.Umsdb.SMS_SignInfo.First(p => p.SellID == sellid);

                    return new WebReturn() {ReturnValue = true, Obj = model};
                }
                else
                {
                    return new WebReturn() {ReturnValue = false, Message = "销售单不存在"};
                }
            }
        }
    }
}