using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_ProInfo
    {
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
        public string TypeName { get; set; }
        public string ClassName { get; set; }
        public string HasNeedIMEI { get; set; }
        public string HasNeedMoreorLess { get; set; }
        public string HasService { get; set; }
        public string HasDecimals { get; set; }
        public bool ISdecimals { get; set; }
        public string PrintName { get; set; }
        public Nullable<int> ProMainID { get; set; }
        public Nullable<int> YanBaoModelID { get; set; }
        public string AirHallID { get; set; }
        public string HallName { get; set; }
    }
}
