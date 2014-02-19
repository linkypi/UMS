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

namespace UserMS.Views.ProSell.Salary
{
    /// <summary>
    /// CurrentSalary.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentSalary : Page
    {
        public CurrentSalary()
        {
            InitializeComponent();

            PublicRequestHelp p = new PublicRequestHelp(this.busy,299,new object[]{},new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                GridSalaryList.ItemsSource = null;
                GridSalaryList.Rebind();

                List<API.View_CurrentSalary> list = e.Result.Obj as    List<API.View_CurrentSalary> ;
                if(list==null)
                {
                    return;
                }
                GridSalaryList.ItemsSource = list;
                GridSalaryList.Rebind();
            }
        }
    }
}
