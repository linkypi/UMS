using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls.DataServices;
using Telerik.Windows.Controls.Map;
using UserMS.Model.MapReport;
using UserMS.ReportService;


namespace UserMS.DemoReport
{
    public partial class SellChart2 : Page
    {
        private FrameworkElement clickedElement;

        public UserMS.Model.SellChart.SellChartModel2 model = new Model.SellChart.SellChartModel2();
       
        public SellChart2()
        {
            InitializeComponent();
            this.AreaInformation.ItemsSource = Store.AreaInfo;
            this.captionLayer.ItemsSource = Store.AreaInfo;

            this.year1.NumberFormatInfo = new NumberFormatInfo() { NumberGroupSeparator = "" };
            this.year2.NumberFormatInfo = new NumberFormatInfo() { NumberGroupSeparator = "" };
            //this.Series1.


            //            BindingOperations.SetBinding(Series1, DataSeries.,
            //                                         new Binding() { Path = new PropertyPath("Label1"), Source = model, Mode = BindingMode.TwoWay });
            //            BindingOperations.SetBinding(Series2, SeriesMapping.LegendLabelProperty,
            //                                  new Binding() { Path = new PropertyPath("Label2"), Source = model, Mode = BindingMode.TwoWay });

            //            ds = new List<SellChartModel>();
            //            Random r=new Random();
            //            for (int i = 1; i < 10; i++)
            //            {
            //                int t = r.Next(10000);
            //                ds.Add(new SellChartModel(){Day=i,M1=t,M2=t+r.Next(-500,500)});
            //            }
            //            this.CHART.ItemsSource = ds;
            Entities e = new ReportServiceContext();

            // this.datasource.LoadingData += datasource_LoadingData;
            this.datasource.LoadedData += datasource_LoadedData;
            this.datasource.BeginInit();
            this.datasource.AutoLoad = true;
            this.datasource.QueryName = "Chart_MapReport";
            this.datasource.Load();
            this.datasource.EndInit();
            this.datasource.DataServiceContext = e;

            //            this.datasource.BeginInit();
            //            ReportContext a = new ReportContext();
            //            this.datasource.QueryName = "GetMapReport";
            //            this.datasource.AutoLoad = true;
            //            this.datasource.DomainContext = a;
            //            this.datasource.Load();
            //            this.datasource.EndInit();
            //            TODO: Calc

        }

        private void datasource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.busy.IsBusy = false;
            this.model.ds = e.Entities.Cast<Chart_MapReport>().ToList();
            this.DataContext = model;
            this.year1.Value = model.Selected_Year1;
            this.year2.Value = model.Selected_Year2;
            this.num1.Value = model.StartDay;
            this.num2.Value = model.EndDay;
            ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[0].LegendLabel = model.Label1;
            ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[1].LegendLabel = model.Label2;
        }



        private FrameworkElement GetSenderDataPoint()
        {
            if (this.clickedElement != null)
            {
                FrameworkElement element = this.clickedElement;
                this.clickedElement = null;

                return element;
            }
            return null;
        }
        private API.Pro_AreaInfo GetDataPointArea(FrameworkElement senderDataPoint)
        {
            if (senderDataPoint.DataContext is API.Pro_HallInfo)
            {
                return Store.AreaInfo.First(p => p.AreaID == ((API.Pro_HallInfo)senderDataPoint.DataContext).AreaID);
            }
            else
            {
                return (API.Pro_AreaInfo)senderDataPoint.DataContext;
            }
        }

        private FrameworkElement selectedarea;
        private void RadMap_OnMapMouseClick(object sender, MapMouseRoutedEventArgs eventargs)
        {
            FrameworkElement senderDataPoint = GetSenderDataPoint();
            if (senderDataPoint != null)
            {

                if (!senderDataPoint.Equals(selectedarea))
                {
                    this.SelectArea(senderDataPoint);


                }
                else
                {
                    this.ClearSelect();
                }
                //                
            }
            else
            {
                this.ClearSelect();
            }
        }

