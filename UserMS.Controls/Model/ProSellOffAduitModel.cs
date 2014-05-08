using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UserMS.Model
{
    public class View_SellOffAduitInfo : INotifyPropertyChanged
    {
        private string aduitNote;

        public string AduitNote
        {
            get { return aduitNote; }
            set { aduitNote = value; }
        }
        private string aduitDate;

        public string AduitDate
        {
            get { return aduitDate; }
            set { aduitDate = value; }
        }
        private string applyUser;

        public string ApplyUser
        {
            get { return applyUser; }
            set { applyUser = value; }
        }

        private string applyDate;

        public string ApplyDate
        {
            get { return applyDate; }
            set { applyDate = value; }
        }

        private string applyNote;

        public string ApplyNote
        {
            get { return applyNote; }
            set { applyNote = value; }
        }
        private int? backID;

        public int? BackID
        {
            get { return backID; }
            set { backID = value; }
        }
        private int? sellID;

        public int? SellID
        {
            get { return sellID; }
            set { sellID = value; }
        }
        private string hallID;

        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
        }
        private string hallName;

        public string HallName
        {
            get { return hallName; }
            set { hallName = value; }
        }
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        private string isAduited;

        public string IsAduited
        {
            get { return isAduited; }
            set { isAduited = value; }
        }
        private string isPass;

        public string IsPass
        {
            get { return isPass; }
            set { isPass = value; }
        }
        private decimal nextPrice;

        public decimal NextPrice
        {
            get { return nextPrice; }
            set { nextPrice = value; }
        }
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
