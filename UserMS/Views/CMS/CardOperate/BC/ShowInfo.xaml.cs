using System;
using System.Collections.Generic;
using System.Windows;

namespace UserMS.Views.CMS.CardOperate.BC
{
    public partial class ShowInfo
    {
        public string IMEI { get; set; }
      // public string NEWIMEI { get; set; }
      // public int VIPID { get; set; }
        public SlModel.VIPModel vipinfo { get; set; }
        List<SlModel.VIPModel> vip = null;
        public ShowInfo()
        {
            vip = new List<SlModel.VIPModel>();
            DGVIPInfo = new Telerik.Windows.Controls.RadGridView();
            DGVIPInfo.ItemsSource = vip;
            InitializeComponent();
        }
        public void showchild(SlModel.VIPModel vipmodel, string IMEI1,  string TypeName1,string Cost_production1,  string Validity1)
        {
            //VIPID = vipmodel.ID;
            IMEI = vipmodel.IMEI;
            //NEWIMEI = IMEI1;
            vipmodel.IMEI = IMEI1;           
            vipmodel.TypeName = TypeName1;
            vipmodel.Cost_production =Convert.ToDecimal(Cost_production1);
            vipmodel.Validity =int.Parse( Validity1);

            vipinfo = vipmodel;
            vip.Add(vipmodel);
            this.DGVIPInfo.ItemsSource = vip;
            this.DGVIPInfo.Rebind();
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

