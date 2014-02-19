using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 销售明细
    /// </summary>
    public class Pro_SellListInfo_Child : Pro_SellListInfo
    {
        private ArrayList _ProOffListForCombbox;

        public ArrayList ProOffListForCombbox
        {
            get { return _ProOffListForCombbox; }
            set { _ProOffListForCombbox = value; }
        }
    }
}
