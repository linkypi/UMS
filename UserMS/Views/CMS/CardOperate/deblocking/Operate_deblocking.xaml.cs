﻿using System.Windows;

namespace UserMS.CMS
{
    public partial class Operate_deblocking
    {
        public Operate_deblocking()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

