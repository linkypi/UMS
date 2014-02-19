using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using UserMS.ReportService;
using System.Collections.Generic;

namespace UserMS.Report.Print.RepairPrint
{
    
    public partial class PrintRepairBill : BasePage
    {
        public object SrcPage { set; get; } 
        List<API.View_ASPReceiveInfo> Printlist;

        public PrintRepairBill()
        {
            InitializeComponent();


            this.Printlist = new List<API.View_ASPReceiveInfo>() { 
            new API.View_ASPReceiveInfo(){
                HallName="银通银通银通银通银通银通银通银通银通银通银通银通",
                ServiceID="123456789123456789",
                Cus_CPC="银通银通银通银通银通银通银通",
                Cus_Phone="1335555666688889999789789789789",
                Cus_Phone2="18977778888777788889999666",
                SysDate=DateTime.Now,
                Cus_Name="银通银通银通银通银通银通银通银通银通",
                Pro_Name="银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通",
                Pro_Error="银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通",
                Pro_SN="ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                Pro_IMEI="ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                Pro_Note="银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通",
                Receiver="银通银通银通银通银通银通银通银通银通银通银通银通银通银通银通"
            },
            };
        }

        /// <summary>
        /// 必要字段
        /// HallName，ServiceID，Cus_CPC，Cus_phone，Cus_phone，SysDate，Cus_Name,Pro_Name，Pro_Error,Pro_SN,Pro_IMEI,Pro_Note,Receiver.
        /// </summary>
        /// <param name="Printlist"></param>
        public PrintRepairBill(List<API.View_ASPReceiveInfo> Printlist)
        {
            InitializeComponent();
            

            this.Printlist = Printlist;
        }
 

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= BasePage_Loaded;
            //this.datasource.LoadedData += datasource_LoadedData;
            try
            {

                 
                #region 报表


                ReportDataSource reportDataSource = new ReportDataSource();

                reportDataSource.Name = "DS_CurrentRepairOrderInfo"; // Name of the DataSet we set in .rdlc

                _reportViewer.LocalReport.DisplayName = "维修受理单";
                _reportViewer.LocalReport.ReportPath = "Report\\Print\\RepairPrint\\RepairBill.rdlc"; // Path of the rdlc file
                List<ReportParameter> rptPara = new List<ReportParameter>();
                rptPara.Add(new ReportParameter("rptName", "维修受理单2"));
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
