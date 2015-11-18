using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPCalendar.Helpers;
using WPCalendar.Models;

namespace WPCalendar
{
    public partial class CalendarItem
    {
        public void GenerateDayEvents()
        {
            List<EventItem> allDayEvents = EventsForDay.Where(x => x.EventType == EventType.Allday).ToList();
            List<EventItem> hourEvents = EventsForDay.Where(x => x.EventType == EventType.Hourly).ToList();

            ConstructAllDayEvents(allDayEvents, Constants.ALL_DAY_EVENT_ITEM_WIDTH, Constants.ALL_DAY_EVENT_ITEM_HEIGHT);
            GenerateHourEvents(hourEvents);
        }

        private void ConstructAllDayEvents(List<EventItem> eventsForDay, double width, double heigth)
        {
            if (eventsForDay.Count > 0)
                ResizeUIControls(0);

            int index = 0; int maxlines = 5;

            if (eventsForDay.Count <= maxlines)
                maxlines = eventsForDay.Count;

            for (index = 0; index < maxlines; index++)
            {
                EventItem eventItem = eventsForDay[index];

                DailyDetailItem item = new DailyDetailItem( eventItem);

                item.Click += EditEvent;
         
                _owningCalendar.spAllDayEvents.Children.Add(item);
            }

            ResizeUIControls(maxlines * (Constants.ALL_DAY_EVENT_ITEM_HEIGHT + 5));
        }

        private void ResizeUIControls(double height)
        {
            if (height < Constants.MAX_ALL_DAY_EVENTS_HEIGHT)
                _owningCalendar.spAllDayEvents.Height = height;
            else
                _owningCalendar.spAllDayEvents.Height = Constants.MAX_ALL_DAY_EVENTS_HEIGHT;

            ResizeScrollviewer();
        }

        public void ResizeScrollviewer()
        {
            double maxHeight = Constants.MAX_HEIGHT_SCROLL;
                //_owningCalendar._scrollViewerHours.ExtentHeight;
            if (!double.IsNaN(maxHeight) && maxHeight != 0)
            {
                if (maxHeight >= Constants.MAX_HEIGHT_SCROLL)
                {
                    double newScrollViewerHeight =
                        maxHeight -
                        _owningCalendar.spAllDayEvents.Height -
                        _owningCalendar._dayDetailsGrid.RowDefinitions[0].Height.Value;
                    if (newScrollViewerHeight > 0)
                        _owningCalendar.scrollViewerHours.Height = newScrollViewerHeight;
                }
            }
        }

        public void GenerateHours()
        {
            TextBlock tb;
            for (int i = 0; i < 24; i++)
            {
                tb = new TextBlock()
                {
                    Height = Constants.GRID_HOURS_CELL_HEIGHT,
                    Text = i.ToString("D2"),
                    Foreground = new SolidColorBrush(Colors.DarkGray),
                    FontSize = Constants.HOUR_FONT_SIZE,
                };
                tb.SetValue(Grid.RowProperty, i + 1);
                tb.SetValue(Grid.ColumnProperty, 0);
                _owningCalendar.hoursDetailsGrid.Children.Add(tb);
            }
        }

        private void GenerateHourEvents(List<EventItem> hourEvents)
        {
            foreach (EventItem item in hourEvents)
            {
                DailyDetailItem detailItem = new DailyDetailItem(item);

                int hours = item.EventEnd.Hour - item.EventStart.Hour;

                TimeSpan timespan = item.EventEnd - item.EventStart;
                double pixels = timespan.Hours * 60 + timespan.Minutes;
                double marginTop = item.EventStart.Minute;
                double marginBottom = item.EventEnd.Minute; 

                detailItem.SetValue(Grid.RowProperty, item.EventStart.Hour +1);
                detailItem.SetValue(Grid.RowSpanProperty, hours);
                detailItem.SetValue(Grid.ColumnProperty, 1);
                detailItem.Height = pixels;

                detailItem.Margin = new Thickness(0, marginTop, 0, marginBottom);

                _owningCalendar.hoursDetailsGrid.Children.Add(detailItem);
                
               
                detailItem.Click += EditEvent;
            }
        }

        protected void EditEvent(object sender, RoutedEventArgs e)
        {
            _owningCalendar.UnregisterHourGridTap();
            Button button = sender as Button;
            EventItem item = button.CommandParameter as EventItem;
            if (item != null)
            {
                _owningCalendar.popup.Child = new PopupEditDeleteChild(item, this);
                _owningCalendar.popup.IsOpen = true;
            }
          
        }

    }
}
