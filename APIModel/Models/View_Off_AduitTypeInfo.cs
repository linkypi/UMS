using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Off_AduitTypeInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool Flag { get; set; }
        public System.DateTime AddDate { get; set; }
        public string AddUserID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
    }
}
