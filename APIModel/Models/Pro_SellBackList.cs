using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellBackList
    {
        public Pro_SellBackList()
        {
            this.Pro_Sell_Yanbao = new List<Pro_Sell_Yanbao>();
            this.Pro_Sell_Yanbao_temp = new List<Pro_Sell_Yanbao_temp>();
            this.Pro_SellBackIMEIList = new List<Pro_SellBackIMEIList>();
        }

        public int ID { get; set; }
        public Nullable<int> BackID { get; set; }
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
        public string Note { get; set; }
        public decimal ProCost { get; set; }
        public decimal LowPrice { get; set; }
        public decimal AduidedNewPrice { get; set; }
        public Nullable<int> OffID { get; set; }
        public Nullable<int> SellListID { get; set; }
        public decimal OffPoint { get; set; }
        public decimal OffPrice { get; set; }
        public decimal OffSepecialPrice { get; set; }
        public decimal WholeSaleOffPrice { get; set; }
        public Nullable<int> SellType_Pro_ID { get; set; }
        public Nullable<int> SpecialID { get; set; }
        public decimal OtherCash { get; set; }
        public decimal ShouldBackCash { get; set; }
        public decimal AnBu { get; set; }
        public decimal LieShou { get; set; }
        public decimal LieShouPrice { get; set; }
        public decimal OtherOff { get; set; }
        public decimal AnBuPrice { get; set; }
        public Nullable<int> BackAduitID { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual ICollection<Pro_Sell_Yanbao> Pro_Sell_Yanbao { get; set; }
        public virtual ICollection<Pro_Sell_Yanbao_temp> Pro_Sell_Yanbao_temp { get; set; }
        public virtual Pro_SellBack Pro_SellBack { get; set; }
        public virtual ICollection<Pro_SellBackIMEIList> Pro_SellBackIMEIList { get; set; }
        public virtual Pro_SellBackInfo_Aduit Pro_SellBackInfo_Aduit { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
    }
}
