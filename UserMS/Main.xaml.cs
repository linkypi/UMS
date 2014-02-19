
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Filtering.Editors;
using Telerik.Windows.Controls.GridView;
using UserMS.MyControl;

namespace UserMS
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Page
    {
        List<API.GetUserAllRemindListResult> models = new List<API.GetUserAllRemindListResult>();

        public Main()
        {
            InitializeComponent();
            //PublicRequestHelp.UpdateInitData(this.busy);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //FancyBalloon balloon = new FancyBalloon();
            //balloon.BalloonText = "Custom Balloon";

            //MyNotifyIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 4000);


            //MainWindow w = new MainWindow();
            //w.Show();
        }


        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            GridView.ItemsSource = models;
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 272, new object[] { "1900-1-1","" }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            models.Clear();
            if (e.Result.ReturnValue)
            {
                List<API.GetUserAllRemindListResult> list = e.Result.Obj as List<API.GetUserAllRemindListResult>;
                if (list == null)
                {
                    return;
                }
                models.AddRange(list);
                GridView.Rebind();
            }
        }

        private void GridView_OnFilterOperatorsLoading(object sender, FilterOperatorsLoadingEventArgs e)
        {
            
        }

        private void GridView_OnFieldFilterEditorCreated(object sender, EditorCreatedEventArgs e)
        {
            var edit = e.Editor as StringFilterEditor;
            if (edit == null) return;
            edit.MatchCaseVisibility=Visibility.Collapsed;
            edit.IsCaseSensitive = false;
            //edit.IsInputMethodEnabled = true;
            //System.Windows.Input.InputMethod.SetIsInputMethodEnabled(edit,true);
        }
    }
}
