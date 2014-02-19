using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ChangeIMEIInfo
    {
        public int ID { get; set; }
        public Nullable<int> ChangeListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public virtual Pro_ChangeProListInfo Pro_ChangeProListInfo { get; set; }
    }
}
