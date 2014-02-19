using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UserMS.Views.RoleMS
{
    public partial class RoleManager : Page
    {
        public RoleManager()
        {
            InitializeComponent();

           PublicRequestHelp prh = new PublicRequestHelp(null,63,new  object[]{},new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

            int count = Store.ProHallInfo.Count;
            //int row =(int) Math.Ceiling(count/5.0);

            // GridLength hgl = new GridLength(25);
            // RowDefinition rd = null;

            // for (int i = 0; i < row; i++)
            // {
            //     rd = new RowDefinition();
            //     rd.Height = hgl;
            //     GridHall.RowDefinitions.Add(rd);
            // }

            //ColumnDefinition cd = null;
          
            //GridLength wgl = new GridLength(85);

            //for (int j = 0; j < 5; j++)
            //{
            //    cd = new ColumnDefinition();
            //    cd.Width = wgl;
            //    GridHall.ColumnDefinitions.Add(cd);
            //}

            int index = 0;
            int row = 0;
            int col = 0;
            CheckBox cb = null;

            foreach (var item in Store.ProHallInfo)
            {
                try
                {
                    cb = new CheckBox();
                    cb.Height = 25;
                    cb.Width = 120;

                    row = index / 5;
                    col = index % 5;//90 * col +
                    Thickness tk2 = new Thickness( 120*col+5, 35 * row + 5, 5, 5);
                    cb.Margin = tk2;
                   // cb.HorizontalAlignment = HorizontalAlignment.Left;
                    cb.Content = item.HallName;
                    cb.Tag = item.HallID;
                    index++;
                    GridHall.Children.Add(cb);
                }
                catch (Exception ex)
                {

                }
            }
                    
          
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            List<API.MenuInfo> list = e.Result.Obj as List<API.MenuInfo>;
            this.treeview.ItemsSource = list;
        }

       
    }
}
