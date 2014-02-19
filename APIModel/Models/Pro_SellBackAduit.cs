using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellBackAduit
    {
        public Pro_SellBackAduit()
        {
            this.Pro_SellBackAduitList = new List<Pro_SellBackAduitList>();
            this.Pro_SellBackAduitOffList = new List<Pro_SellBackAduitOffList>();
        }

        public int ID { get; set; }
        public string AduitUser { get; set; }
        public Nullable<System.DateTime> AduitDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public bool Aduited { get; set; }
        public bool Passed { get; set; }
        public bool Used { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Note { get; set; }
        public decimal AduitMoney { get; set; }
        public string HallID { get; set; }
        public string AduitID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<decimal> ApplyMoney { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public Nullable<int> VIPID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual ICollection<Pro_SellBackAduitList> Pro_SellBackAduitList { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
        public virtual ICollection<Pro_SellBackAduitOffList> Pro_SellBackAduitOffList { get; set; }
    }
}
