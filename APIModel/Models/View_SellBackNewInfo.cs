using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SellBackNewInfo
    {
        public string SellBackID { get; set; }
        public string BillID { get; set; }
        public Nullable<int> Pro_ClassID { get; set; }
        public string ClassName { get; set; }
        public Nullable<int> Pro_TypeID { get; set; }
        public string TypeName { get; set; }
        public string ProName { get; set; }
        public string ProFormat { get; set; }
        public Nullable<decimal> procount { get; set; }
        public Nullable<int> SellType { get; set; }
        public string SellTypeName { get; set; }
        public Nullable<decimal> proprice { get; set; }
        public Nullable<decimal> Realproprice { get; set; }
        public string IMEI { get; set; }
        public string TicketID { get; set; }
        public Nullable<decimal> CashTicket { get; set; }
        public Nullable<decimal> TicketUsed { get; set; }
        public Nullable<decimal> OffPoint { get; set; }
        public string OffName { get; set; }
        public Nullable<decimal> OffPrice { get; set; }
        public string SepecialOffName { get; set; }
        public Nullable<decimal> OffSepecialPrice { get; set; }
        public Nullable<decimal> WholeSaleOffPrice { get; set; }
        public Nullable<decimal> otheroff { get; set; }
        public Nullable<decimal> OtherCash { get; set; }
        public string IsFree { get; set; }
        public Nullable<decimal> CashPrice { get; set; }
        public string ProID { get; set; }
        public string HallID { get; set; }
        public string Seller { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public Nullable<decimal> anbu { get; set; }
        public Nullable<decimal> lieshou { get; set; }
        public Nullable<decimal> daixiaofei { get; set; }
        public Nullable<int> ClassTypeID { get; set; }
        public string ClassTypeName { get; set; }
    }
}
