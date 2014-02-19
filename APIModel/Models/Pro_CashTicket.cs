using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_CashTicket
    {
        public int ID { get; set; }
        public Nullable<int> SellListID { get; set; }
        public string TicketID { get; set; }
        public Nullable<bool> IsBack { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
    }
}
