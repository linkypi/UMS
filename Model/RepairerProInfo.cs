using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RepairerProInfo
    {
        public string RepairerID{get;set;}
        public string Name{ get; set; }
        public bool Dirty { get; set; }
        public List<Model.TypeInfo> Children {get;set; }
    }

    public class TypeInfo 
    {
        public string TypeID{get;set;}
        public string TypeName{ get; set; }
    }
}
