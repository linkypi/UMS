using System;
using System.Net;

namespace Model
{
    public class IMEIModel
    {
        private string newIMEI;
        private string oldIMEI;
        private string proID;
        private string inListID;

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }
        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        public string OldIMEI
        {
            get { return oldIMEI; }
            set { oldIMEI = value; }
        }

        public string NewIMEI
        {
            get { return newIMEI; }
            set { newIMEI = value; }
        }

        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private string checkNote;

        public string CheckNote
        {
            get { return checkNote; }
            set { checkNote = value; }
        }

    }
}
