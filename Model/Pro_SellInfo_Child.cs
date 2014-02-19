using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 销售单
    /// </summary>
    public class Pro_SellInfo_Child : Pro_SellInfo
    {
        private List<VIP_OffList> _SellOffListForCombbox;
        private List<VIP_OffList> _SellSpecielListForCombbox;
        private List<VIP_OffList> _SellTicketListForCombbox;
        private List<Pro_SellListInfo> _SellListChild;

        public List<Pro_SellListInfo> SellListChild
        {
            get { return _SellListChild; }
            set { _SellListChild = value; }
        }

        public List<VIP_OffList> SellTicketListForCombbox
        {
            get { return _SellTicketListForCombbox; }
            set { _SellTicketListForCombbox = value; }
        }

        public List<VIP_OffList> SellSpecielListForCombbox
        {
            get { return _SellSpecielListForCombbox; }
            set { _SellSpecielListForCombbox = value; }
        }

        public List<VIP_OffList> SellOffListForCombbox
        {
            get { return _SellOffListForCombbox; }
            set { _SellOffListForCombbox = value; }
        }
    }
}
