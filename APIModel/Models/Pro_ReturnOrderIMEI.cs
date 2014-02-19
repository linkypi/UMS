using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ReturnOrderIMEI
    {
        public Pro_ReturnOrderIMEI()
        {
            this.Pro_IMEI = new List<Pro_IMEI>();
        }

        public int ID { get; set; }
        public Nullable<int> ReturnListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
        public virtual Pro_ReturnListInfo Pro_ReturnListInfo { get; set; }
    }
}
