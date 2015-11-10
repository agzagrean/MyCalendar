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
                calendarItem._owningCalendar.popup.Child = new PopupAddEditChild(calendarItem);
            calendarItem._owningCalendar.popup.IsOpen = true;
        }
    }
}