        private void SelectArea(FrameworkElement ele)
        {
            if (ele is Telerik.Windows.Controls.Map.MapPolygon)
            {
                var e = ele as Telerik.Windows.Controls.Map.MapPolygon;
                e.Fill = new SolidColorBrush(new Color() { A = 128, R = 31, G = 163, B = 235 });
                e.Stroke = new SolidColorBrush(new Color() { A = 255, R = 0, G = 0, B = 0 });
                e.StrokeThickness = 3;
                if (selectedarea != null)
                {
                    ((MapPolygon)selectedarea).Fill =
                      GetDataPointArea(selectedarea).AreaColor;
                    ((MapPolygon)selectedarea).Stroke = new SolidColorBrush(new Color() { A = 255, R = 255, G = 255, B = 255 });
                    ((MapPolygon)selectedarea).StrokeThickness = 1;
                }
                selectedarea = e;
            }
            else if (ele is TextBlock)
            {

            }

            model.Selected_AreaID = GetDataPointArea(selectedarea).AreaID;

            new Thread(() =>
            {

                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = true;
                });

                model.calcnewresult();
                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = false;
                    //                    this.Series1.LegendLabel = model.Label1;
                    //                    this.Series2.LegendLabel = model.Label2;
                    ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[0].LegendLabel = model.Label1;
                    ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[1].LegendLabel = model.Label2;
                });


            }).Start();


            //this.Grid.GroupDescriptors.Clear();

        }


        private void ClearSelect()
        {
            if (selectedarea != null)
            {
                ((MapPolygon)selectedarea).Fill = GetDataPointArea(selectedarea).AreaColor;
                ((MapPolygon)selectedarea).Stroke = new SolidColorBrush(new Color() { A = 255, R = 255, G = 255, B = 255 });
                ((MapPolygon)selectedarea).StrokeThickness = 1;
                selectedarea = null;
            }
            model.Selected_AreaID = 0;

            new Thread(() =>
            {

                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = true;
                });

                model.calcnewresult();
                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = false;
                    //                    this.Series1.LegendLabel = model.Label1;
                    //                    this.Series2.LegendLabel = model.Label2;
                    ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[0].LegendLabel = model.Label1;
                    ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[1].LegendLabel = model.Label2;
                });


            }).Start();
            //            this.Grid.GroupDescriptors.Clear();
            //            GroupDescriptor descriptor = new GroupDescriptor();
            //            descriptor.Member = "AreaName";
            //            descriptor.DisplayContent = "区域";
            //            descriptor.SortDirection = ListSortDirection.Descending;
            //            descriptor.AggregateFunctions.Add(new SumFunction(){ SourceField="SellPrice",ResultFormatString="{0:#,#.00}",Caption="销售总额: "});
            //            this.Grid.GroupDescriptors.Add(descriptor);
        }

        private void elementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                this.clickedElement = element;
            }

        }

        private void GEN_Chart_OnClick(object sender, RoutedEventArgs e)
        {

            this.model.Selected_Year1 = Convert.ToInt32(year1.Value);
            this.model.Selected_Year2 = Convert.ToInt32(year2.Value);
            this.model.StartDay = Convert.ToInt32(num1.Value);
            this.model.EndDay = Convert.ToInt32(num2.Value);
            new Thread(() =>
            {

                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = true;
                });

                model.calcnewresult();
                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = false;
                    ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[0].LegendLabel = model.Label1;
                    ((DataSeriesCollection)CHART.DefaultView.ChartArea.DataSeries)[1].LegendLabel = model.Label2;
                    //                    this.Series1.LegendLabel = model.Label1;
                    //                    this.Series2.LegendLabel = model.Label2;
                });


            }).Start();


            //TODO:rebind data
            //            ds = new List<SellChartModel>();
            //            Random r = new Random();
            //            int f = Convert.ToInt32(From.Text);
            //            int t = Convert.ToInt32(To.Text);
            //            for (int i = f; i <= t; i++)
            //            {
            //                int d = r.Next(10000);
            //                ds.Add(new SellChartModel() { Day = i, M1 = d, M2 = d + r.Next(-500, 500) });
            //            }
            //            this.CHART.ItemsSource = ds;
        }



        private void FMonth_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs selectionChangedEventArgs)
        {
            RadComboBox combox = sender as RadComboBox;
            if (combox == null) return;

            this.model.Selected_Month1 = Convert.ToInt32(combox.Text.Replace("月", "").Trim());

        }

        private void TMonth_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs selectionChangedEventArgs)
        {
            RadComboBox combox = sender as RadComboBox;
            if (combox == null) return;

            this.model.Selected_Month2 = Convert.ToInt32(combox.Text.Replace("月", "").Trim());

        }

        private void CHART_DataBound(object sender, Telerik.Windows.Controls.Charting.ChartDataBoundEventArgs e)
        {


        }
    }
}
