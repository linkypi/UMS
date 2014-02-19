using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Sell_Back_NewSpecialOffListInfo
    {
        public string SellID { get; set; }
        public string OldID { get; set; }
        public Nullable<int> Pro_ClassID { get; set; }
        public string ClassName { get; set; }
        public Nullable<int> Pro_typeid { get; set; }
        public string typename { get; set; }
        public string proname { get; set; }
        public string proformat { get; set; }
        public int procount { get; set; }
        public Nullable<int> sellType { get; set; }
        public string selltypename { get; set; }
        public Nullable<int> proprice { get; set; }
        public Nullable<int> RealPrice { get; set; }
        public Nullable<int> anbu { get; set; }
        public Nullable<int> AnBuPrice { get; set; }
        public string imei { get; set; }
        public string ticketid { get; set; }
        public Nullable<int> cashticket { get; set; }
        public Nullable<int> ticketused { get; set; }
        public Nullable<int> offpoint { get; set; }
        public string offname { get; set; }
        public Nullable<int> offprice { get; set; }
        public string sepecialoffname { get; set; }
        public Nullable<decimal> offsepecialprice { get; set; }
        public Nullable<int> wholesaleoffprice { get; set; }
        public Nullable<int> OtherOff { get; set; }
        public Nullable<int> OtherCash { get; set; }
        public Nullable<int> LieShouPrice { get; set; }
        public string isfree { get; set; }
        public Nullable<decimal> cashprice { get; set; }
        public string ProID { get; set; }
        public string hallid { get; set; }
        public string Seller { get; set; }
        public Nullable<int> Vip_ID { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string NOte { get; set; }
        public Nullable<System.DateTime> sellDate { get; set; }
    }
}
