using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIPOffListAduit
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int Type { get; set; }
        public decimal ArriveMoney { get; set; }
        public string Note { get; set; }
        public Nullable<int> VIPTicketMaxCount { get; set; }
        public string SalesName { get; set; }
        public int HeadID { get; set; }
    }
}
