using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPCalendar.Models;

namespace WPCalendar
{
    public class CalendarChangedEventArgs: EventArgs
    {
        // ReSharper disable UnusedMember.Local
        private CalendarChangedEventArgs() { }
        // ReSharper restore UnusedMember.Local

        internal CalendarChangedEventArgs(EventCalendar calendar)
        {
            Calendar = calendar;
        }

        /// <summary>
        /// Date that is currently selected on the calendar
        /// </summary>
        public EventCalendar Calendar { get; private set; }
    }
}
