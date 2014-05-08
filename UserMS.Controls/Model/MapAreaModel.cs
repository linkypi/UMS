using System.Collections.Generic;
using Telerik.Windows.Controls.Map;

namespace UserMS.Model
{
    public class MapAreaModel
    {
        public int AreaID { get; set; }
        public List<Location> Points { get; set; }
        public string AreaName { get; set; }

    }
}