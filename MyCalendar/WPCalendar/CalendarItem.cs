using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPCalendar;
using WPCalendar.Helpers;
using WPCalendar.Models;
using System.Linq;
using System.Windows.Input;

namespace WPCalendar
{
    /// <summary>
    /// This class corresponds to a calendar item / cell
    /// </summary>
    public partial class CalendarItem : Button
    {
        #region Members

        readonly Calendar _owningCalendar;

        #endregion

        #region Constructor

        /// <summary>
        /// Create new instance of a calendar cell
        /// </summary>
        [Obsolete("Internal use only")]
        public CalendarItem()
        {
            DefaultStyleKey = typeof(CalendarItem);
        }

        /// <summary>
        /// Create new instance of a calendar cell
        /// </summary>
        /// <param name="owner">Calenda control that a cell belongs to</param>
        public CalendarItem(Calendar owner)
        {
            DefaultStyleKey = typeof(CalendarItem);
            _owningCalendar = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Day number for this calendar cell.
        /// This changes depending on the month shown
        /// </summary>
        public int DayNumber
        {
            get { return (int)GetValue(DayNumberProperty); }
            internal set { SetValue(DayNumberProperty, value); }
        }

        /// <summary>
        /// Day number for this calendar cell.
        /// This changes depending on the month shown
        /// </summary>
        public static readonly DependencyProperty DayNumberProperty =
            DependencyProperty.Register("DayNumber", typeof(int), typeof(CalendarItem), new PropertyMetadata(0, OnDayNumberChanged));

        private static void OnDayNumberChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var item = source as CalendarItem;
            if (item != null)
            {
                item.SetForecolor();
                item.SetBackcolor();
            }
        }

        internal bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
       
        internal static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(CalendarItem), new PropertyMetadata(false, OnIsSelectedChanged));
 
        private List<EventItem> _eventsForDay;
        public List<EventItem> EventsForDay
        {
            get { return _eventsForDay; }
            set { if (value != _eventsForDay) _eventsForDay = value; }
        }

        private static void OnIsSelectedChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var item = source as CalendarItem;
            if (item != null)
            {
                item.SetBackcolor();
                item.SetForecolor();
            }
        }

        /// <summary>
        /// Date for the calendar item
        /// </summary>
        public DateTime ItemDate
        {
            get { return (DateTime)GetValue(ItemDateProperty); }
            internal set { SetValue(ItemDateProperty, value); }
        }

        /// <summary>
        /// Date for the calendar item
        /// </summary>
        internal static readonly DependencyProperty ItemDateProperty =
            DependencyProperty.Register("ItemDate", typeof(DateTime), typeof(CalendarItem), new PropertyMetadata(null));



        /// <summary>
        /// Date for the calendar item
        /// </summary>
        public Brush BackgroundBrush
        {
            get { return (Brush)GetValue(BackgroundBrushProperty); }
            internal set { SetValue(BackgroundBrushProperty, value); }
        }

        /// <summary>
        /// Date for the calendar item
        /// </summary>
        internal static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(CalendarItem), new PropertyMetadata(null));
        #endregion

        #region Template

        /// <summary>
        /// Apply default template and perform initialization
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Background = new SolidColorBrush(Colors.Transparent);
            Foreground = Application.Current.Resources["PhoneForegroundBrush"] as Brush;
            SetBackcolor();
            SetForecolor();

         //   _owningCalendar._scrollViewerHours.SizeChanged += _scrollViewerHours_SizeChanged;
        }

        public void _scrollViewerHours_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            if (double.IsNaN(sv.Height))
                ResizeScrollviewer();
        }

        private bool IsConverterNeeded()
        {
            bool returnValue = true;
            if (_owningCalendar.DatesSource != null)
            {
                if (!_owningCalendar.DatesAssigned.Contains(ItemDate))
                {
                    returnValue = false;
                }
            }
            return returnValue;
        }

        internal void SetBackcolor()
        {
            if (BackgroundBrush != null)
                Background = BackgroundBrush;
            else
                Background = new SolidColorBrush();
        }

        internal void SetForecolor()
        {
            var defaultBrush = Application.Current.Resources["PhoneForegroundBrush"] as Brush;
         
            if (ItemDate == DateTime.Today)
                Foreground = new SolidColorBrush(Color.FromArgb(255, 191, 82, 121));
            else
                Foreground = defaultBrush;
        }

        #endregion

        #region Day events

        public void DisplayDetailView()
        {
            _owningCalendar.SwitchToDetailsView();
 
            GenerateHours();
            GenerateDayEvents(EventsForDay);   
        }

     
        #endregion

      

    }
}
