using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPControls;
using WPControls.Helpers;
using WPControls.Models;

namespace MyCalendar.Converters
{
    public class BackgroundColorConverter : IDateToBrushConverter
    {

        public Brush Convert(DateTime dateTime, bool isSelected, List<EventItem> eventsForDay, Brush defaultBrush_selected, Brush defaultBrush_today)
        {

            if (eventsForDay != null && eventsForDay.Count > 0)
            {
                return HelperMethods.ConstructBackground(eventsForDay, 60, 60, isSelected, dateTime == DateTime.Today, defaultBrush_selected, defaultBrush_today);
            }
            return HelperMethods.SetSelectedTodayBackground(60, 60, isSelected, dateTime == DateTime.Today, defaultBrush_selected, defaultBrush_today);
        }
    }
}
