using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UserMS.Sys_tem.Config
{
    public partial class SysConfig : Page
    {
        private string menuid = "";
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
                //menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "121";
            }
            DockPanel dp = (DockPanel)this.FindName("panel");

            for (int i = 0; i < Store.Options.Count; i++)
            {
                WrapPanel wp = AddControl(i);
                wp.SetValue(DockPanel.DockProperty, Dock.Top);
                dp.Children.Add(wp);
            }

            WrapPanel wpb = new WrapPanel();
            wpb.Margin = new Thickness(20, 35, 0, 0);
            wpb.Orientation = System.Windows.Controls.Orientation.Horizontal;

            StackPanel sp = new StackPanel();
            sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
            sp.Height = 30;

            Button btn = new Button();
            btn.Width = 100;
            btn.Content = "保存";
            btn.Height = 28;
            btn.Click += Button_Click;
            sp.Children.Add(btn);
            wpb.Children.Add(sp);
            dp.Children.Add(wpb);

            int index = 0;
            foreach (var item in Store.Options)
            {
                TextBox text = (TextBox)this.FindName("name_" + index);
                text.Text = Convert.ToString(item.Name);
                text.IsReadOnly = true;
                text.Tag = Convert.ToString(item.ID);
                TextBox value = (TextBox)this.FindName("value_" + index);
                value.Text = Convert.ToString(item.Value);
                TextBox value2 = (TextBox)this.FindName("value1_" + index);
                value2.Text = Convert.ToString(item.Value2);
                //  TextBox cn = (TextBox)this.FindName("classname_" + index);
                // cn.Text = Convert.ToString(item.ClassName);
                TextBox note = (TextBox)this.FindName("note_" + index);
                note.Text = Convert.ToString(item.Note);

                if (item.Name == "仓管参数")
                {
                    text.IsReadOnly = true;
                    note.IsReadOnly = true;
                    // cn.IsReadOnly = true;
                }
                if (item.Name == "广信延保")
                {
                    text.IsReadOnly = true;
                    value2.IsReadOnly = true;
                    note.IsReadOnly = true;
                    // cn.IsReadOnly = true;
                }

                index++;
            }
        }

        private WrapPanel AddControl(int index)
        {
            WrapPanel wp = new WrapPanel();
            wp.Orientation = System.Windows.Controls.Orientation.Horizontal;
            wp.Margin = new Thickness(20, 10, 0, 0);
            wp.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            wp.Children.Add(AddChildren("名称：", "name_", index));
            wp.Children.Add(AddChildren("参数1：", "value_", index));
            wp.Children.Add(AddChildren("参数2：", "value1_", index));
            // wp.Children.Add(AddChildren("类名：", "classname_", index));
            wp.Children.Add(AddChildren("备注：", "note_", index));
            return wp;
        }

        private StackPanel AddChildren(string name, string title, int index)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
            sp.Margin = new Thickness(20, 0, 0, 0);
            sp.Height = 25;
            TextBlock tb = new TextBlock();
            tb.Text = name;

            TextBox t = new TextBox();
            t.Name = title + index;
            this.RegisterName(t.Name, t);

            t.Height = 25;
            t.Width = 100;
            sp.Children.Add(tb);
            sp.Children.Add(t);
            return sp;
        }

        public SysConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<API.Sys_Option> list = new List<API.Sys_Option>();

            for (int index = 0; index < Store.Options.Count; index++)
            {
                API.Sys_Option so = new API.Sys_Option();

                TextBox text = (TextBox)this.FindName("name_" + index);
                if (!Validate(text.Text, index))
                {
                    return;
                }
                so.Name = text.Text;
                so.ID = Convert.ToInt32(text.Tag.ToString());

                TextBox value = (TextBox)this.FindName("value_" + index);
                so.Value = value.Text;
                TextBox value2 = (TextBox)this.FindName("value1_" + index);
                so.Value2 = value2.Text;

                //TextBox cn = (TextBox)this.FindName("classname_" + index);
                //if (!Validate(cn.Text, index))
                //{
                //    return;
                //}
                //so.ClassName = cn.Text;
                TextBox note = (TextBox)this.FindName("note_" + index);
                so.Note = note.Text;
                list.Add(so);
            }


            PublicRequestHelp lrh = new PublicRequestHelp(this.busy, 172, new object[] { list }, new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                PublicRequestHelp lrh = new PublicRequestHelp(this.busy, 171, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败");
            }
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                Store.Options = e.Result.Obj as List<API.Sys_Option>;
            }
        }

        private bool Validate(string txt, int index)
        {
            if (string.IsNullOrEmpty(txt))
            {
                TextBlock title = (TextBlock)this.FindName("tb_name" + index % 5);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,title.Text + "不能为空");
                return false;
            }
            return true;
        }

    }
}
