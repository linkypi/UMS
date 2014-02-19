using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_Sell_Yanbao_temp
    {
        public int ID { get; set; }
        public string YanBaoName { get; set; }
        public string BillID { get; set; }
        public Nullable<int> SellListID { get; set; }
        public string MobileType { get; set; }
        public string MobileName { get; set; }
        public Nullable<decimal> MobilePrice { get; set; }
        public string MobileIMEI { get; set; }
        public Nullable<System.DateTime> MobileDate { get; set; }
        public string FatureNum { get; set; }
        public string BateriNum { get; set; }
        public string NgarkuesNum { get; set; }
        public string KufjeNum { get; set; }
        public string Note { get; set; }
        public Nullable<int> BackListID { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNum { get; set; }
        public Nullable<int> SellType { get; set; }
        public virtual Pro_SellBackList Pro_SellBackList { get; set; }
        public virtual Pro_SellListInfo_Temp Pro_SellListInfo_Temp { get; set; }
    }
}
