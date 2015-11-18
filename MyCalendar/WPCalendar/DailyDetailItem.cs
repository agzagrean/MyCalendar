using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPCalendar.Helpers;
using WPCalendar.Models;

namespace WPCalendar
{
    public class DailyDetailItem : Button
    {
        #region Members

        public readonly EventItem eventItem;


        #endregion


        #region Properties

       


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            internal set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DailyDetailItem), new PropertyMetadata(""));


        #endregion

        #region Event handler
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

       /// <summary>
        /// Create new instance of a calendar cell
        /// </summary>
   
        [Obsolete("Internal use only")]
        public DailyDetailItem()
        {
            DefaultStyleKey = typeof(DailyDetailItem);
        }


        public DailyDetailItem(EventItem item)
        {
            DefaultStyleKey = typeof(DailyDetailItem);
            Height = Constants.ALL_DAY_EVENT_ITEM_HEIGHT;
            this.eventItem = item;
            this.CommandParameter = item;
            this.Title = eventItem.EventTitle;
            this.FontSize = Constants.EVENT_FONT_SIZE;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetBackground();
            SetForeground();
        }

        private void SetBackground()
        {
            if (eventItem != null)
                Background = eventItem.EventColor;
            else
                Background = new SolidColorBrush();
        }

        private void SetForeground()
        {
            Foreground = new SolidColorBrush(Colors.White);
        }
    }
}
