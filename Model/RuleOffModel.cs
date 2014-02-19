using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RuleOffModel
    {
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private bool flag;

        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        private string deleter;

        public string Deleter
        {
            get { return deleter; }
            set { deleter = value; }
        }

        private DateTime deleteDate;

        public DateTime DeleteDate
        {
            get { return deleteDate; }
            set { deleteDate = value; }
        }

    }
}
