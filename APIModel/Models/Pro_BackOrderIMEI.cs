using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BackOrderIMEI
    {
        public int ID { get; set; }
        public Nullable<int> BackListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public virtual Pro_BackListInfo Pro_BackListInfo { get; set; }
    }
}
