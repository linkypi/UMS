using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OutImportModelList
    {
        private List<IMEIModel> iMEIList;

        public List<IMEIModel> IMEIList
        {
            get { return iMEIList; }
            set { iMEIList = value; }
        }

        private string proName;

        public string ProName
        {
            get { return proName; }
            set { proName = value; }
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

  
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private string proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        private decimal proCount;


        public decimal ProCount
        {
            get { return proCount; }
            set { proCount = value; }
        }
        private string proFormat;

        public string ProFormat
        {
            get { return proFormat; }
            set { proFormat = value; }
        }

        private string inListID;

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }

        private bool needIMEI;

        public bool NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
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



    }
}
