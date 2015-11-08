using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPCalendar.Helpers;
using WPCalendar.Models;

namespace WPCalendar
{
    public partial class CalendarItem
    {
        public void GenerateDayEvents(List<EventItem> events)
        {
            ConstructAllDayEvents(events, 450, Constants.ALL_DAY_EVENT_ITEM_HEIGHT);
            List<EventItem> hourEvents = events.Where(x => x.EventStart.Hour != x.EventEnd.Hour).ToList();
            GenerateHourEvents(hourEvents);
        }

        private void ConstructAllDayEvents(List<EventItem> eventsForDay, double width, double heigth)
        {
            if (eventsForDay.Count > 0)
                ResizeUIControls(0);

            int index = 0; int maxlines = 5;

            if (eventsForDay.Count <= maxlines)
                maxlines = eventsForDay.Count;

            Button btnDayEvent;
            for (index = 0; index < maxlines; index++)
            {
                btnDayEvent = new Button()
                    {
                        Width = width,
                        Height = heigth,
                        Margin = new Thickness(0, -10, 0, 0),
                        FontSize = 16,
                        Background = eventsForDay[index].EventColor,
                        Content = eventsForDay[index].EventTitle,
                        Foreground = CustomColor.White,
                        BorderThickness = new Thickness(0),
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left,

                    };
                btnDayEvent.Click += EditEvent;
                _owningCalendar._spAllDayEvents.Children.Add(btnDayEvent);
            }

            _owningCalendar._spAllDayEvents.Children.Add(new Rectangle()
            {
                Height = 1.3,
                Stroke = new SolidColorBrush(Colors.LightGray),
                StrokeThickness = 1.3,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            });

            ResizeUIControls(maxlines * 60);
        }

        private void ResizeUIControls(double height)
        {
            if (height < Constants.MAX_ALL_DAY_EVENTS_HEIGHT)
                _owningCalendar._spAllDayEvents.Height = height;
            else
                _owningCalendar._spAllDayEvents.Height = Constants.MAX_ALL_DAY_EVENTS_HEIGHT;

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
                        _owningCalendar._spAllDayEvents.Height -
                        _owningCalendar._dayDetailsGrid.RowDefinitions[0].Height.Value;
                    if (newScrollViewerHeight > 0)
                        _owningCalendar._scrollViewerHours.Height = newScrollViewerHeight;
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
                _owningCalendar._hoursDetails.Children.Add(tb);
            }
        }

        private void GenerateHourEvents(List<EventItem> hourEvents)
        {
            Button button;
            foreach (EventItem item in hourEvents)
            {
                button = new Button()
                {
                    FontSize = Constants.EVENT_FONT_SIZE,
                    Content = item.EventTitle,
                    Background = item.EventColor,
                    Foreground = new SolidColorBrush(Colors.White),
                };

                int hours = item.EventEnd.Hour - item.EventStart.Hour;

                button.SetValue(Grid.RowProperty, item.EventStart.Hour);
                button.SetValue(Grid.RowSpanProperty, hours);
                button.SetValue(Grid.ColumnProperty, 1);
                _owningCalendar._hoursDetails.Children.Add(button);
                button.Click += EditEvent;
            }
        }


        protected void EditEvent(object sender, RoutedEventArgs e)
        {
            //todo
        }
    }
}
