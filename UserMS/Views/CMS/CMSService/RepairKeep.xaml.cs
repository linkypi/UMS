using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;

namespace UserMS.Report.InOrder
{
    public partial class RepairKeep : BasePage
    {
        public int MethodID = 64;
        public int ExportMethodID = 65;

        #region list
        public List<API.View_Pro_InOrder> list = new List<API.View_Pro_InOrder>() { 
            new API.View_Pro_InOrder(){ID=	1	,InOrderID="	王生	", Pro_HallID="	13424547854	", HallName="	20130312	",OldID="	华为	",UserID="	E5658712177871	",  Note="	贴膜	"},
             new API.View_Pro_InOrder(){ID=	1	,InOrderID="	黄东强	", Pro_HallID="	18938753189	", HallName="	2010-06-03 11:33:48.000	",OldID="	中兴S100	",UserID="	  A1000010812AE4	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	2	,InOrderID="	苏文波	", Pro_HallID="	13346483581	", HallName="	2010-06-03 17:30:48.000	",OldID="	酷派S20	",UserID="	 A00000184D904D	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	3	,InOrderID="	肖胜华	", Pro_HallID="	18938767666	", HallName="	2010-06-03 18:39:27.000	",OldID="	酷派D520	",UserID="	 A10000269C4153_ZZ	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	4	,InOrderID="	梁银好	", Pro_HallID="	13326985788	", HallName="	2010-06-03 18:30:29.000	",OldID="	LGKV230	",UserID="	 A10000269C4174_ZZ	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	5	,InOrderID="	吴焕洪	", Pro_HallID="	13326930630	", HallName="	2010-06-03 18:32:27.000	",OldID="	LGKV230	",UserID="	_990002149464040	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	6	,InOrderID="	黄生	", Pro_HallID="	13326987832	", HallName="	2010-06-03 18:34:12.000	",OldID="	LGKV230	",UserID="	_990002149554006	",  Note="	刷机	"},
 new API.View_Pro_InOrder(){ID=	7	,InOrderID="	黄生	", Pro_HallID="	13326961223	", HallName="	2010-06-03 18:43:07.000	",OldID="	中兴S130	",UserID="	_990002149590240	",  Note="	刷机	"},
 new API.View_Pro_InOrder(){ID=	8	,InOrderID="	黎兆龙	", Pro_HallID="	13392901197	", HallName="	2010-06-04 17:12:59.000	",OldID="	中兴S130	",UserID="	_990002149608224	",  Note="	刷机	"},
 new API.View_Pro_InOrder(){ID=	9	,InOrderID="	付朋	", Pro_HallID="	18938757160	", HallName="	2010-06-04 17:13:57.000	",OldID="	中兴S130	",UserID="	_990002149629626	",  Note="	维修	"},
 new API.View_Pro_InOrder(){ID=	10	,InOrderID="	张小姐	", Pro_HallID="	18933321199	", HallName="	2010-06-04 18:47:08.000	",OldID="	摩托罗拉XT800黑	",UserID="	_990002160009484	",  Note="	维修	"},
 new API.View_Pro_InOrder(){ID=	11	,InOrderID="	罗生	", Pro_HallID="	18933329661	", HallName="	2010-06-04 18:48:10.000	",OldID="	中兴S130	",UserID="	_990002160034417	",  Note="	维修	"},
 new API.View_Pro_InOrder(){ID=	12	,InOrderID="	王平	", Pro_HallID="	18933329260	", HallName="	2010-06-04 18:48:44.000	",OldID="	中兴S130	",UserID="	_990002160043871	",  Note="	维修	"},
 new API.View_Pro_InOrder(){ID=	13	,InOrderID="	成日鲜	", Pro_HallID="	18933329310	", HallName="	2010-06-04 18:49:25.000	",OldID="	中兴S100	",UserID="	_990002160046130	",  Note="	维修	"},
 new API.View_Pro_InOrder(){ID=	14	,InOrderID="	合达鞋厂	", Pro_HallID="	18933329320	", HallName="	2010-06-04 18:50:06.000	",OldID="	中兴S100	",UserID="	_990002160046353	",  Note="	维修	"},
 new API.View_Pro_InOrder(){ID=	15	,InOrderID="	远丰投资公司	", Pro_HallID="	18933329931	", HallName="	2010-06-04 18:50:51.000	",OldID="	LGKX191	",UserID="	_990002160047724	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	16	,InOrderID="	亿华泉饮料厂	", Pro_HallID="	18933329215	", HallName="	2010-06-04 18:51:28.000	",OldID="	LGKV230	",UserID="	_990002160049373	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	17	,InOrderID="	远丰投资公司	", Pro_HallID="	18933329125	", HallName="	2010-06-04 18:52:21.000	",OldID="	中兴S130	",UserID="	_990002160065189	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	18	,InOrderID="	张雪峰	", Pro_HallID="	18924998436	", HallName="	2010-06-04 18:53:14.000	",OldID="	中兴S130	",UserID="	_990002160080816	",  Note="	贴膜	"},
 new API.View_Pro_InOrder(){ID=	19	,InOrderID="	梁彬	", Pro_HallID="	18925320795	", HallName="	2010-06-04 18:54:08.000	",OldID="	中兴S130	",UserID="	_990002160083760	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	20	,InOrderID="	李锦添	", Pro_HallID="	18925320537	", HallName="	2010-06-04 18:54:46.000	",OldID="	中兴S130	",UserID="	_990002160084651	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	21	,InOrderID="	莫楚茵	", Pro_HallID="	18938790522	", HallName="	2010-06-04 18:55:47.000	",OldID="	中兴S130	",UserID="	_990002160087159	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	22	,InOrderID="	卢添成	", Pro_HallID="	18938790523	", HallName="	2010-06-04 18:56:23.000	",OldID="	中兴S130	",UserID="	_990002160110985	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	23	,InOrderID="	陈健	", Pro_HallID="	18933329910	", HallName="	2010-06-04 18:57:08.000	",OldID="	中兴S100	",UserID="	_990002160115745	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	24	,InOrderID="	钟振汉	", Pro_HallID="	18924998446	", HallName="	2010-06-04 18:57:54.000	",OldID="	中兴S100	",UserID="	_990002160117980	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	25	,InOrderID="	李锡洋	", Pro_HallID="	13392922338	", HallName="	2010-06-04 18:58:36.000	",OldID="	三星I329	",UserID="	_990002160127443	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	26	,InOrderID="	陈健	", Pro_HallID="	13318282681	", HallName="	2010-06-04 18:59:20.000	",OldID="	LGKX195	",UserID="	_990002160130058	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	27	,InOrderID="	吴海雄	", Pro_HallID="	18938743996	", HallName="	2010-06-04 19:00:17.000	",OldID="	海尔C600	",UserID="	_990002160135677	",  Note="	下载软件	"},
 new API.View_Pro_InOrder(){ID=	28	,InOrderID="	许俊	", Pro_HallID="	18925329927	", HallName="	2010-06-04 19:01:25.000	",OldID="	华为C5900	",UserID="	_990002160159271	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	29	,InOrderID="	王兴伟	", Pro_HallID="	18924987775	", HallName="	2010-06-04 19:02:14.000	",OldID="	诺基亚1506	",UserID="	_990002160172829	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	30	,InOrderID="	先生	", Pro_HallID="	18933329217	", HallName="	2010-06-04 19:03:05.000	",OldID="	LGKV230	",UserID="	_990002160179717	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	31	,InOrderID="	先生	", Pro_HallID="	18924987770	", HallName="	2010-06-04 19:03:53.000	",OldID="	LGKV230	",UserID="	_990002160183818	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	32	,InOrderID="	祁丽贵	", Pro_HallID="	18924994918	", HallName="	2010-06-04 19:04:53.000	",OldID="	中兴S130	",UserID="	_990002160193072	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	33	,InOrderID="	李昌伟	", Pro_HallID="	18925320258	", HallName="	2010-06-04 19:05:38.000	",OldID="	中兴S130	",UserID="	_A10000235A86F7	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	34	,InOrderID="	李锦良	", Pro_HallID="	18928133298	", HallName="	2010-06-04 19:06:25.000	",OldID="	中兴S130	",UserID="	1	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	35	,InOrderID="	徐作桂	", Pro_HallID="	18933329235	", HallName="	2010-06-04 19:07:16.000	",OldID="	中兴S130	",UserID="	2	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	36	,InOrderID="	李瑟	", Pro_HallID="	18925320270	", HallName="	2010-06-04 19:07:59.000	",OldID="	中兴S130	",UserID="	3	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	37	,InOrderID="	陶心华	", Pro_HallID="	18933329562	", HallName="	2010-06-04 19:09:02.000	",OldID="	中兴S130	",UserID="	4	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	38	,InOrderID="	蔡社添	", Pro_HallID="	18933329236	", HallName="	2010-06-04 19:09:55.000	",OldID="	中兴S130	",UserID="	5	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	39	,InOrderID="	陈炳培	", Pro_HallID="	18925320408	", HallName="	2010-06-04 19:10:49.000	",OldID="	中兴S130	",UserID="	6	",  Note="	清理	"},
 new API.View_Pro_InOrder(){ID=	40	,InOrderID="	魏松	", Pro_HallID="	18925320498	", HallName="	2010-06-04 19:11:44.000	",OldID="	中兴S130	",UserID="	7	",  Note="	清理	"},

        };
        #endregion


        public RepairKeep()
        {
            InitializeComponent();
            InitGrid2();
        }

        // 当用户导航到此页面时执行。
        private void InitGrid2()
        { 
            #region 列
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("Inorderid");
            col.Header = "客户姓名";
            this.dataGrid1.Columns.Add(col);

 
            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("Pro_HallID");
            col2.Header = "客户电话";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col22 = new GridViewDataColumn();
            col22.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col22.Header = "保养时间";
            this.dataGrid1.Columns.Add(col22);


            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("OldID");
            col3.Header = "终端型号";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("UserID");
            col4.Header = "终端串码";
            this.dataGrid1.Columns.Add(col4);
             

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col41.Header = "保养内容";
            this.dataGrid1.Columns.Add(col41);

           

          
           
            #endregion

            #region 取第一页的数据
            //API.ReportPagingParam pageParam = new API.ReportPagingParam() { 
            //    PageIndex=0,
            //    PageSize=this.RadPager.PageSize,
            //    ParamList=new List<API.ReportSqlParams>()
            //};
            //this.InitPageEntity(MethodID,this.dataGrid1 ,this.busy, this.RadPager, pageParam);
	        #endregion
            this.dataGrid1.ItemsSource = list;
            
        }

        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取第一页的数据
            //API.ReportPagingParam pageParam = new API.ReportPagingParam()
            //{
            //    PageIndex = e.NewPageIndex,
            //    PageSize = this.RadPager.PageSize,
            //    ParamList = new List<API.ReportSqlParams>()
            //};
            //this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "97-2003   Excel文件(*.xls)|*.xls";
            if (saveFileDialog.ShowDialog() ?? false)
            {
                System.IO.Stream fs = saveFileDialog.OpenFile();
                SlModel.SheetColumn[] sheetColumns = new SlModel.SheetColumn[]
                {
                    new SlModel.SheetColumn(){Header="入库单号",ObjectProperty="InOrderID"},
                    new SlModel.SheetColumn(){Header="仓库编码",ObjectProperty="Pro_HallID"},
                    new SlModel.SheetColumn(){Header="仓库名称",ObjectProperty="HallName"},
                    new SlModel.SheetColumn(){Header="原始单号",ObjectProperty="OldID"},
                    new SlModel.SheetColumn(){Header="入库日期",ObjectProperty="InDate"},
                    new SlModel.SheetColumn(){Header="录入人",ObjectProperty="UserName"},
                    new SlModel.SheetColumn(){Header="备注",ObjectProperty="Note"}

                };
                string sheetName="入库记录";
                    try
                    {
                        #region 获取导出数据
                        API.ReportPagingParam pageParam = new API.ReportPagingParam()
                        {

                            ParamList = new List<API.ReportSqlParams>()
                        };
                        this.InitPageEntity(ExportMethodID, this.busy, pageParam, fs, sheetColumns, sheetName);
                
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,string.Format("导出失败：", ex.Message));
                    }
                }
            

           
           

        }
         

    }
}
