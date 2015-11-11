using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPCalendar.Models;

namespace WPCalendar.Helpers
{
    public static class EditAddDeleteHelpers
    {
        internal static void AddEditView(object sender, CalendarItem calendarItem)
        {
            calendarItem._owningCalendar.UnregisterHourGridTap();
            Button button = sender as Button;

            EventItem item = button.CommandParameter as EventItem;
            if (item != null)
                calendarItem._owningCalendar.popup.Child = new PopupAddEditChild(item, calendarItem);
            else
            {
                DateTime date = calendarItem.ItemDate;
                DateTime start = date;
                DateTime end = date;

                //new event
                if (button != null)
                {
                    int hourStart = (int)button.GetValue(Grid.RowProperty);
                    int hourEnd = hourStart + (int)button.GetValue(Grid.RowSpanProperty);

                    start = new DateTime(date.Year, date.Month, date.Day, hourStart, 0, 0);
                    end = new DateTime(date.Year, date.Month, date.Day, hourEnd, 0, 0);
                }
                calendarItem._owningCalendar.popup.Child = new PopupAddEditChild(calendarItem, start, end);
            }
            calendarItem._owningCalendar.popup.IsOpen = true;
        }
    }
}
