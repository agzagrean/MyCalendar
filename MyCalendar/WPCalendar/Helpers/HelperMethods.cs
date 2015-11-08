using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPCalendar.Models;

namespace WPCalendar.Helpers
{
    public static class HelperMethods
    {
        
        public static EventCalendar GenerateMockCalendar()
        {
            List<EventItem> events = new List<EventItem>();
            events.Add(new EventItem(new DateTime(2015, 11, 2), new DateTime(2015, 11, 3), "dentist", "", CustomColor.Aqua));

            events.Add(new EventItem(new DateTime(2015, 11, 2), new DateTime(2015, 11, 4), "hair stylist", "", CustomColor.Green));
            events.Add(new EventItem(new DateTime(2015, 11, 5, 8, 0, 0), new DateTime(2015, 11, 5, 10, 0, 0), "Coffee", "", CustomColor.Marsala));
            events.Add(new EventItem(new DateTime(2015, 11, 5, 0, 0, 0), new DateTime(2015, 11, 5, 0, 0, 0), "meeting", "", CustomColor.Lavender));
            events.Add(new EventItem(new DateTime(2015, 11, 8), new DateTime(2015, 11, 15), "training", "",CustomColor.Yellow));
            events.Add(new EventItem(new DateTime(2015, 11, 30), new DateTime(2015, 12, 15), "exams", "", CustomColor.Peach));
            EventCalendar cal = new EventCalendar();
            cal.AllEvents = events;
            return cal;
        }
                        
        public static List<EventItem> GetEventItemsForDate(DateTime dateTime, List<EventItem>allEvents)
        {
            List<EventItem> events = new List<EventItem>();
            List<EventItem> myEvents = new List<EventItem>();
            if (allEvents != null)
                events = allEvents.OrderByDescending(x => x.EventStart).ToList();

            if (events != null && events.Count > 0)

                myEvents = events.Where(item => dateTime.DateHasEvent(item)).ToList();

            return myEvents;
        }
       
        #region set background

        public static Brush GetBackgroundBrush(List<EventItem> eventsForDay, DateTime dateTime, bool isSelected)
        {
            if (eventsForDay != null && eventsForDay.Count > 0)
            {
                return HelperMethods.ConstructBackground(eventsForDay, 60, 60, isSelected, dateTime == DateTime.Today);
            }
            return HelperMethods.SetSelectedTodayBackground(60, 60, isSelected, dateTime == DateTime.Today);
        }

        private static Brush ConstructBackground(List<EventItem> eventsForDay, double width, double heigth, bool isSelected, bool isToday)
        {
            if (eventsForDay.Count == 0)
                return new SolidColorBrush(Colors.Transparent);

            int index = 0; int maxlines = 3;

            Canvas canvasToBeBrush = new Canvas();

            canvasToBeBrush.Width = width;
            canvasToBeBrush.Height = heigth;

            if (eventsForDay.Count <= maxlines)
                maxlines = eventsForDay.Count;
            for (index = 0; index < maxlines; index++)
            {
                canvasToBeBrush.Children.Add(
                    new Rectangle()
                    {
                        Width = width,
                        Height = 10,
                        Margin = new System.Windows.Thickness(0, index * 10, 0, 0),
                        Fill = eventsForDay[index].EventColor
                    });
            }

            WriteableBitmap bitmapToBrush = new WriteableBitmap(canvasToBeBrush, null);
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = bitmapToBrush;

            return myBrush;
        }

        private static Brush SetSelectedTodayBackground(double width, double heigth, bool isSelected, bool isToday)
        {
            Canvas canvasToBeBrush = new Canvas();

            canvasToBeBrush.Width = width;
            canvasToBeBrush.Height = heigth;
            WriteableBitmap bitmapToBrush = new WriteableBitmap(canvasToBeBrush, null);
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = bitmapToBrush;

            return myBrush;
        }

        private static void SetCanvasBackground(bool isSelected, double width, double heigth, bool isToday, Canvas canvasToBeBrush)
        {

            Border border = new Border()
            {
                BorderBrush = CustomColor.Gray,
                BorderThickness = new System.Windows.Thickness(2),
                Width = width,
                Height = heigth,
                Opacity = 0.7
            };

            if (isSelected)
            {
                // canvasToBeBrush.Background = ;
            }
            else
                if (isToday)
                    canvasToBeBrush.Background = CustomColor.Gray;
        }
        
        #endregion
    }
}
