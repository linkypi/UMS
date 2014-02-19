using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_IMEI
    {
        public Pro_IMEI()
        {
            this.Pro_IMEI11 = new List<Pro_IMEI>();
        }

        public int ID { get; set; }
        public string InListID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> NEW_IMEI_ID { get; set; }
        public string IMEI { get; set; }
        public Nullable<int> OutID { get; set; }
        public Nullable<int> BorowID { get; set; }
        public Nullable<int> ReturnID { get; set; }
        public Nullable<int> RepairID { get; set; }
        public Nullable<int> VIPID { get; set; }
        public string HallID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<int> AuditID { get; set; }
        public virtual Pro_BorowOrderIMEI Pro_BorowOrderIMEI { get; set; }
        public virtual Pro_SellOffAduitInfo Pro_SellOffAduitInfo { get; set; }
        public virtual Pro_IMEI Pro_IMEI1 { get; set; }
        public virtual Pro_IMEI Pro_IMEI2 { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI11 { get; set; }
        public virtual Pro_IMEI Pro_IMEI3 { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
        public virtual Pro_OutInfo Pro_OutInfo { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_RepairListInfo Pro_RepairListInfo { get; set; }
        public virtual Pro_ReturnOrderIMEI Pro_ReturnOrderIMEI { get; set; }
        public virtual VIP_VIPInfo_Temp VIP_VIPInfo_Temp { get; set; }
    }
}
