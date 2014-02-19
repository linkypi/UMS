using System;
using System.Collections.Generic;

namespace SlModel
{
    /// <summary>
    /// 进库实体
    /// </summary>
    public class ViewModel:BaseModel
    {
        public decimal PCount { get; set; }
        private bool isNeedIMEI;

        /// <summary>
        /// 是否需要串码
        /// </summary>
        public bool IsNeedIMEI
        {
            get { return isNeedIMEI; }
            set { isNeedIMEI = value; }
        }
        private string hallID;
        private string inlist;
        private string note;
        private bool sucess;
   
        private int borowListID;
        private string aduitID;
        private string orderID;
        private string fromHallID;
        private DateTime date;
        private string fromHallName;
        private string userName;
        private decimal repairReturnCount;
        private int repairListID;
        private decimal aduitCount;
        private decimal? price;

        public decimal? Price
        {
            get { return price; }
            set { price = value; }
        }

        public decimal AduitCount
        {
            get { return aduitCount; }
            set { aduitCount = value; }
        }

     
        public int RepairListID
        {
            get { return repairListID; }
            set { repairListID = value; }
        }

        /// <summary>
        /// 送修返还数量
        /// </summary>
        public decimal RepairReturnCount
        {
            get { return repairReturnCount; }
            set { repairReturnCount = value; }
        }


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
        private string touserID;

        public string TouserID
        {
            get { return touserID; }
            set { touserID = value; }
        }

        private string touserName;

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

        public int BorowListID
        {
            get { return borowListID; }
            set { borowListID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Sucess
        {
            get
            {
                return sucess;
            }
            set
            {
                sucess = value;
                if (Sucess)
                {
                    note = "成功";
                }
                else
                {
                    note = "失败";
                }
            
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }


        public string Inlist
        {
            get { return inlist; }
            set { inlist = value; }
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
    }
}
