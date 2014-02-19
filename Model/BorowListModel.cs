using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public  class BorowListModel
    {
        private string iMEI;
        private string note;
        private bool sucess;
        private string proClassID;
        private string pro_TypeID;
        private decimal proPrice;
        private string proName;
        private decimal proCount;
        private string proformat;
        private string proID;
        private string typeName;
        private string className;
        private bool needIMEI;
        private string inListID;
        private List<IMEIModel> iIMEIList;
        private int borowListID;
        private decimal aduitCount;
        private string bID;
        private bool isDecimal;

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }


        public string BID
        {
            get { return bID; }
            set { bID = value; }
        }

        private string isReturn;

        public string IsReturn
        {
            get { return isReturn; }
            set { isReturn = value; }
        }
        private decimal returnCount;

        public decimal ReturnCount
        {
            get { return returnCount; }
            set { returnCount = value; }
        }
        private decimal unReturnCount;

        public decimal UnReturnCount
        {
            get { return unReturnCount; }
            set { unReturnCount = value; }
        }

        public decimal AduitCount
        {
            get { return aduitCount; }
            set { aduitCount = value; }
        }


        public int BorowListID
        {
            get { return borowListID; }
            set { borowListID = value; }
        }

        public List<IMEIModel> IIMEIList
        {
            get { return iIMEIList; }
            set { iIMEIList = value; }
        }

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }

        public bool Sucess
        {
            get { return sucess; }
            set { sucess = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
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
        /// 商品属性
        /// </summary>
        public string ProFormat
        {
            get { return proformat; }
            set { proformat = value; }
        }

        /// <summary>
        /// 商品型号
        /// </summary>
        public string Pro_ClassID
        {
            get { return proClassID; }
            set { proClassID = value; }
        }

        /// <summary>
        /// 商品类别
        /// </summary>
        public string Pro_TypeID
        {
            get { return pro_TypeID; }
            set { pro_TypeID = value; }
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

        /// <summary>
        /// 零售价格
        /// </summary>
        public decimal ProPrice
        {
            get { return proPrice; }
            set { proPrice = value; }
        }

        public bool NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
        }


        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
    }
}
