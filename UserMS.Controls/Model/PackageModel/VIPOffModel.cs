using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserMS.Model.PackageModel
{
    public class VIPOffModel
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string groupType;

        public string GroupType
        {
            get { return groupType; }
            set { groupType = value; }
        }

        private int groupTypeID;

        public int GroupTypeID
        {
            get { return groupTypeID; }
            set { groupTypeID = value; }
        }


        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        private decimal limit;

        public decimal Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private List<GounpSource> children;

        public List<GounpSource> Children
        {
            get { return children; }
            set { children = value; }
        }
    }
}
