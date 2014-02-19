using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BorowOrderIMEI
    {
        public Pro_BorowOrderIMEI()
        {
            this.Pro_IMEI = new List<Pro_IMEI>();
        }

        public int ID { get; set; }
        public Nullable<int> BorowListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsReturn { get; set; }
        public virtual Pro_BorowListInfo Pro_BorowListInfo { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
    }
}
