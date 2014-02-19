using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ProInfo
    {
        public Pro_ProInfo()
        {
            this.Pro_BorowAduitList = new List<Pro_BorowAduitList>();
            this.Pro_BorowListInfo = new List<Pro_BorowListInfo>();
            this.Pro_ChangeProListInfo = new List<Pro_ChangeProListInfo>();
            this.Pro_ChangeProListInfo1 = new List<Pro_ChangeProListInfo>();
            this.Pro_IMEI = new List<Pro_IMEI>();
            this.Pro_InOrderList = new List<Pro_InOrderList>();
            this.Pro_PriceChangeList = new List<Pro_PriceChangeList>();
            this.SMS_SignPayInListInfo = new List<SMS_SignPayInListInfo>();
            this.VIP_ProOffList = new List<VIP_ProOffList>();
            this.Pro_ProProperty_F = new List<Pro_ProProperty_F>();
            this.Pro_RepairReturnListInfo = new List<Pro_RepairReturnListInfo>();
            this.Pro_SellTypeProduct_bak = new List<Pro_SellTypeProduct_bak>();
            this.Pro_SellBackList = new List<Pro_SellBackList>();
            this.Pro_SellTypeProduct = new List<Pro_SellTypeProduct>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
            this.Pro_SellAduitList = new List<Pro_SellAduitList>();
            this.Pro_SellSendInfo = new List<Pro_SellSendInfo>();
            this.VIP_SendProOffList = new List<VIP_SendProOffList>();
            this.Pro_StoreInfo = new List<Pro_StoreInfo>();
            this.Sys_SalaryList_bak = new List<Sys_SalaryList_bak>();
            this.Sys_SalaryList = new List<Sys_SalaryList>();
            this.Sys_SalaryCurrentList = new List<Sys_SalaryCurrentList>();
            this.VIP_VIPService = new List<VIP_VIPService>();
            this.VIP_VIPTypeService_BAK = new List<VIP_VIPTypeService_BAK>();
            this.VIP_VIPTypeService = new List<VIP_VIPTypeService>();
            this.Pro_Sell_Service = new List<Pro_Sell_Service>();
        }

        public string ProID { get; set; }
        public Nullable<int> Pro_TypeID { get; set; }
        public Nullable<int> Pro_ClassID { get; set; }
        public int VIP_TypeID { get; set; }
        public string ProName { get; set; }
        public bool NeedIMEI { get; set; }
        public Nullable<System.DateTime> SepDate { get; set; }
        public bool BeforeSep { get; set; }
        public decimal BeforeRate { get; set; }
        public bool AfterSep { get; set; }
        public decimal AfterRate { get; set; }
        public decimal TicketLevel { get; set; }
        public decimal BeforeTicket { get; set; }
        public decimal AfterTicket { get; set; }
        public string ValueIDList { get; set; }
        public bool IsService { get; set; }
        public string Note { get; set; }
        public Nullable<bool> NeedMoreorLess { get; set; }
        public string ProFormat { get; set; }
        public Nullable<int> ProMainID { get; set; }
        public string PrintName { get; set; }
        public bool ISdecimals { get; set; }
        public Nullable<int> YanBaoModelID { get; set; }
        public string AirHallID { get; set; }
        public Nullable<int> Pro_ClassTypeID { get; set; }
        public virtual ICollection<Pro_BorowAduitList> Pro_BorowAduitList { get; set; }
        public virtual ICollection<Pro_BorowListInfo> Pro_BorowListInfo { get; set; }
        public virtual ICollection<Pro_ChangeProListInfo> Pro_ChangeProListInfo { get; set; }
        public virtual ICollection<Pro_ChangeProListInfo> Pro_ChangeProListInfo1 { get; set; }
        public virtual Pro_ClassInfo Pro_ClassInfo { get; set; }
        public virtual Pro_ClassType Pro_ClassType { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
        public virtual ICollection<Pro_InOrderList> Pro_InOrderList { get; set; }
        public virtual ICollection<Pro_PriceChangeList> Pro_PriceChangeList { get; set; }
        public virtual ICollection<SMS_SignPayInListInfo> SMS_SignPayInListInfo { get; set; }
        public virtual Pro_TypeInfo Pro_TypeInfo { get; set; }
        public virtual VIP_VIPType VIP_VIPType { get; set; }
        public virtual Pro_ProMainInfo Pro_ProMainInfo { get; set; }
        public virtual ICollection<VIP_ProOffList> VIP_ProOffList { get; set; }
        public virtual ICollection<Pro_ProProperty_F> Pro_ProProperty_F { get; set; }
        public virtual ICollection<Pro_RepairReturnListInfo> Pro_RepairReturnListInfo { get; set; }
        public virtual ICollection<Pro_SellTypeProduct_bak> Pro_SellTypeProduct_bak { get; set; }
        public virtual ICollection<Pro_SellBackList> Pro_SellBackList { get; set; }
        public virtual ICollection<Pro_SellTypeProduct> Pro_SellTypeProduct { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual ICollection<Pro_SellAduitList> Pro_SellAduitList { get; set; }
        public virtual ICollection<Pro_SellSendInfo> Pro_SellSendInfo { get; set; }
        public virtual ICollection<VIP_SendProOffList> VIP_SendProOffList { get; set; }
        public virtual ICollection<Pro_StoreInfo> Pro_StoreInfo { get; set; }
        public virtual ICollection<Sys_SalaryList_bak> Sys_SalaryList_bak { get; set; }
        public virtual ICollection<Sys_SalaryList> Sys_SalaryList { get; set; }
        public virtual ICollection<Sys_SalaryCurrentList> Sys_SalaryCurrentList { get; set; }
        public virtual ICollection<VIP_VIPService> VIP_VIPService { get; set; }
        public virtual ICollection<VIP_VIPTypeService_BAK> VIP_VIPTypeService_BAK { get; set; }
        public virtual ICollection<VIP_VIPTypeService> VIP_VIPTypeService { get; set; }
        public virtual ICollection<Pro_Sell_Service> Pro_Sell_Service { get; set; }
    }
}
