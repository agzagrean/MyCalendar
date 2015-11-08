using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPCalendar.Models;

namespace WPCalendar.Helpers
{
    public static class ExtensionMethods
    {
        public static void SetDayType(this CalendarItem calendarItem, EventCalendar calendar)
        {
            if (calendar != null && calendar.AllEvents != null)
            {
                List<EventItem> evItems = HelperMethods.GetEventItemsForDate(calendarItem.ItemDate, calendar.AllEvents);
                calendarItem.EventsForDay = evItems;
                calendarItem.BackgroundBrush = HelperMethods.GetBackgroundBrush(evItems, calendarItem.ItemDate, calendarItem.IsSelected);
            }
        }

        public static bool DateHasEvent(this DateTime date, EventItem item)
        {
            DateTime start = new DateTime(item.EventStart.Year, item.EventStart.Month, item.EventStart.Day);
            DateTime end = new DateTime(item.EventEnd.Year, item.EventEnd.Month, item.EventEnd.Day);

            return start <= date && end >= date;
        }

        #region calendar

        public static void GenerateDayEvents(this Calendar calendar, List<EventItem> events)
        {
            calendar.ConstructAllDayEvents(events, 450, 60);
            List<EventItem> hourEvents = events.Where(x => x.EventStart.Hour != x.EventEnd.Hour).ToList();
            calendar._hoursDetails.GenerateHourEvents(hourEvents);
        }

        private static void ConstructAllDayEvents(this Calendar calendar, List<EventItem> eventsForDay, double width, double heigth)
        {
            if (eventsForDay.Count == 0)
                return;

            int index = 0; int maxlines = 5;

            if (eventsForDay.Count <= maxlines)
                maxlines = eventsForDay.Count;
            for (index = 0; index < maxlines; index++)
            {
                calendar._spAllDayEvents.Children.Add(
                    new Button()
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
                    });
            }

            calendar._spAllDayEvents.Children.Add(new Rectangle()
            {
                Width = width,
                Height = 1.3,
                Stroke = new SolidColorBrush(Colors.LightGray),
                StrokeThickness = 1.3,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            });

            calendar.ResizeUIControls(width, maxlines);
        }

        private static void ResizeUIControls(this Calendar calendar, double width, int maxlines)
        {
            calendar._spAllDayEvents.Width = width;

            double newSpAllDayEventHeight = maxlines * 60;
            if (newSpAllDayEventHeight < 100)
                calendar._spAllDayEvents.Height = newSpAllDayEventHeight;
            else
                calendar._spAllDayEvents.Height = 100;

            calendar.ResizeScrollviewer();
        }

        public static void ResizeScrollviewer(this Calendar calendar)
        {
            double maxHeight = calendar._scrollViewerHours.ExtentHeight;
            if (!double.IsNaN(maxHeight) && maxHeight != 0)
            {
                if (maxHeight > 700)
                {
                    double newScrollViewerHeight =
                        maxHeight -
                        calendar._spAllDayEvents.Height -
                        calendar._dayDetailsGrid.RowDefinitions[0].Height.Value;
                    if (newScrollViewerHeight > 0)
                        calendar._scrollViewerHours.Height = newScrollViewerHeight;
                }
            }
        }

        public static void GenerateHours(this Calendar calendar)
        {
            TextBlock tb;
            for (int i = 0; i < 24; i++)
            {
                tb = new TextBlock()
                {
                    Text = i.ToString("D2"),
                    Foreground = new SolidColorBrush(Colors.DarkGray),
                    FontSize = 22
                };
                tb.SetValue(Grid.RowProperty, i);
                tb.SetValue(Grid.ColumnProperty, 0);
                calendar._hoursDetails.Children.Add(tb);
            }
        }

        private static void GenerateHourEvents(this Grid hoursCalendar, List<EventItem> hourEvents)
        {
            Button button;
            foreach (EventItem item in hourEvents)
            {
                button = new Button()
                {
                    Content = item.EventTitle,
                    Background = item.EventColor,
                    Foreground = new SolidColorBrush(Colors.White),
                };


                int hours = item.EventEnd.Hour - item.EventStart.Hour;

                button.SetValue(Grid.RowProperty, item.EventStart.Hour);

                button.SetValue(Grid.RowSpanProperty, hours);

                button.SetValue(Grid.ColumnProperty, 1);
                hoursCalendar.Children.Add(button);
            }
        }

          
        #endregion


        #region GridControl
        public static void GenerateLines(this GridControl grid)
        {
            if (!grid.ShowGridLines)
            {
                int noOfColumns = grid.ColumnDefinitions.Count;
                int noOfRows = grid.RowDefinitions.Count;

                grid.SetRowSeparators(noOfColumns, noOfRows);
                grid.SetColumnSeparators(noOfColumns, noOfRows);
            }
        }

        private static void SetRowSeparators(this GridControl grid, int noOfColumns, int noOfRows)
        {
            for (int row = 0; row < noOfRows; row++)
            {
                Rectangle horizontalLine = new Rectangle();
                horizontalLine.Stroke = new SolidColorBrush(Colors.LightGray);
                horizontalLine.Height = 0.5;
                //GridLineThickness;
                horizontalLine.StrokeThickness = 0.5;
                //GridLineThickness;
                horizontalLine.VerticalAlignment = VerticalAlignment.Bottom;

                horizontalLine.SetValue(Grid.RowProperty, row);
                horizontalLine.SetValue(Grid.ColumnSpanProperty, noOfColumns);

                grid.Children.Add(horizontalLine);
            }


        }

        private static void SetColumnSeparators(this GridControl grid, int noOfColumns, int noOfRows)
        {
            for (int column = 0; column < noOfColumns; column++)
            {
                Rectangle verticalLine = new Rectangle();
                verticalLine.Stroke = new SolidColorBrush(Colors.LightGray);
                verticalLine.Width = 0.5;
                //GridLineThickness;
                verticalLine.StrokeThickness = 0.5;
                //GridLineThickness;
                verticalLine.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

                verticalLine.SetValue(Grid.ColumnProperty, column);
                verticalLine.SetValue(Grid.RowProperty, 1);
                verticalLine.SetValue(Grid.RowSpanProperty, noOfRows - 1);

               grid.Children.Add(verticalLine);
            }
        }
        
        
        #endregion
    }
}
