using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using Model;

namespace DAL
{
    public class Pro_Sell_CarInfo
    {
        private int MethodID;
        public Pro_Sell_CarInfo()
        {
            this.MethodID = 0;
        }
        public Pro_Sell_CarInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        public WebReturn Search(Model.Sys_UserInfo sysuser, string CName, string CID)
        {
            try
            {
                var list = Common.IllegalModel.GetModels(CName, CID);
                var prolist = new List<Pro_SellListInfo>();
               
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    #region "验证用户操作仓库  商品的权限 "
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();
                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(sysuser, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }
                    #endregion
                    Regex ex=new Regex(@"【(\d+?)】");

                    foreach (var illegalModel in list)
                    {
                        var sid = ex.Match(illegalModel.违章信息).Groups[1];
                        var query =
                            lqh.Umsdb.Pro_ProInfo.Where(
                                p => ValidProIDS.Contains(p.Pro_ClassID+"") && (p.ProFormat == sid.Value));
                        if (query.Any())
                        {
                            var pro = query.First();
                            if (pro.Pro_SellTypeProduct.All(p => p.SellType != 1))
                            {
                                continue;
                            }
                            var selltypep = pro.Pro_SellTypeProduct.First(p => p.SellType == 1);
                            prolist.Add(new Pro_SellListInfo()
                            {
                                ProID = pro.ProID,
                                ProCount = 1,
                                SellType = 1,
                                SellType_Pro_ID = selltypep.ID,
                                ProPrice = selltypep.Price,
                                
                                OtherCash = (illegalModel.管辖区域 == "中山" || illegalModel.管辖区域 == "江门") ? 0 : 40,
                                CashPrice = selltypep.Price + ((illegalModel.管辖区域 == "中山" || illegalModel.管辖区域 == "江门") ? 0 : 40),
                                Pro_Sell_Car = new EntitySet<Pro_Sell_Car>()
                                {
                                    new Pro_Sell_Car()
                                    {
                                        Address =illegalModel.管辖区域,
                                        CarID = CID,
                                        CarName = CName,
                                        Desc = illegalModel.违章信息,
                                        IsOther = !(illegalModel.管辖区域 == "中山" || illegalModel.管辖区域 == "江门"),


                                    }
                                }

                            });

                        }
                    }
                    if (prolist.Count == 0)
                    {
                        return new WebReturn() { Obj = prolist, ReturnValue = false,Message = "有违章, 但无可代办内容"};
                    }
                    return new  WebReturn(){Obj = prolist,ReturnValue = true};
                }


            }
            catch (Exception ex)
            {
                return new WebReturn() { ReturnValue = false,Message = ex.Message};    
            }
            return new WebReturn() { ReturnValue = false};    
        }

    }
}