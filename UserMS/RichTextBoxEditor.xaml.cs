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
using System.Windows.Shapes;

namespace UserMS
{
    /// <summary>
    /// RichTextBoxEditor.xaml 的交互逻辑
    /// </summary>
    public partial class RichTextBoxEditor : Window
    {
        public RichTextBoxEditor()
        {
            InitializeComponent();
        }

        public RichTextBoxEditor(string text)
        {
            InitializeComponent();
            this.Introduction.Text = text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
