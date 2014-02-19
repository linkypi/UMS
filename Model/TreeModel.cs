using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public  class TreeModel
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }


        private List<TreeModel> children;

        public List<TreeModel> Children
        {
            get { return children; }
            set { children = value; }
        }

    }
}
