using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_PackageSalesNameInfo
    {
        public Nullable<int> Parent { get; set; }
        public string Note { get; set; }
        public string SalesName { get; set; }
        public int ID { get; set; }
        public string OldSalesName { get; set; }
        public string OldNote { get; set; }
    }
}
