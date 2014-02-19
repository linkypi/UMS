using System;
using System.Net;

namespace Model
{
    public class AduitListInfo
    {
        private int id;
        private decimal proCount;
        private string proName;
        private string proID;
        private string hallName;

        private string proFormat;

        public string ProFormat
        {
            get { return proFormat; }
            set { proFormat = value; }
        }
        private string typeName;
        private string className;
        private decimal offMoney;
        private string note;
        private bool needIMEI;
        private decimal proCost;

        public decimal ProCost
        {
            get { return proCost; }
            set { proCost = value; }
        }
        private bool isDecimal;

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }

        public bool NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
        }

        private int sellTypeID;

        public int SellTypeID
        {
            get { return sellTypeID; }
            set { sellTypeID = value; }
        }

        private decimal newPrice;

        public decimal NewPrice
        {
            get { return newPrice; }
            set { newPrice = value; }
        }

        private decimal maxPrice;

        public decimal MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

        private decimal minPrice;

        public decimal MinPrice
        {
            get { return minPrice; }
            set { minPrice = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private decimal proPrice;

        public decimal ProPrice
        {
            get { return proPrice; }
            set { proPrice = value; }
        }


        public decimal OffMoney
        {
            get { return offMoney; }
            set { offMoney = value; }
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

        public string HallName
        {
            get { return hallName; }
            set { hallName = value; }
        }

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }
 
        public decimal ProCount
        {
            get { return proCount; }
            set { proCount = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

    }
}
