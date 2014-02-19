using System;
using System.Collections.Generic;
using System.Net;

namespace Model
{
    public class AduitModel
    {
        private string aduitID;
        private string applyUser;
        private DateTime applyDate;
        private bool passed;
        private string note;
        private int id;
        private string hallID;
        private string hallName;
        private decimal money;
        private string memberName;
        private int validity;

        public int Validity
        {
            get { return validity; }
            set { validity = value; }
        }

        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }

        public decimal Money
        {
            get { return money; }
            set { money = value; }
        }

        public string HallName
        {
            get { return hallName; }
            set { hallName = value; }
        }

        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
        }

        private List<AduitListInfo> aduitListInfo;

        public List<AduitListInfo> AduitListInfo
        {
            get 
            {
                if (aduitListInfo == null)
                {
                    aduitListInfo = new List<AduitListInfo>();
                }
                return aduitListInfo; 
            }
            set { aduitListInfo = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public bool Passed
        {
            get { return passed; }
            set { passed = value; }
        }

        public DateTime ApplyDate
        {
            get { return applyDate; }
            set { applyDate = value; }
        }

        public string ApplyUser
        {
            get { return applyUser; }
            set { applyUser = value; }
        }

        public string AduitID
        {
            get { return aduitID; }
            set { aduitID = value; }
        }

    }
}
