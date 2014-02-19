using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellListInfo
    {
        public Pro_SellListInfo()
        {
            this.Pro_CashTicket = new List<Pro_CashTicket>();
            this.Pro_Sell_Car = new List<Pro_Sell_Car>();
            this.Pro_Sell_JiPeiKa = new List<Pro_Sell_JiPeiKa>();
            this.Pro_Sell_Service = new List<Pro_Sell_Service>();
            this.Pro_Sell_Yanbao = new List<Pro_Sell_Yanbao>();
            this.Pro_SellBackAduitList = new List<Pro_SellBackAduitList>();
            this.Pro_SellBackList = new List<Pro_SellBackList>();
            this.Pro_SellIMEIList = new List<Pro_SellIMEIList>();
            this.Pro_SellList_RulesInfo = new List<Pro_SellList_RulesInfo>();
            this.Pro_SellSendInfo = new List<Pro_SellSendInfo>();
            this.VIP_VIPInfo = new List<VIP_VIPInfo>();
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
        public decimal OtherCash { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public decimal AnBu { get; set; }
        public decimal LieShou { get; set; }
        public decimal LieShouPrice { get; set; }
        public decimal OtherOff { get; set; }
        public Nullable<int> SellAduitID { get; set; }
        public Nullable<int> BackAduitID { get; set; }
        public Nullable<int> OffAduitListID { get; set; }
        public decimal AnBuPrice { get; set; }
        public decimal YanbaoModelPrice { get; set; }
        public bool NeedAduit { get; set; }
        public string ClassType { get; set; }
        public Nullable<int> ProOffListID { get; set; }
        public decimal RulesShowToCus { get; set; }
        public decimal RulesUnShowToCus { get; set; }
        public decimal RulesGetBack { get; set; }
        public decimal RulesUnGetBack { get; set; }
        public virtual ICollection<Pro_CashTicket> Pro_CashTicket { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual ICollection<Pro_Sell_Car> Pro_Sell_Car { get; set; }
        public virtual ICollection<Pro_Sell_JiPeiKa> Pro_Sell_JiPeiKa { get; set; }
        public virtual ICollection<Pro_Sell_Service> Pro_Sell_Service { get; set; }
        public virtual ICollection<Pro_Sell_Yanbao> Pro_Sell_Yanbao { get; set; }
        public virtual Pro_SellAduit Pro_SellAduit { get; set; }
        public virtual Pro_SellBack Pro_SellBack { get; set; }
        public virtual ICollection<Pro_SellBackAduitList> Pro_SellBackAduitList { get; set; }
        public virtual Pro_SellBackInfo_Aduit Pro_SellBackInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellBackList> Pro_SellBackList { get; set; }
        public virtual ICollection<Pro_SellIMEIList> Pro_SellIMEIList { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual Pro_SellInfo_Aduit Pro_SellInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellList_RulesInfo> Pro_SellList_RulesInfo { get; set; }
        public virtual Pro_SellOffAduitInfoList Pro_SellOffAduitInfoList { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
        public virtual Pro_SellListServiceInfo Pro_SellListServiceInfo { get; set; }
        public virtual Pro_SellSpecalOffList Pro_SellSpecalOffList { get; set; }
        public virtual Pro_SellTypeProduct Pro_SellTypeProduct { get; set; }
        public virtual ICollection<Pro_SellSendInfo> Pro_SellSendInfo { get; set; }
        public virtual ICollection<VIP_VIPInfo> VIP_VIPInfo { get; set; }
    }
}
