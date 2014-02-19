using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class SMS_SellBack
    {
        public int ID { get; set; }
        public string SellBackID { get; set; }
        public Nullable<int> SellID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public string AduitID { get; set; }
        public decimal BackMoney { get; set; }
        public Nullable<decimal> BackCount { get; set; }
        public string BillID { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
    }
}
