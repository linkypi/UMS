using System;

namespace SlModel
{
    public class VIPModel
    {
        private int iD;
        private string aduitMoney;
        /// <summary>
        /// 申请金额
        /// </summary>
        public string  AduitMoney
        {
            get { return aduitMoney; }
            set { aduitMoney = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        private string memberName;

        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }
        private string sex;

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        private DateTime birthday;

        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }
        private string mobiPhone;

        public string MobiPhone
        {
            get { return mobiPhone; }
            set { mobiPhone = value; }
        }
        private string telePhone;

        public string TelePhone
        {
            get { return telePhone; }
            set { telePhone = value; }
        }
        private string qQ;

        public string QQ
        {
            get { return qQ; }
            set { qQ = value; }
        }
        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private string iDCard;

        public string IDCard
        {
            get { return iDCard; }
            set { iDCard = value; }
        }
        private int iDCard_ID;

        public int IDCard_ID
        {
            get { return iDCard_ID; }
            set { iDCard_ID = value; }
        }
        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private string seller;

        public string Seller
        {
            get { return seller; }
            set { seller = value; }
        }
        private string typeName;

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        private decimal cost_production;

        public decimal Cost_production
        {
            get { return cost_production; }
            set { cost_production = value; }
        }
        private string sBalance;

        public string SBalance
        {
            get { return sBalance; }
            set { sBalance = value; }
        }
        private string sPoint;

        public string SPoint
        {
            get { return sPoint; }
            set { sPoint = value; }
        }
        private int validity;

        public int Validity
        {
            get { return validity; }
            set { validity = value; }
        }

        private string iMEI;

        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }

        private string updUser;

        public string UpdUser
        {
            get { return updUser; }
            set { updUser = value; }
        }
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        //private string iDCardName;
        ///// <summary>
        ///// 证件名称
        ///// </summary>
        //public string IDCardName
        //{
        //    get { return iDCardName; }
        //    set { iDCardName = value; }
        //}

    }
}
