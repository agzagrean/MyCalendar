using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPCalendar.Helpers;
using WPCalendar.Models;

namespace WPCalendar
{
    public partial class Calendar
    {

        #region Switch views

        public void SwitchToMonthView()
        {
            _itemsGrid.Visibility = nextButton.Visibility = tbYearMonthLabel.Visibility = Visibility.Visible;
            _dayDetailsGrid.Visibility = spDetailsHeader.Visibility = Visibility.Collapsed;

            ClearDetailsGrid();

            UnregisterHourGridTap();

            // reenable gestures
           // EnableGesturesSupport();
        }

        public void ClearDetailsGrid()
        {
            spAllDayEvents.Height = 0;
            spAllDayEvents.Children.Clear();

            hoursDetailsGrid.Children.Clear();
            scrollViewerHours.ScrollToVerticalOffset(0);
        }


        public void RegisterHourGridTap()
        {
            hoursDetailsGrid.Tap += AddEventItem;
        }
        public void UnregisterHourGridTap()
        {
            hoursDetailsGrid.Tap -= AddEventItem;
        }

        public void SwitchToDetailsView()
        {
            _itemsGrid.Visibility  = nextButton.Visibility = tbYearMonthLabel.Visibility = Visibility.Collapsed;
            _dayDetailsGrid.Visibility = spDetailsHeader.Visibility = Visibility.Visible;

            hoursDetailsGrid.GenerateLines();
            hoursDetailsGrid.Tap += AddEventItem;

            //DisableGesturesSupport();
        }
        #endregion


        public void AddEventItem(object sender, GestureEventArgs e)
        {
            Grid grid = sender as Grid;
            Point point = e.GetPosition(grid);

            string eventTitle = " + New title event";
            double width = (grid.Parent as ScrollViewer).Width - 50;

            DateTime dateTime = _lastItem.ItemDate;
            int hour = (int)Math.Ceiling((point.Y) / Constants.GRID_HOURS_CELL_HEIGHT);
            EventItem eventItem = new EventItem()
               {
                   EventColor = CustomColor.CornflowerBlue,
                   EventStart = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, 0, 0),
                   EventEnd = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour + 1, 0, 0),
                   EventTitle = eventTitle,
                   EventLocation = string.Empty
               };

            DailyDetailItem eventDetails = new DailyDetailItem(eventItem);
            eventDetails.Click += EditEvent;

            _lastItem.EventsForDay.Add(eventItem);

            eventDetails.SetValue(Grid.RowProperty, hour);
            eventDetails.SetValue(Grid.RowSpanProperty, 1);
            eventDetails.SetValue(Grid.ColumnProperty, 1);
            grid.Children.Add(eventDetails);
        }

        private void EditEvent(object sender, RoutedEventArgs e)
        {
            EditAddDeleteHelpers.AddEditView(sender, _lastItem);
        }
        

    }
}
