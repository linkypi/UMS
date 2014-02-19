using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class VIP_VIPCardMS
    {
        private int _MethodID;

        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public VIP_VIPCardMS()
        {
            this.MethodID = 0;
        }

        public VIP_VIPCardMS(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        public Model.WebReturn Search(Model.Sys_UserInfo user,string  CardID,string  IDCard_ID,string VIPName,string MobilePhone)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions ds =new DataLoadOptions();
                ds.LoadWith<Model.VIP_VIPInfo>(info => info.VIP_VIPService);

                lqh.Umsdb.LoadOptions = ds;
//                var cardid = (from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
//                             where (b.IMEI == CardID||b.IDCard_ID.ToString()==IDCard_ID
//                             ||b.MemberName==VIPName||b.MobiPhone==MobilePhone)&&b.Flag==true
//                             select b).ToList();
                var cardid = (from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                              where (b.IMEI == CardID ) && b.Flag == true
                              select b).ToList();
                if (cardid.Count() == 0)
                {
                    string Msg="会员不存在或条件输入有误";
              
                    return new Model.WebReturn() { Obj = null,ReturnValue = false, Message = Msg };
                }
                return new Model.WebReturn() { Obj = cardid, Message = "已获取", ReturnValue = true };
            }
            
        }
        /// <summary>
        /// 查询卡类别信息和服务
        /// </summary>
        /// <param name="user"></param>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public Model.WebReturn SearchVIPType(Model.Sys_UserInfo user,string IMEI)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions d = new DataLoadOptions();
                d.LoadWith<Model.Pro_ProInfo>(c => c.Pro_IMEI);
                d.LoadWith<Model.VIP_VIPType>(c => c.VIP_VIPTypeService);

                lqh.Umsdb.LoadOptions = d;
                #region 验证用户操作仓库  商品的权限
                List<string> ValidHallIDS = new List<string>();
                //有权限的商品
                List<string> ValidClassIDS = new List<string>();

                Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidClassIDS, lqh);

                if (ret.ReturnValue != true)
                { return ret; }                
                #endregion
                var exitIMEI = from b in lqh.Umsdb.VIP_VIPInfo
                               where b.IMEI == IMEI
                               select b;
                if (exitIMEI.Count() > 0)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "该会员卡已使用" };
                }
              
                        
                var query_VIPType = from b in lqh.Umsdb.Pro_ProInfo
                                    from imei in b.Pro_IMEI                                
                                    join r in lqh.Umsdb.VIP_VIPType on b.VIP_TypeID equals r.ID
                                    where imei.IMEI == IMEI && imei.VIPID ==null && imei.SellID ==null
                                    && imei.OutID == null && imei.BorowID == null && imei.RepairID == null && (imei.AssetID == null || imei.AssetID == 0)
                                    where ValidHallIDS.Contains(imei.HallID)
                                    select new 
                                    {
                                        r,
                                        imei.HallID,
                                        imei.ProID
                                    };
                                    
                if (query_VIPType.Count() ==0)
                {
                    string Msg = "可操作仓库无该卡或存在其他操作";                 
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = Msg };
                }
                query_VIPType.First().r.VIP_VIPTypeService.ToList();
                ArrayList List = new ArrayList() { query_VIPType.First().HallID, query_VIPType.First().ProID};

                return new Model.WebReturn() { Obj = query_VIPType.First().r, Message = "已获取", ReturnValue = true, ArrList = List };
            }
        }
      
    }
}
