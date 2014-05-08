using System.Windows;
using System.Windows.Controls;
using UserMS.Model;

namespace UserMS.Model
{
    public class GridViewRowSytleSelector:StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ProSellBackGridModel)
            {
                ProSellBackGridModel model = item as ProSellBackGridModel;

                if (model.Status == ProSellBackGridModel.SellListStatus.Back)
                {
                    return RedStyle;
                }
                if (model.Status == ProSellBackGridModel.SellListStatus.New)
                {
                    return GreenStyle;
                }
                return null;


            }
            if (item is ProSellGridModel)
            {
                ProSellGridModel model = item as ProSellGridModel;
                
                if (!model.IsOK)
                {
                    return TomatoStyle;
                }
            }

            if (item is View_SellOffAduitInfo)
            {
                View_SellOffAduitInfo model = item as View_SellOffAduitInfo;
                if (model.IsAduited == "N")
                {
                    return RedStyle;
                }
            }

            return null;
        }
        public Style RedStyle { get; set; }
        public Style GreenStyle { get; set; }
        public Style TomatoStyle { get; set; }
    }
}