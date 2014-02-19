using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_AirOutInfo
    {
        public Pro_AirOutInfo()
        {
            this.Pro_AirOutListInfo = new List<Pro_AirOutListInfo>();
        }

        public int ID { get; set; }
        public string FromHallID { get; set; }
        public string Pro_HallID { get; set; }
        public string UserID { get; set; }
        public string OutOrderID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> OutDate { get; set; }
        public string FromUserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Note { get; set; }
        public string ToUserID { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
        public Nullable<bool> Audit { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public virtual ICollection<Pro_AirOutListInfo> Pro_AirOutListInfo { get; set; }
    }
}
