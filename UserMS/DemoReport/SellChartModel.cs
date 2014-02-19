using System.ComponentModel;
using SlModel.Annotations;

namespace UserMS.DemoReport
{
    public class SellChartModel
    {
        public int Day { get; set; }
        public decimal M1 { get; set; }
        public decimal M2 { get; set; }
        public decimal? p { get
        {
            if (M1 == 0) return null; return (M2 - M1) / M1; } }

    }
}