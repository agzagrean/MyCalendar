﻿using System;

namespace WPCalendar
{
    /// <summary>
    /// Event arguments used for MonthChanging and MonthChanged events of the calendar
    /// </summary>
    public class MonthChangedEventArgs : EventArgs
    {
        private MonthChangedEventArgs() { }

        internal MonthChangedEventArgs(int year, int month)
        {
            Year = year;
            Month = month;
        }

        /// <summary>
        /// Year for newly selected month/year combination
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// Month for newly selected month/year combination
        /// </summary>
        public int Month { get; private set; }
    }
}
