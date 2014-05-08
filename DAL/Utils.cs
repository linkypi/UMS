using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Serialization;
namespace Common
{
    public static class Utils
    {
        public static bool IsZhongshanSellType(int? SelltypeID)
        {

            return !new int?[] {8, 9, 12}.Contains(SelltypeID);

        }
        public static T Clone<T>(this T source)
        {
            var dcs = new DataContractSerializer(typeof(T));
            using (var ms = new System.IO.MemoryStream())
            {
                dcs.WriteObject(ms, source);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)dcs.ReadObject(ms);
            }
        }


        public static Model.WebReturn CheckIMEI(Model.Pro_IMEI imei)
        {
            Model.WebReturn r = new Model.WebReturn() { ReturnValue = true };
            if (imei == null)
            {
                r.Message = "串码不存在！ ";
                r.ReturnValue = false;
            }
            else if (imei.Pro_ProInfo == null)
            {
                r.Message = "串码：" + imei.IMEI + " 的商品不存在";
                r.ReturnValue = false;
            }
            else if (!imei.Pro_ProInfo.NeedIMEI == true)
            {
                r.Message = "串码："+ imei.IMEI+" 属于无串码商品";
                r.ReturnValue = false;
            }

            else if (imei.VIPID >0 || imei.SellID > 0 || imei.OutID > 0 || imei.BorowID > 0 || imei.RepairID > 0 || imei.AuditID > 0 || imei.AssetID > 0|| imei.BJID>0|| imei.PJID>0||imei.State>0)
            {
                r.Message = "串码已处理: " + imei.IMEI;
                r.ReturnValue = false;
            }
            else if (imei.Pro_StoreInfo == null || imei.Pro_StoreInfo.ProCount - 1 < 0)
            {
                r.Message = "串码 "+ imei.IMEI +" 库存不足";
                r.ReturnValue = false;
            }

            //switch ((Common.State)imei.State)
            //{
            //     case Common.State.Assest:

            //        break;
            //     case Common.State.Audit:

            //        break;
            //     case Common.State.BackUp:

            //        break;
            //     case Common.State.PeiJian:

            //        break;
            //     case Common.State.Borrow:

            //        break;
            //     case Common.State.Out:

            //        break;

            //     case Common.State.Repair:

            //        break;
            //     case Common.State.Return:

            //        break;
            //     case Common.State.Sell:

            //        break;
            //     case Common.State.VIP:

            //        break;
            //}
            return r;
        }

    }
}