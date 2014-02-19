using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OutImportModel
    {
        private List<OutImportModelList> children;

        public List<OutImportModelList> Children
        {
            get { return children; }
            set { children = value; }
        }

        private string proformat;

        public string ProFormat
        {
            get { return proformat; }
            set { proformat = value; }
        }


        private string typeName;

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        private int typeID;

        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private int classID;

        public int ClassID
        {
            get { return classID; }
            set { classID = value; }
        }


        private bool success;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        private string checkNote;

        public string CheckNote
        {
            get { return checkNote; }
            set { checkNote = value; }
        }

        private string proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        private bool needIMEI;

        public bool NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
        }

        private string iMEI;

        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }
        private string fromHallID;


        public string FromHallID
        {
            get { return fromHallID; }
            set { fromHallID = value; }
        }

        private string toHallID;

        public string ToHallID
        {
            get { return toHallID; }
            set { toHallID = value; }
        }

    
        private string oldID;

        public string OldID
        {
          get { return oldID; }
          set { oldID = value; }
        }
        private string fromHall;


        public string FromHall
        {
          get { return fromHall; }
          set { fromHall = value; }
        }

        private string  toHall;

        public string ToHall
        {
          get { return toHall; }
          set { toHall = value; }
        }
         private string fromUser;


        public string FromUser
        {
          get { return fromUser; }
          set { fromUser = value; }
        }


        private string proName;


        public string ProName
        {
          get { return proName; }
          set { proName = value; }
        }
        private decimal proCount;


        public decimal ProCount
        {
          get { return proCount; }
          set { proCount = value; }
        }
  
        private string note;


        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private string inListID;

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }

    }
}
