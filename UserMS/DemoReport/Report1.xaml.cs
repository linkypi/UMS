using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Telerik.Pivot.Core;
using Telerik.Windows.Controls.DataServices;
using UserMS.ReportService;

namespace UserMS.DemoReport
{
    public partial class Report1 : Page
    {
        public Report1()
        {
            InitializeComponent();
//            DemoHallInfo d=new DemoHallInfo();
//            

          
            Entities e = new ReportServiceContext();

            // this.datasource.LoadingData += datasource_LoadingData;
            this.datasource.LoadedData += datasource_LoadedData;
            this.datasource.BeginInit();
            this.datasource.AutoLoad = true;
            this.datasource.QueryName = "Report_EveryHallStoreInfo";
            this.datasource.Load();
            this.datasource.EndInit();
            this.datasource.DataServiceContext = e;
            
        }

        private void datasource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            var localDataProvider = this.Resources["datas"] as LocalDataSourceProvider;

           
            localDataProvider.ItemsSource = e.Entities.Cast<Report_EveryHallStoreInfo>().ToList();
            
        }
    }
}
