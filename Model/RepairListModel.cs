using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RepairListModel
    {

        private string proName;
        private decimal proCount;
        private string proID;
        private string typeName;
        private string className;
        private string inListID;
        private string note;
        private string iMEI;
        private int repairID;
        private int repairListID;
        private string newIMEI;
        private bool needIMEI;
        private decimal backCount;
        private string proFormat;
        private bool isdecimal;

        public bool IsDecimal
        {
            get { return isdecimal; }
            set { isdecimal = value; }
        }

        public string ProFormat
        {
            get { return proFormat; }
            set { proFormat = value; }
        }

        public decimal BackCount
        {
            get { return backCount; }
            set { backCount = value; }
        }

        public bool NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
        }

        public string NewIMEI
        {
            get { return newIMEI; }
            set { newIMEI = value; }
        }

        public int RepairListID
        {
            get { return repairListID; }
            set { repairListID = value; }
        }

        public int RepairID
        {
            get { return repairID; }
            set { repairID = value; }
        }

        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        /// <summary>
        /// 商品品牌
        /// </summary>
        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }

        /// <summary>
        /// 商品数量
        /// </summary>
        public decimal ProCount
        {
            get { return proCount; }
            set { proCount = value; }
        }

        public string ProClassName
        {
            get { return className; }
            set { className = value; }
        }

        public string ProTypeName
        {
            get { return typeName; }
            set { typeName = value; }

        }
    }
}
