using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;

namespace UserMS.Report.InOrder
{
    public partial class RepairVisit : BasePage
    {
        public int MethodID = 64;
        public int ExportMethodID = 65;

        #region list
        public List<API.View_Pro_InOrder> list = new List<API.View_Pro_InOrder>() { 
             new API.View_Pro_InOrder(){ID=	1	,InOrderID="	黄东强	", Pro_HallID="	18938753189	", HallName="	2010-06-03 11:33:48.000	",OldID="	贴膜	",UserID="	蔡社添	",  Note="	还可以	"},
 new API.View_Pro_InOrder(){ID=	2	,InOrderID="	苏文波	", Pro_HallID="	13346483581	", HallName="	2010-06-03 17:30:48.000	",OldID="	贴膜	",UserID="	陈炳培	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	3	,InOrderID="	肖胜华	", Pro_HallID="	18938767666	", HallName="	2010-06-03 18:39:27.000	",OldID="	贴膜	",UserID="	魏松	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	4	,InOrderID="	梁银好	", Pro_HallID="	13326985788	", HallName="	2010-06-03 18:30:29.000	",OldID="	贴膜	",UserID="	莫楚茵	",  Note="	还可以	"},
 new API.View_Pro_InOrder(){ID=	5	,InOrderID="	吴焕洪	", Pro_HallID="	13326930630	", HallName="	2010-06-03 18:32:27.000	",OldID="	贴膜	",UserID="	卢添成	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	6	,InOrderID="	黄生	", Pro_HallID="	13326987832	", HallName="	2010-06-03 18:34:12.000	",OldID="	刷机	",UserID="	谭显春	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	7	,InOrderID="	黄生	", Pro_HallID="	13326961223	", HallName="	2010-06-03 18:43:07.000	",OldID="	刷机	",UserID="	成日鲜	",  Note="	还可以	"},
 new API.View_Pro_InOrder(){ID=	8	,InOrderID="	黎兆龙	", Pro_HallID="	13392901197	", HallName="	2010-06-04 17:12:59.000	",OldID="	刷机	",UserID="	合达鞋厂	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	9	,InOrderID="	付朋	", Pro_HallID="	18938757160	", HallName="	2010-06-04 17:13:57.000	",OldID="	维修	",UserID="	远丰投资公司	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	10	,InOrderID="	张小姐	", Pro_HallID="	18933321199	", HallName="	2010-06-04 18:47:08.000	",OldID="	维修	",UserID="	亿华泉饮料厂	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	11	,InOrderID="	罗生	", Pro_HallID="	18933329661	", HallName="	2010-06-04 18:48:10.000	",OldID="	维修	",UserID="	远丰投资公司	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	12	,InOrderID="	王平	", Pro_HallID="	18933329260	", HallName="	2010-06-04 18:48:44.000	",OldID="	维修	",UserID="	张雪峰	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	13	,InOrderID="	成日鲜	", Pro_HallID="	18933329310	", HallName="	2010-06-04 18:49:25.000	",OldID="	维修	",UserID="	梁彬	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	14	,InOrderID="	合达鞋厂	", Pro_HallID="	18933329320	", HallName="	2010-06-04 18:50:06.000	",OldID="	维修	",UserID="	冯天海	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	15	,InOrderID="	远丰投资公司	", Pro_HallID="	18933329931	", HallName="	2010-06-04 18:50:51.000	",OldID="	贴膜	",UserID="	梁寿棉	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	16	,InOrderID="	亿华泉饮料厂	", Pro_HallID="	18933329215	", HallName="	2010-06-04 18:51:28.000	",OldID="	贴膜	",UserID="	大涌镇计划生育办公室	",  Note="	满意，速度快	"},
 new API.View_Pro_InOrder(){ID=	17	,InOrderID="	远丰投资公司	", Pro_HallID="	18933329125	", HallName="	2010-06-04 18:52:21.000	",OldID="	贴膜	",UserID="	中山汇达制衣有限公司	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	18	,InOrderID="	张雪峰	", Pro_HallID="	18924998436	", HallName="	2010-06-04 18:53:14.000	",OldID="	贴膜	",UserID="	先生	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	19	,InOrderID="	梁彬	", Pro_HallID="	18925320795	", HallName="	2010-06-04 18:54:08.000	",OldID="	下载软件	",UserID="	先生	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	20	,InOrderID="	李锦添	", Pro_HallID="	18925320537	", HallName="	2010-06-04 18:54:46.000	",OldID="	下载软件	",UserID="	先生	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	21	,InOrderID="	莫楚茵	", Pro_HallID="	18938790522	", HallName="	2010-06-04 18:55:47.000	",OldID="	下载软件	",UserID="	李少奇	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	22	,InOrderID="	卢添成	", Pro_HallID="	18938790523	", HallName="	2010-06-04 18:56:23.000	",OldID="	下载软件	",UserID="	远丰投资公司	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	23	,InOrderID="	陈健	", Pro_HallID="	18933329910	", HallName="	2010-06-04 18:57:08.000	",OldID="	下载软件	",UserID="	庞雪兰	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	24	,InOrderID="	钟振汉	", Pro_HallID="	18924998446	", HallName="	2010-06-04 18:57:54.000	",OldID="	下载软件	",UserID="	富山清泉公司	",  Note="	非常满意，服务很到位	"},
 new API.View_Pro_InOrder(){ID=	25	,InOrderID="	李锡洋	", Pro_HallID="	13392922338	", HallName="	2010-06-04 18:58:36.000	",OldID="	下载软件	",UserID="	陈算祖	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	26	,InOrderID="	陈健	", Pro_HallID="	13318282681	", HallName="	2010-06-04 18:59:20.000	",OldID="	下载软件	",UserID="	桂山殖峰林园	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	27	,InOrderID="	吴海雄	", Pro_HallID="	18938743996	", HallName="	2010-06-04 19:00:17.000	",OldID="	下载软件	",UserID="	廖小红	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	28	,InOrderID="	许俊	", Pro_HallID="	18925329927	", HallName="	2010-06-04 19:01:25.000	",OldID="	清理	",UserID="	涂慧琴	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	29	,InOrderID="	王兴伟	", Pro_HallID="	18924987775	", HallName="	2010-06-04 19:02:14.000	",OldID="	清理	",UserID="	余业芳	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	30	,InOrderID="	先生	", Pro_HallID="	18933329217	", HallName="	2010-06-04 19:03:05.000	",OldID="	清理	",UserID="	蒙玉党	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	31	,InOrderID="	先生	", Pro_HallID="	18924987770	", HallName="	2010-06-04 19:03:53.000	",OldID="	清理	",UserID="	鑫达塑料玩具厂	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	32	,InOrderID="	祁丽贵	", Pro_HallID="	18924994918	", HallName="	2010-06-04 19:04:53.000	",OldID="	清理	",UserID="	李刘彬	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	33	,InOrderID="	李昌伟	", Pro_HallID="	18925320258	", HallName="	2010-06-04 19:05:38.000	",OldID="	清理	",UserID="	先生	",  Note="	服务态度倒是不错 就是慢了点	"},
 new API.View_Pro_InOrder(){ID=	34	,InOrderID="	李锦良	", Pro_HallID="	18928133298	", HallName="	2010-06-04 19:06:25.000	",OldID="	清理	",UserID="	先生	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	35	,InOrderID="	徐作桂	", Pro_HallID="	18933329235	", HallName="	2010-06-04 19:07:16.000	",OldID="	清理	",UserID="	先生	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	36	,InOrderID="	李瑟	", Pro_HallID="	18925320270	", HallName="	2010-06-04 19:07:59.000	",OldID="	清理	",UserID="	先生	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	37	,InOrderID="	陶心华	", Pro_HallID="	18933329562	", HallName="	2010-06-04 19:09:02.000	",OldID="	清理	",UserID="	奴多姿纺织厂有限公司	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	38	,InOrderID="	蔡社添	", Pro_HallID="	18933329236	", HallName="	2010-06-04 19:09:55.000	",OldID="	清理	",UserID="	庞雪兰	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	39	,InOrderID="	陈炳培	", Pro_HallID="	18925320408	", HallName="	2010-06-04 19:10:49.000	",OldID="	清理	",UserID="	方新良	",  Note="	不错	"},
 new API.View_Pro_InOrder(){ID=	40	,InOrderID="	魏松	", Pro_HallID="	18925320498	", HallName="	2010-06-04 19:11:44.000	",OldID="	清理	",UserID="	梁玉根	",  Note="	不错	"},

        };
        #endregion


        public RepairVisit()
        {
            InitializeComponent();
            InitGrid2();
        }

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
            col22.Header = "服务日期";
            this.dataGrid1.Columns.Add(col22);


            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("OldID");
            col3.Header = "服务内容";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("UserID");
            col4.Header = "维修员";
            this.dataGrid1.Columns.Add(col4);
             

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col41.Header = "回访结果";
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
