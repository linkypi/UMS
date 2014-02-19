using System;
using System.Collections.Generic;

namespace SlModel
{
    public class OutViewModel:BaseModel
    {
        
        private string proClassID;
        private string pro_TypeID;
        private decimal proPrice;
        private string proName;
        private int proCount;
        private string proformat;
        private string proID;
        private string hallID;
        private string note;
        private string aduitID;
        private string orderID;
        private string fromHallID;
        private DateTime date;
        private string fromHallName;
        private string userName;
        private string touserID;
        private string touserName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public string TouserID
        {
            get { return touserID; }
            set { touserID = value; }
        }

        public string TouserName
        {
            get { return touserName; }
            set { touserName = value; }
        }
        public string FromHallName
        {
            get { return fromHallName; }
            set { fromHallName = value; }
        }
        private string pro_hallName;
        public string Pro_hallName
        {
            get { return pro_hallName; }
            set { pro_hallName = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string FromHallID
        {
            get { return fromHallID; }
            set { fromHallID = value; }
        }

        public string OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public string AduitID
        {
            get { return aduitID; }
            set { aduitID = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        private List<string> iMEI;
        /// <summary>
        /// 串号
        /// </summary>
        public List<string> IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
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
        public int ProCount
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
    }
}
