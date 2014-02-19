namespace SlModel
{
    public class BaseModel
    {
        private static int identity;
        private int id;

        private string proClassID;
        private string pro_TypeID;
        private decimal proPrice;
        private string proName;
        private decimal proCount;
        private string proformat;
        private string proID;
        private string proClassName;
        private string proTypeName;
        private bool isDecimal;
        public string InListID { get;set;}
        public string IMEI { get; set; }

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }

        public string TypeName
        {
            get { return proTypeName; }
            set { proTypeName = value; }
        } 

        public string ClassName
        {
            get { return proClassName; }
            set { proClassName = value; }
        }


        public BaseModel()
        {
            id = identity++;
        }

        public int ID
        {
            get { return id; }
        }

        private bool needIMEI;

        public bool NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
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
        /// 单卖价格
        /// </summary>
        public decimal ProPrice
        {
            get { return proPrice; }
            set { proPrice = value; }
        }
    }
}
