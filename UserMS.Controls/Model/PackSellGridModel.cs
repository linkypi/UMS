using System.Collections.Generic;

namespace UserMS.Model
{
    public class PackSellGridModel:ProSellGridModel
    {
         public string GridTemplate { get; set; }
        public object GridTemplateData { get; set; }
        public Dictionary<int,List<API.Pro_ProInfo>> ProInfos { get; set; }
       // public List<API.Pro_ProInfo> ProInfos { get; set; }
        public API.Package_GroupInfo GroupInfo { get; set; }
        public int package_proinfoID { get; set; }
        public List<API.Pro_SellList_RulesInfo> Rules { get; set; }
        public string GroupName
        {
            get
            {
                if (GroupInfo != null)
                {
                    return GroupInfo.GroupName;
                }
                return "";
            }
        }
    }
}