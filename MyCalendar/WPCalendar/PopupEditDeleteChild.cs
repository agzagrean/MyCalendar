using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WPCalendar.Helpers;
using WPCalendar.Models;

namespace WPCalendar
{
    public class PopupEditDeleteChild : Control, INotifyPropertyChanged
    {
        #region Private

        Button btnBackToCalendar, btnEditEvent, btnDeleteEvent;
        Border borderTitle;
        Grid gridDetails;
        TextBlock tbEventTitle, tbEventLocation, tbEventDate;

        protected CalendarItem _owningCalendarItem;
        protected EventItem eventItem;
        #endregion

        #region Properties
        public string EventTitle
        {
            get { return (string)GetValue(EventTitleProperty); }
            internal set { SetValue(EventTitleProperty, value); }
        }

        public static readonly DependencyProperty EventTitleProperty =
            DependencyProperty.Register("EventTitle", typeof(string), typeof(PopupEditDeleteChild), new PropertyMetadata(""));

        public string EventLocation
        {
            get { return (string)GetValue(EventLocationProperty); }
            internal set { SetValue(EventLocationProperty, value); }
        }

        public static readonly DependencyProperty EventLocationProperty =
            DependencyProperty.Register("EventLocation", typeof(string), typeof(PopupEditDeleteChild), new PropertyMetadata(""));

        public string EventDate
        {
            get { return (string)GetValue(EventDateProperty); }
            internal set { SetValue(EventDateProperty, value); }
        }

        public static readonly DependencyProperty EventDateProperty =
            DependencyProperty.Register("EventDate", typeof(string), typeof(PopupEditDeleteChild), new PropertyMetadata(""));

        private SolidColorBrush _eventColor;
        public SolidColorBrush EventColor
        {
            get
            { return _eventColor; }
            set
            {
                if (value != _eventColor)
                    _eventColor = value;
                NotifyPropertyChanged("EventColor");
            }
        }
    
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
        public PopupEditDeleteChild(EventItem item, CalendarItem calendarItem)
        {
            DefaultStyleKey = typeof(PopupEditDeleteChild);
            eventItem = item;
            EventTitle = item.EventTitle;
            EventLocation = item.EventLocation;
            EventColor = item.EventColor;
            string dateTimeFormat = "ddd, dd MMM, HH:mm";
            EventDate = string.Format("{0} - {1}", eventItem.EventStart.ToString(dateTimeFormat), eventItem.EventEnd.ToString(dateTimeFormat));
            _owningCalendarItem = calendarItem;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AssignUIElements();
            Init();
        }

        private void Init()
        {
            this.Height = (this.Parent as Popup).Height;
            this.Width = (this.Parent as Popup).Width;

            btnEditEvent.CommandParameter = eventItem;
        }

        private void AssignUIElements()
        {
            

            btnBackToCalendar = GetTemplateChild("BtnBackToCalendar") as Button;
            btnBackToCalendar.Click += BackToCalendar;

            btnEditEvent = GetTemplateChild("BtnEditEvent") as Button;
            btnEditEvent.Click += EditEvent;

            btnDeleteEvent = GetTemplateChild("BtnDeleteEvent") as Button;
            btnDeleteEvent.Click += DeleteEvent;

            borderTitle = GetTemplateChild("BorderTitle") as Border;
            gridDetails = GetTemplateChild("gridDetails") as Grid;

            tbEventTitle = GetTemplateChild("TbEventTitle") as TextBlock;
            tbEventLocation = GetTemplateChild("TbEventLocation") as TextBlock;
            tbEventDate = GetTemplateChild("TbEventDate") as TextBlock;
        }

        void DeleteEvent(object sender, System.Windows.RoutedEventArgs e)
        {
            _owningCalendarItem._owningCalendar.RegisterHourGridTap();

            if (_owningCalendarItem.EventsForDay.Contains(eventItem))
            {
                _owningCalendarItem.EventsForDay.Remove(eventItem);
                _owningCalendarItem._owningCalendar.EventsCalendar.AllEvents.Remove(eventItem);
            }

            //refresh calendarItem details
            _owningCalendarItem.Refresh();
            //refresh calendar details
            _owningCalendarItem._owningCalendar.Refresh();

            (this.Parent as Popup).IsOpen = false;
        }


        void EditEvent(object sender, System.Windows.RoutedEventArgs e)
        {
            EditAddDeleteHelpers.AddEditView(sender, _owningCalendarItem);
        }

        void BackToCalendar(object sender, System.Windows.RoutedEventArgs e)
        {
            _owningCalendarItem._owningCalendar.RegisterHourGridTap();

            (this.Parent as Popup).IsOpen = false;
        }
    }
}
