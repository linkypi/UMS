using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserMS.Model
{
    public class RepairNote
    {
        private string note;
        private int iD;
        private int newid;
        

        public int NewID
        {
            get { return newid; }
            set { newid = value; }
        }
        private static  int identity =1;

        public RepairNote(string id, int klas)
        {
            note = id;
            iD = identity++;
            newid = klas;
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

        public static List<RepairNote> Generate(string str, int kls)
        {
            List<string> arr = str.Split(",".ToArray()).ToList();
            List<RepairNote> list = new List<RepairNote>();
            foreach (var item in arr)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                RepairNote rp = new RepairNote(item,kls);
                list.Add(rp);
            }
            return list;
        }

    }

}
