using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Navigation;

using Telerik.Windows;
using Telerik.Windows.Controls.DataServices;
using Telerik.Windows.Controls.Map;
using Telerik.Windows.Data;
using UserMS.Model.MapReport;
using UserMS.ReportService;


namespace UserMS.DemoReport
{
    public partial class MapReport : Page
    {
        private FrameworkElement clickedElement;
        //        protected const string ShapeRelativeUriFormat = "/UserMS;component/MapResources/doc.kml";
        //        protected  Uri MapSources = new Uri(ShapeRelativeUriFormat, UriKind.RelativeOrAbsolute);
        private UserMS.Model.MapReport.MapReportModel m;
        
        public MapReport()
        {
            InitializeComponent();
            this.AreaInformation.ItemsSource = Store.AreaInfo;
            this.captionLayer.ItemsSource = Store.AreaInfo;
            this.busy.IsBusy = true;

//                        new Thread(() =>
//                        {

                            Entities e = new ReportServiceContext();

                            this.datasource.LoadingData += datasource_LoadingData;
                            this.datasource.LoadedData += datasource_LoadedData;
                            this.datasource.BeginInit();
                            this.datasource.AutoLoad = true;
                            this.datasource.QueryName = "Chart_MapReport";
                            this.datasource.Load();
                            this.datasource.EndInit();
                            this.datasource.DataServiceContext = e;
           


//                        }).Start();
//                       List<PerDayModel> a=new List<PerDayModel>();
//                        var startday = new DateTime(2010, 1, 1);
//                        Random r =new Random();
//                        for (int i = 0; i < 2000; i++)
//                        {
//                            PerDayModel b=new PerDayModel(startday,r.Next(1000),r.Next(1000),r.Next(1000));
//                            startday = startday.AddDays(1);
//                            a.Add(b);
//                        }
//                        this.Line1.ItemsSource = a;
                       // this.busy.IsBusy = false;

        }

        private void datasource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.busy.IsBusy = false;
            //            var a = reportContext.Demo_MapReports.ToList();
            m = new MapReportModel(e.Entities.Cast<Chart_MapReport>().ToList());
            this.DataContext = m;
            TimeBar1.SelectionStart = m.PeriodEnd.AddDays(-30);
            TimeBar1.VisiblePeriodStart = m.PeriodEnd.AddDays(-90);
        }

        private void datasource_LoadingData(object sender, LoadingDataEventArgs e)
        {
             
        }

        void lo_Completed(object sender, EventArgs e)
        {
            this.busy.IsBusy = false;
//            var a = reportContext.Demo_MapReports.ToList();
             //m = new MapReportModel(a);
            this.DataContext = m;
            TimeBar1.SelectionStart = m.PeriodEnd.AddDays(-30);
            TimeBar1.VisiblePeriodStart = m.PeriodEnd.AddDays(-90);
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

            ((MapReportModel)this.DataContext).SelectedAreaID = GetDataPointArea(selectedarea).AreaID;
            //this.Grid.GroupDescriptors.Clear();
            calc();
            TitleHallname.Text = GetDataPointArea(selectedarea).AreaName;
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
            ((MapReportModel)this.DataContext).SelectedAreaID = 0;
            calc();
            TitleHallname.Text = "";
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

        private void RadTimeBar_OnSelectionChanged(object sender, RadRoutedEventArgs e)
        {
            calc();
        }

        private void calc()
        {
            new Thread(() =>
            {

                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = true;
                });

                m.calcselldata();
                Dispatcher.BeginInvoke((ThreadStart)delegate()
                {
                    this.busy.IsBusy = false;
                });


            }).Start();
        }
    }
}
