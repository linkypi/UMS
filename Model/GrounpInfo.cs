using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public  class GrounpInfo
    {
        public string GrounpName { get; set; }
        public string Note { get; set; }
        public string IsMustName { get; set; }
        public int SellType { get; set; }
        public string SellTypeName { get; set; }
        public int GroupID { get; set; }
        public List<ProModel> ProModel { get; set; }
    }
}
