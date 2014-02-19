using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_InOrderList
    {
        public Pro_InOrderList()
        {
            this.Pro_BackListInfo = new List<Pro_BackListInfo>();
            this.Pro_IMEI = new List<Pro_IMEI>();
            this.Pro_InOrderIMEI = new List<Pro_InOrderIMEI>();
            this.SMS_SignPayInListInfo = new List<SMS_SignPayInListInfo>();
            this.Pro_InOrderList1 = new List<Pro_InOrderList>();
            this.Pro_OutOrderList = new List<Pro_OutOrderList>();
            this.Pro_ReturnListInfo = new List<Pro_ReturnListInfo>();
            this.Pro_SellBackList = new List<Pro_SellBackList>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
            this.Pro_StoreInfo = new List<Pro_StoreInfo>();
        }

        public string InListID { get; set; }
        public Nullable<int> Pro_InOrderID { get; set; }
        public string InOrderID { get; set; }
        public string ProID { get; set; }
        public decimal ProCount { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public string InitInListID { get; set; }
        public decimal RetailPrice { get; set; }
        public virtual ICollection<Pro_BackListInfo> Pro_BackListInfo { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
        public virtual Pro_InOrder Pro_InOrder { get; set; }
        public virtual ICollection<Pro_InOrderIMEI> Pro_InOrderIMEI { get; set; }
        public virtual ICollection<SMS_SignPayInListInfo> SMS_SignPayInListInfo { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual ICollection<Pro_InOrderList> Pro_InOrderList1 { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList2 { get; set; }
        public virtual ICollection<Pro_OutOrderList> Pro_OutOrderList { get; set; }
        public virtual ICollection<Pro_ReturnListInfo> Pro_ReturnListInfo { get; set; }
        public virtual ICollection<Pro_SellBackList> Pro_SellBackList { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual ICollection<Pro_StoreInfo> Pro_StoreInfo { get; set; }
    }
}
