using UserMS.API;

namespace UserMS.Model
{
    public class ProSellCarGridModel:ProSellGridModel
    {
         public string CName { get; set; }
        public string CID { get; set; }
        public bool isOther { get; set; }
        public string Address { get; set; }
        public string Desc { get; set; }


       
    }
}