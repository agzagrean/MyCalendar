using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPCalendar.Models;

namespace WPCalendar.Converters
{
    public class EventTypeToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value is EventType)
            {
                EventType evType = (EventType)value;
                return evType == EventType.Allday ? true : false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && (bool)value)
            {
                return (bool)value ? EventType.Allday : EventType.Hourly;
            }

            return EventType.Hourly;
        }
    }
}
