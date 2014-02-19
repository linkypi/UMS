using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RepairModel
    {
        private int iD;
        private string repairID;
        private DateTime repairDate;
        private string userName;
        private string hallName;
        private string note;
        private string hallID;
        private string userID;
        private DateTime sysDate;

        public DateTime SysDate
        {
            get { return sysDate; }
            set { sysDate = value; }
        }

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
        }


        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string HallName
        {
            get { return hallName; }
            set { hallName = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public DateTime RepairDate
        {
            get { return repairDate; }
            set { repairDate = value; }
        }

        public string RepairID
        {
            get { return repairID; }
            set { repairID = value; }
        }

    }
}
