using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;

namespace UserMS.Report.Print
{
    /// <summary>
    /// PrintOutOrder2.xaml 的交互逻辑
    /// </summary>
    public partial class PrintOutOrder2 : Page
    {
        public PrintOutOrder2()
        {
            InitializeComponent();
        }

        List<API.Report_OutOrderListInfoWithIMEI> Printlist;

        /// <summary>
        /// 必要字段
        /// 
        /// </summary>
        /// <param name="Printlist"></param>
        public PrintOutOrder2(List<API.Report_OutOrderListInfoWithIMEI> Printlist)
        {
            InitializeComponent();
            

            this.Printlist = Printlist;
        }

        public object SrcPage { set; get; } 
        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= BasePage_Loaded;
            //this.datasource.LoadedData += datasource_LoadedData;
            try
            {

                 
                #region 报表


                ReportDataSource reportDataSource = new ReportDataSource();

               // reportDataSource.Name = "DS_CurrentRepairOrderInfo"; // Name of the DataSet we set in .rdlc

                reportDataSource.Name = "OutOrderDataSet"; 
                
                _reportViewer.LocalReport.DisplayName = "调拨单";
                _reportViewer.LocalReport.ReportPath = "Report\\Print\\Out.rdlc"; // Path of the rdlc file
                //List<ReportParameter> rptPara = new List<ReportParameter>();
                //rptPara.Add(new ReportParameter("rptName", "维修受理单2"));
                System.Reflection.FieldInfo info;
                foreach (RenderingExtension extension in _reportViewer.LocalReport.ListRenderingExtensions())
                {
                    if (extension.Name.ToLower() == "pdf" || extension.Name.ToLower().IndexOf("word") >= 0)
                    {
                        info = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                        info.SetValue(extension, false);

                    }
                }

                
                reportDataSource.Value = this.Printlist;
                _reportViewer.LocalReport.DataSources.Add(reportDataSource);

                 
                _reportViewer.RefreshReport();
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + "");
            }
        }

        private void Prev_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new Uri( SrcPage.ToString(), UriKind.Relative));
        }
  

    }
}
