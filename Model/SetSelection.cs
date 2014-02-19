using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SetSelection
    {
        private int borowListID;
        private int repairListID;
        private bool sucess;
        
        public bool Sucess
        {
            get { return sucess; }
            set { sucess = value; }
        }
         
        public int RepairListID
        {
            get { return repairListID; }
            set { repairListID = value; }
        }

        public int BorowListID
        {
            get { return borowListID; }
            set { borowListID = value; }
        }

        private string inList;

        public string InList
        {
            get { return inList; }
            set { inList = value; }
        }
        private decimal countnum;

        public decimal Countnum
        {
            get { return countnum; }
            set { countnum = value; }
        }
        private string proid;

        public string Proid
        {
            get { return proid; }
            set { proid = value; }
        }
        private List<string> oldIMEI;

        public List<string> OldIMEI
        {
            get { return oldIMEI; }
            set { oldIMEI = value; }
        }
        List<string> returnIMEI;

        public List<string> ReturnIMEI
        {
            get { return returnIMEI; }
            set { returnIMEI = value; }
        }
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        private string hallID;

        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
        }
        List<string> st;
        public List<string> St
        {
            get { return st; }
            set { st = value; }
        }
    }
}
