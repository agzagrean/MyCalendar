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
using WPCalendar.Models;

namespace WPCalendar.Helpers
{
    public static class ExtensionMethods
    {
        public static bool Equals(this EventItem eventItem, EventItem eventItem2)
        {
            return eventItem.EventTitle.Equals(eventItem2.EventTitle) &&
                eventItem.EventLocation.Equals(eventItem2.EventLocation) &&
                eventItem.EventStart.Equals(eventItem2.EventStart) &&
                eventItem.EventEnd.Equals(eventItem2.EventEnd) &&
                eventItem.EventColor.Equals(eventItem2.EventColor) &&
                eventItem.EventType.Equals(eventItem2.EventType);
        }

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

        public static bool IsEqual(this DateTime date, DateTime dateToCompare)
        {
            DateTime date1 = new DateTime(date.Year, date.Month, date.Day);
            DateTime date2 = new DateTime(dateToCompare.Year, dateToCompare.Month, dateToCompare.Day);

            return date1 == date2;
        }


        #region Grid
        public static void GenerateLines(this Grid grid)
        {
            if (!grid.ShowGridLines)
            {
                int noOfColumns = grid.ColumnDefinitions.Count;
                int noOfRows = grid.RowDefinitions.Count;

                grid.SetRowSeparators(noOfColumns, noOfRows);
                grid.SetColumnSeparators(noOfColumns, noOfRows);
            }
        }

        private static void SetRowSeparators(this Grid grid, int noOfColumns, int noOfRows)
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

        private static void SetColumnSeparators(this Grid grid, int noOfColumns, int noOfRows)
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
