using System;
using System.Collections.Generic;
using System.Linq;

namespace UserMS.Common
{
    public class HallAdder
    {
        private  MultSelecter msFrm = null;
        private MyAutoTextBox textbox=null;
        private List<TreeViewModel> tvmodels;
        private bool hasInit = false;

        public HallAdder(ref MyAutoTextBox tb)
        {
            textbox = tb;
             tvmodels = new List<TreeViewModel>();
        }

        public void Add()
        {
            if (!hasInit) 
            {
                if (Store.ProHallInfo != null)
                {
                    TreeViewModel tvm = null;
                    foreach (var hi in Store.ProHallInfo)
                    {
                        tvm = new TreeViewModel();
                        tvm.ID = hi.HallID;
                        tvm.Title = hi.HallName;
                        tvmodels.Add(tvm);
                    }
                }

                msFrm = new MultSelecter(
                  tvmodels,
                  Store.ProHallInfo, "HallID", "HallName",
                 new string[] { "HallID", "HallName" },
                 new string[] { "仓库编码", "仓库名称" }
                );
                hasInit = true;
                msFrm.Closed += HallSelect_Closed;
            }
            msFrm.ShowDialog();
        }

        /// <summary>
        /// 确定添加营业厅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HallSelect_Closed(object sender, EventArgs e)
        {
            UserMS.MultSelecter ms = sender as UserMS.MultSelecter;
            if (ms.DialogResult==true)
            {
                List<UserMS.API.Pro_HallInfo> phList = ms.SelectedItems.OfType<UserMS.API.Pro_HallInfo>().ToList();
                if (phList.Count == 0) return;
                textbox.Tag = phList[0].HallID;
                textbox.TextBox.SearchText = phList[0].HallName;
            }
        }

    }
}
