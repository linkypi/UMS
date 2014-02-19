using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PropertyValueModel
    {
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        private string pvalue;

        public string Pvalue
        {
            get { return pvalue; }
            set { pvalue = value; }
        }

        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

    }
}
