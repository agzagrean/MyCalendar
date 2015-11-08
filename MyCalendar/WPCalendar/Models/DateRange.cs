﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPCalendar.Models
{
    public interface IRange<T>
    {
        T Start { get; }
        T End { get; }
        bool Includes(T value);
        bool Includes(IRange<T> range);
    }

    public class DateRange : IRange<DateTime>
    {
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public bool Includes(DateTime value)
        {
            return (Start <= value) && (value <= End);
        }

        public bool Includes(IRange<DateTime> range)
        {
            return (Start <= range.Start) && (range.End <= End);
        }

        public bool IsOverlapping(IRange<DateTime> range)
        {
            return
                   (Start <= range.Start && End >= range.End)
                ||
                   (Start <= range.End && End >= range.End)
                ||
                   (Start <= range.Start) && (range.Start <= End)
                ||
                    (Start <= range.End) && (range.End <= End)
                ||
                    (range.Start >= Start) && (Start <= range.End)
                ||
                    (range.Start >= End) && (End <= range.End);
        }
    }
}
