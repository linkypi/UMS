using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using Model;

namespace DAL
{
    public class SMS_SignSendPayInfo
    {
        private int MethodID;

        public SMS_SignSendPayInfo()
        {
            MethodID = 0;
        }

        public SMS_SignSendPayInfo(int M)
        {
            MethodID = M;
        }

        public WebReturn Del(Model.Sys_UserInfo sysUser, Model.SMS_SignSendPayInfo model)
        {
            Model.WebReturn r = null;
            bool NoError = true;

            if (model == null)
            {

                return new WebReturn() { ReturnValue = false };

            }
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions dl = new DataLoadOptions();
                dl.LoadWith<Model.SMS_SignInfo>(info => info.SMS_SignSendPayInfo);
                dl.LoadWith<Model.SMS_SignInfo>(info => info.SMS_SignSendPayInfo_Delete);
                dl.LoadWith<Model.SMS_SignSendPayInfo>(info => info.SMS_SignPayInListInfo);
                lqh.Umsdb.LoadOptions = dl;
                var query = lqh.Umsdb.SMS_SignSendPayInfo.Where(p => p.ID == model.ID);
                if (query.Any())
                {
                    var s = query.First();
                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Any() && !ValidHallIDS.Contains(s.SMS_SignInfo.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" };
                    #endregion
                    if (model.RealCount < 0 || model.RealPay < 0)
                    {
                        return new WebReturn()
                        {
                            ReturnValue = false,
                            Message = "数据有误"

                        };
                    }
                    //加库存开始
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == s.SMS_SignInfo.HallID && b.ProID == model.ProID && s.SMS_SignPayInListInfo.Select(p => p.InListID).Contains(b.InListID)
                                     orderby b.InListID
                                     select b).ToList();


                    foreach (var listinfo in s.SMS_SignPayInListInfo)
                    {
                        var m = StoreList.First(p => p.InListID == listinfo.InListID);
                        m.ProCount += Convert.ToDecimal(listinfo.ProCount);


                    }


                    //加库存结束


                    SMS_SignSendPayInfo_Delete v=new    SMS_SignSendPayInfo_Delete()
                    {
                        OldID = s.ID,
                        SellID = s.SellID,
                        RealPay=s.RealPay,
                        RealCount=s.RealCount,
                        ProID = s.ProID,
                        SysDate = s.SysDate,
                        UserID = s.UserID,
                        DeleteDate = DateTime.Now,
                    };
                    lqh.Umsdb.SMS_SignSendPayInfo_Delete.InsertOnSubmit(v);
                    var signinfo = s.SMS_SignInfo;
                    signinfo.RealPay -= model.RealPay;
                    signinfo.RealCount -= model.RealCount;
                    
                    signinfo.SMS_SignSendPayInfo.Remove(s);


                    lqh.Umsdb.SubmitChanges();
                    lqh.Umsdb.Refresh(RefreshMode.OverwriteCurrentValues, signinfo);
                    return new WebReturn() { ReturnValue = true, Message = "保存成功", Obj = signinfo };
                }
                else
                {
                    return new WebReturn() { ReturnValue = false, Message = "无该合同" };
                }
            }
        }
        public WebReturn Add(Model.Sys_UserInfo sysUser, Model.SMS_SignSendPayInfo model)
        { Model.WebReturn r = null;
            bool NoError = true;
            
            if (model == null)
            {

                return new WebReturn() { ReturnValue = false };

            }
            model.UserID = sysUser.UserID;
            model.SysDate = DateTime.Now;
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions dl = new DataLoadOptions();
                dl.LoadWith<Model.SMS_SignInfo>(info => info.SMS_SignSendPayInfo);
                dl.LoadWith<Model.SMS_SignInfo>(info => info.SMS_SignSendPayInfo_Delete);
                dl.LoadWith<Model.SMS_SignSendPayInfo>(info => info.SMS_SignPayInListInfo);
                lqh.Umsdb.LoadOptions = dl;
                var query = lqh.Umsdb.SMS_SignInfo.Where(p => p.ID == model.SellID);
                if (query.Any())
                {
                    var s = query.First();
                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Any() && !ValidHallIDS.Contains(s.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" };
                    #endregion

                    if (model.RealCount < 0 || model.RealPay < 0)
                    {
                        return new WebReturn()
                        {
                            ReturnValue = false,
                            Message = "数据有误"

                        };
                    }
                    var osend = s.SMS_SignSendPayInfo.Sum(p => p.RealCount);
                    var opay = s.SMS_SignSendPayInfo.Sum(p => p.RealPay);
                    if (osend + model.RealCount > s.SignCount)
                    {
                        return new WebReturn()
                        {
                            ReturnValue = false,Message = "超出短信量"
                        
                        };
                    }
                    if ((opay > s.SignPay)&&model.RealPay>0)
                    {
                        return new WebReturn()
                        {
                            ReturnValue = false,Message = "超出价格"
                        
                        };
                    }
                    //减库存开始

                    //拣货
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == s.HallID && b.ProID==model.ProID && b.ProCount > 0
                                     orderby b.InListID
                                     select b).ToList();
                    var needsms = model.RealCount;
                    foreach (var proStoreInfo in StoreList)
                    {
                        decimal thissms = 0;
                        if (proStoreInfo.ProCount >= needsms)
                        {
//本明细数量
                            thissms = needsms;
                        }
                        else
                        {
                            thissms = proStoreInfo.ProCount;
                        }
                        proStoreInfo.ProCount -= thissms;
                        needsms -= thissms;
                        Model.SMS_SignPayInListInfo listinfo=new SMS_SignPayInListInfo();
                        
                        listinfo.InListID = proStoreInfo.InListID;
                        listinfo.ProCount = thissms;
                        listinfo.ProID = model.ProID;
                        model.SMS_SignPayInListInfo.Add(listinfo);
                        if (needsms == 0)
                        {
                            break;
                        }
                        //lqh.Umsdb.SMS_SignPayInListInfo.InsertOnSubmit(listinfo);
                    }
                    if (needsms > 0)
                    {
                        return new WebReturn(){ReturnValue = false,Message = "库存不足"};
                    }
                    //减库存结束
                    s.RealPay += model.RealPay;
                    s.RealCount += model.RealCount;
                    s.SMS_SignSendPayInfo.Add(model);
                    

                    lqh.Umsdb.SubmitChanges();
                    lqh.Umsdb.Refresh(RefreshMode.OverwriteCurrentValues,s);
                    return new WebReturn() { ReturnValue = true, Message = "保存成功",Obj=s };
                }
                else
                {
                    return new WebReturn() {ReturnValue = false, Message = "无该合同"};
                }
            }
        }
        
    }
}