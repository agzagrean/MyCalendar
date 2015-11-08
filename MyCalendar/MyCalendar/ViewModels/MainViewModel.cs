using WPCalendar.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPCalendar;
using WPCalendar.Models;

namespace MyCalendar.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Event handlers
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


        private EventCalendar customCalendar;
        public EventCalendar CustomCalendar
        {
            get
            {
                if (customCalendar == null)
                    customCalendar = HelperMethods.GenerateMockCalendar();

                return customCalendar;
            }
            set { customCalendar = value; }
        }
    }
}
