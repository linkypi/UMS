using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_PriceChange
    {
        public Pro_PriceChange()
        {
            this.Pro_PriceChangeList = new List<Pro_PriceChangeList>();
            this.Pro_YanbaoPriceStepInfo_bak = new List<Pro_YanbaoPriceStepInfo_bak>();
            this.Pro_YanbaoPriceStepInfo = new List<Pro_YanbaoPriceStepInfo>();
        }

        public int ID { get; set; }
        public string ChangeID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Pro_PriceChangeList> Pro_PriceChangeList { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<Pro_YanbaoPriceStepInfo_bak> Pro_YanbaoPriceStepInfo_bak { get; set; }
        public virtual ICollection<Pro_YanbaoPriceStepInfo> Pro_YanbaoPriceStepInfo { get; set; }
    }
}
