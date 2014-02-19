using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public  class SeleterModel
    {
        public string ShowCount { get; set; }
        public decimal Count { get; set; }
        public bool? ISdecimals { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public decimal Point { get; set; }
        public decimal ReduceMoney { get; set; }
        public decimal Salary { get; set; }
        public string IsNeedMoreorLess { get; set; }
        public List<SelecterIMEI> NewIMEI { get; set; }
        public decimal RetailPrice { get; set; }
        private string hallID;
        /// <summary>
        /// 仓库ID
        /// </summary>
        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
        }
        private string proID;
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }
        private string proName;
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }
        private string proTypeName;
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string ProTypeName
        {
            get { return proTypeName; }
            set { proTypeName = value; }
        }
        private string proClassName;
        /// <summary>
        /// 类别名称
        /// </summary>
        public string ProClassName
        {
            get { return proClassName; }
            set { proClassName = value; }
        }
        private int proCount;
        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProCount
        {
            get { return proCount; }
            set { proCount = value; }
        }
        private string proInListID;
        /// <summary>
        /// 批次号
        /// </summary>

        public string ProInListID
        {
            get { return proInListID; }
            set { proInListID = value; }
        }
        List<SelecterIMEI> isIMEI;
        /// <summary>
        /// 检货串码
        /// </summary>
        public List<SelecterIMEI> IsIMEI
        {
            get { return isIMEI; }
            set { isIMEI = value; }
        }
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        private int classID;

        public int ClassID
        {
            get { return classID; }
            set { classID = value; }
        }
        private int typeID;

        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }
  
        private string proFormat;

        public string ProFormat
        {
            get { return proFormat; }
            set { proFormat = value; }
        }
        private string sellTypeName;

        public string SellTypeName
        {
            get { return sellTypeName; }
            set { sellTypeName = value; }
        }
  
        private int sellTypeID;

        public int SellTypeID
        {
            get { return sellTypeID; }
            set { sellTypeID = value; }
        }
        private decimal offPrice;

        public decimal OffPrice
        {
            get { return offPrice; }
            set { offPrice = value; }
        }
        private string needIMEI;
        /// <summary>
        /// 需要串码
        /// </summary>
        public string NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
        }
        private string isService;
        /// <summary>
        /// 属于服务
        /// </summary>
        public string IsService
        {
            get { return isService; }
            set { isService = value; }
        }
        public bool IsNeedIMEI { get; set; }
        public bool IsServicePro { get; set; }

        #region 新的商品
        public string NewProID { get; set; }
        public string NewProName{get;set;}
        public string NewTypeName { get; set; }
        public string NewClassName{get;set;}
        public string NewInListID { get; set;}
        public string NewNote { get; set; }
        public int NewClassID { get; set; }
        public int NewTypeID { get; set; }
        public string NewProFormat { get; set; }
        public string NewSellTypeName { get; set; }
        public string NewPrice { get; set; }
        public int NewSellTypeID { get; set; }
        public bool NewIsNeedIMEI { get; set; }
        public decimal NewCount { get; set; }
        #endregion 
    }
}
