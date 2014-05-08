using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using SlModel.Annotations;
using System;
using UserMS.ReportService;

namespace UserMS.Model.SellChart
{
    public class SellChartModel2 : INotifyPropertyChanged
    {
        private int _selectedYear1;
        private int _selectedYear2;
        private int _selectedMonth1;
        private int _selectedMonth2;
        private int _startDay;
        private int _endDay;
        private int _selectedAreaId;
        public string Label1
        {
            get { return Selected_Year1 + "年" + Selected_Month1 + "月"; }
        }
        public string Label2
        {
            get { return Selected_Year2 + "年" + Selected_Month2 + "月"; }
        }
        private List<Chart_MapReport> _ds;

        public int MaxDay
        {
            get
            {
                return Math.Min(
                    DateTime.DaysInMonth(Selected_Year1, Selected_Month1),
                    DateTime.DaysInMonth(Selected_Year2, Selected_Month2)
                    );
            }
        }

        private List<UserMS.DemoReport.SellChartModel> _result;
        public List<UserMS.DemoReport.SellChartModel> result
        {
            get
            {
                if (ds == null || ds.Count == 0 || Selected_Year1 == 0 || Selected_Year2 == 0 || Selected_Month1 == 0 ||
                    Selected_Month2 == 0 || StartDay == 0 || EndDay == 0)
                {
                    return new List<UserMS.DemoReport.SellChartModel>();
                }
                else
                {

                    return _result;
                }


            }
        }

        public List<Chart_MapReport> ds
        {
            get { return _ds; }
            set
            {
                if (Equals(value, _ds)) return;
                _ds = value;
                OnPropertyChanged("ds");
                calcnewresult();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Selected_Year1
        {
            get { return _selectedYear1; }
            set
            {
                if (value == _selectedYear1) return;
                _selectedYear1 = value;
                OnPropertyChanged("Selected_Year1");
                OnPropertyChanged("MaxDay");
            }
        }

        public int Selected_Year2
        {
            get { return _selectedYear2; }
            set
            {
                if (value == _selectedYear2) return;
                _selectedYear2 = value;
                OnPropertyChanged("Selected_Year2");
                OnPropertyChanged("MaxDay");
            }
        }

        public int Selected_Month1
        {
            get { return _selectedMonth1; }
            set
            {
                if (value == _selectedMonth1) return;
                _selectedMonth1 = value;
                OnPropertyChanged("Selected_Month1");
                OnPropertyChanged("MaxDay");
            }
        }

        public int Selected_Month2
        {
            get { return _selectedMonth2; }
            set
            {
                if (value == _selectedMonth2) return;
                _selectedMonth2 = value;
                OnPropertyChanged("Selected_Month2");
                OnPropertyChanged("MaxDay");
            }
        }

        public int StartDay
        {
            get { return _startDay; }
            set
            {
                if (value == _startDay) return;
                _startDay = value;
                OnPropertyChanged("StartDay");

            }
        }

        public int EndDay
        {
            get { return _endDay; }
            set
            {
                if (value == _endDay) return;
                _endDay = value;
                OnPropertyChanged("EndDay");

            }
        }

        public int Selected_AreaID
        {
            get { return _selectedAreaId; }
            set
            {
                if (value == _selectedAreaId) return;
                _selectedAreaId = value;
                OnPropertyChanged("Selected_AreaID");
            }
        }


        public SellChartModel2()
        {
            _selectedYear1 = 2013;
            _selectedYear2 = 2013;
            _startDay = 1;
            _endDay = 5;
            _selectedMonth1 = 1;
            _selectedMonth2 = 2;

        }


        public void calcnewresult()
        {
            if (StartDay > MaxDay)
            {
                StartDay = MaxDay;
            }
            if (EndDay > MaxDay)
            {
                EndDay = MaxDay;
            }

            DateTime startdate1 = new DateTime(Selected_Year1, Selected_Month1, StartDay);
            DateTime enddate1 = new DateTime(Selected_Year1, Selected_Month1, EndDay);
            DateTime startdate2 = new DateTime(Selected_Year2, Selected_Month2, StartDay);
            DateTime enddate2 = new DateTime(Selected_Year2, Selected_Month2, EndDay);
            var m1 =
                ds.Where(p => p.DATE >= startdate1 && p.DATE <= enddate1);
            var m2 = ds.Where(p => p.DATE >= startdate2 && p.DATE <= enddate2);
            if (Selected_AreaID != 0)
            {
                m1 = m1.Where(p => p.AreaID == Selected_AreaID);
                m2 = m2.Where(p => p.AreaID == Selected_AreaID);
            }

            List<UserMS.DemoReport.SellChartModel> results = new List<UserMS.DemoReport.SellChartModel>();
            for (int i = StartDay; i <= EndDay; i++)
            {
                results.Add(new UserMS.DemoReport.SellChartModel()
                {
                    Day = i,
                    M1 =  Convert.ToDecimal(m1.Any(p => p.DATE.Day == i) ? m1.Where(p => p.DATE.Day == i).Sum(p => p.SellPrice) : 0),
                    M2 = Convert.ToDecimal(m2.Any(p => p.DATE.Day == i) ? m2.Where(p => p.DATE.Day == i).Sum(p => p.SellPrice) : 0),
                });

            }
            _result = results;
            OnPropertyChanged("result");
            OnPropertyChanged("Label1");
            OnPropertyChanged("Label2");

        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)

                this.OnUiThread(() => this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)));
            //handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected delegate void OnUiThreadDelegate();

        protected void OnUiThread(OnUiThreadDelegate onUiThreadDelegate)
        {
            // Are we on the Dispatcher thread ?
            //Deployment.Current.Dispatcher.BeginInvoke(onUiThreadDelegate);
            if (Application.Current.Dispatcher.CheckAccess())
            {
                onUiThreadDelegate();
            }
            else
            {
                // We are not on the UI Dispatcher thread so invoke the call on it.
                Application.Current.Dispatcher.BeginInvoke(onUiThreadDelegate);
            }
        }

    }
}