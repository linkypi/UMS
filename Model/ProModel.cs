using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProModel
    {
        public string AirHallID { get; set; }
        public decimal AfterPrice { get; set; }
        public decimal OffPoint { get; set; }
        public decimal OffMoney { get; set; }
        public decimal OffRate { get; set; }
        public decimal Salary { get; set; }
        public int YanBaoModelID { get; set; }
        public int ProMainID { get; set; }
        public string Isdecimal { get; set; }
        //public bool NeedMoreorLess { get; set; }
        public string IsNeedMoreorLess { get; set; }
        public DateTime? SepDate { get; set; }
        //public bool BeforeSep { get; set; }
        public string BeforeSep { get; set; }

        public decimal BeforeRate { get; set; }      
        //public bool AfterSep { get; set; }
        public string AfterSep { get; set; }

        public decimal AfterRate { get; set; }
        public decimal TicketLevel { get; set; }
        public decimal BeforeTicket { get; set; }
        public decimal AfterTicket { get; set; }
        public string PrintName { get; set; }
        public string Introduction { get; set; }
        private int proID;
        


        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProID
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
        private bool isNeedIMEI;

        public bool IsNeedIMEI
        {
            get { return isNeedIMEI; }
            set { isNeedIMEI = value; }
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
        private string price;

        public string Price
        {
            get { return price; }
            set { price = value; }
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
        List<PropertyModel> propertyModel;

        public List<PropertyModel> PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }
        private int vIPTypeID;
        /// <summary>
        /// 会员卡类别ID
        /// </summary>
        public int VIPTypeID
        {
            get { return vIPTypeID; }
            set { vIPTypeID = value; }
        }



        public string AssetFrom { get; set; }
        public int? AssetPeriod { get; set; } 
        public decimal? AssetRate { get; set; }
        public decimal? AssetPrice { get; set; }
        public decimal? AssetValue { get; set; }
        public bool? AssetFinish { get; set; } 
        public string AssetStatus { get; set; }

    }
}

