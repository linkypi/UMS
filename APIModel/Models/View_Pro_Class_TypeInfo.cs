using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_Class_TypeInfo
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
        public string ClassName { get; set; }
        public Nullable<int> Order { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> ClassTypeID { get; set; }
        public string ClassTypeName { get; set; }
    }
}
