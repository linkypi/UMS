using System.Collections.Generic;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using System.Linq;
using UserMS.MyControl;
using System.Windows;
using System;
using System.Windows.Navigation;
using System.Windows.Input;

namespace UserMS
{
    public  class Logger
    {
        
        public static TextBlock StatusBar;

        public static void Log(string msg)
        {
          if (Logger.StatusBar != null)
          {
              Logger.StatusBar.Text = msg;
          }
        }

        public static void AddPageToTab(RadTabControl tab,NotifyMessage model)
        {

            int count = tab.Items.Count;

            if (count > 1)
            {

                List<object> items = new List<object>();
                foreach (var item in tab.Items)
                {
                    items.Add(item);
                }

                var query = items.Where(p => ((RadTabItem)p).Name == model.PageTitle);
                if (query.Count() > 0)//已打开
                {
                    ((RadTabItem)query.First()).IsSelected = true;
                    return;
                }
                if (count >= 11)//打开太多
                {
                    Log("您打开的页面的太多了，请先关闭一些");
                    return;
                }

            }

            try
            {

                MyTabItem tbi = new MyTabItem();

                tbi.Name = model.PageTitle;
                MainTabHeader h = new MainTabHeader();

                h.xButton.MouseLeftButtonDown += new MouseButtonEventHandler(model.XButton_MouseLeftButtonDown);
                h.xButton.MouseMove += new MouseEventHandler(model.XButton_MouseMove);
                h.xButton.MouseLeave += new MouseEventHandler(model.XButton_MouseLeave); 
                tbi.tabContextMenu.ItemClick += model.TabCloseMenu_ItemClick;


                h.TextBlockHeader.Text = model.PageTitle;
                h.xButton.Tag = model.PageTitle;


                tbi.Header = h;

                Frame newframe = new Frame();
                newframe.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                newframe.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;
                
                newframe.BorderThickness=new Thickness(1);
                newframe.Source = new Uri(model.SkinName, UriKind.Relative);
                
                tbi.Content = newframe;

                tab.Items.Add(tbi);
                tbi.IsSelected = true;
            }
            catch (Exception ex) {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"页面地址有误");
            }
        }
    }
}