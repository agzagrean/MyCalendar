using System.Windows.Media;
using System;
using WPCalendar.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace WPCalendar
{
    /// <summary>
    /// This converter can be used to control day number color and background color
    /// for each day
    /// </summary>
    public interface IDateToBrushConverter
    {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="dateTime"></param>
      /// <param name="isSelected"></param>
      /// <param name="eventsForDay"></param>
      /// <returns></returns>
        Brush Convert(DateTime dateTime, bool isSelected, List<EventItem> eventsForDay);
    }
}
