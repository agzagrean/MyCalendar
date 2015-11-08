using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPCalendar.Helpers;
using WPCalendar.Models;

namespace WPCalendar.Converters
{
    public class ColorConverter : IDateToBrushConverter
    {
        public Brush Convert(DateTime dateTime, bool isSelected, List<EventItem> eventsForDay)
        {
           return HelperMethods.GetBackgroundBrush(eventsForDay, dateTime, isSelected);  
        }
    }
}
