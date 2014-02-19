using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellListInfo_Temp
    {
        public Pro_SellListInfo_Temp()
        {
            this.Pro_Sell_JiPeiKa_temp = new List<Pro_Sell_JiPeiKa_temp>();
            this.Pro_Sell_Yanbao_temp = new List<Pro_Sell_Yanbao_temp>();
            this.VIP_VIPInfo_Temp = new List<VIP_VIPInfo_Temp>();
        }

        public int ID { get; set; }
        public Nullable<int> SellID { get; set; }
        public string ProID { get; set; }
        public decimal ProCount { get; set; }
        public string InListID { get; set; }
        public Nullable<int> SellType { get; set; }
        public decimal CashTicket { get; set; }
        public decimal ProPrice { get; set; }
        public string TicketID { get; set; }
        public decimal TicketUsed { get; set; }
        public decimal CashPrice { get; set; }
        public string IMEI { get; set; }
        public Nullable<int> ServiceInfo { get; set; }
        public string Note { get; set; }
        public decimal ProCost { get; set; }
        public decimal LowPrice { get; set; }
        public Nullable<int> AduidID { get; set; }
        public decimal AduidedOldPrice { get; set; }
        public Nullable<int> OffID { get; set; }
        public decimal OffPoint { get; set; }
        public Nullable<int> SpecialID { get; set; }
        public Nullable<int> SellType_Pro_ID { get; set; }
        public decimal OffPrice { get; set; }
        public decimal OffSepecialPrice { get; set; }
        public decimal WholeSaleOffPrice { get; set; }
        public Nullable<int> BackID { get; set; }
        public Nullable<int> OldSellListID { get; set; }
        public bool IsFree { get; set; }
        public string ChargePhoneNum { get; set; }
        public string OldID { get; set; }
        public string ChargePhoneName { get; set; }
        public string UserID { get; set; }
        public string HallID { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
        public decimal AnBu { get; set; }
        public decimal LieShouPrice { get; set; }
        public decimal YanBaoModelPrice { get; set; }
        public bool NeedAduit { get; set; }
        public virtual ICollection<Pro_Sell_JiPeiKa_temp> Pro_Sell_JiPeiKa_temp { get; set; }
        public virtual ICollection<Pro_Sell_Yanbao_temp> Pro_Sell_Yanbao_temp { get; set; }
        public virtual ICollection<VIP_VIPInfo_Temp> VIP_VIPInfo_Temp { get; set; }
    }
}
