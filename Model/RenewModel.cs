using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RenewModel
    {
        private string iMEI;
        private string memberName;
        private string mobilePhone;
        private int  idCard_ID;
        private int validity;
        private decimal renewMoney;
        private string idCard;
        private int renewalTime;
        private string cardType;
        private int vIPID;
        private decimal point;
        private int aduitValidity;
        private decimal aduitMoney;
        private int oldRenewID;
        private int aduitID;
        private bool flag;
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }


        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public int AduitID
        {
            get { return aduitID; }
            set { aduitID = value; }
        }

        public int OldRenewID
        {
            get { return oldRenewID; }
            set { oldRenewID = value; }
        }

        public decimal AduitMoney
        {
            get { return aduitMoney; }
            set { aduitMoney = value; }
        }
        public int AduitValidity
        {
            get { return aduitValidity; }
            set { aduitValidity = value; }
        }
         

        public decimal Point
        {
            get { return point; }
            set { point = value; }
        }

        public int VIPID
        {
            get { return vIPID; }
            set { vIPID = value; }
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        public int RenewalTime
        {
            get { return renewalTime; }
            set { renewalTime = value; }
        }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDCard
        {
            get { return idCard; }
            set { idCard = value; }
        }

        public decimal RenewMoney
        {
            get { return renewMoney; }
            set { renewMoney = value; }
        }

        public int Validity
        {
            get { return validity; }
            set { validity = value; }
        }

        /// <summary>
        /// 证件类型编号
        /// </summary>
        public int IDCard_ID
        {
          get { return idCard_ID; }
          set { idCard_ID = value; }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }

        /// <summary>
        /// 卡号
        /// </summary>
        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }
    }
}
