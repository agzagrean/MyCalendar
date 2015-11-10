using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPCalendar.Helpers;

namespace WPCalendar
{
    public partial class Calendar
    {

        #region Switch views

        public void SwitchToMonthView()
        {
            _itemsGrid.Visibility = previousButton.Visibility = nextButton.Visibility = tbYearMonthLabel.Visibility = Visibility.Visible;
            _dayDetailsGrid.Visibility = backToMonthViewButton.Visibility = Visibility.Collapsed;

            _spAllDayEvents.Height = 0;
            _spAllDayEvents.Children.Clear();

            _hoursDetails.Children.Clear();
            _scrollViewerHours.ScrollToVerticalOffset(0);

            UnregisterHourGridTap();

            _itemsGrid.GenerateLines();

            // reenable gestures
            EnableGesturesSupport();
        }


        public void RegisterHourGridTap()
        {
            _hoursDetails.Tap += AddEventItem;
        }
        public void UnregisterHourGridTap()
        {
            _hoursDetails.Tap -= AddEventItem;
        }

        public void SwitchToDetailsView()
        {
            _itemsGrid.Visibility = previousButton.Visibility = nextButton.Visibility = tbYearMonthLabel.Visibility = Visibility.Collapsed;
            _dayDetailsGrid.Visibility = backToMonthViewButton.Visibility = Visibility.Visible;

            _hoursDetails.GenerateLines();
          
            _hoursDetails.Tap += AddEventItem;

            //you don't want flicking years when scrolling into day details
            DisableGesturesSupport();
        }
        #endregion


        public void AddEventItem(object sender, GestureEventArgs e)
        {
            Grid grid = sender as Grid;
            Point point = e.GetPosition(grid);

            string eventTitle = " + New title event";
            double width = (grid.Parent as ScrollViewer).Width - 50;

            Button button = new Button()
            {
                Width = width,
                FontSize = Constants.EVENT_FONT_SIZE,
                Content = eventTitle,
                Foreground = CustomColor.White,
                Background = CustomColor.CornflowerBlue,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left
            };
            button.Click += EditEvent;

            int hour = (int)Math.Ceiling((point.Y) / Constants.GRID_HOURS_CELL_HEIGHT);

            DateTime dateTime = _lastItem.ItemDate;
         
            _lastItem.EventsForDay.Add(new Models.EventItem()
            {
                EventStart = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, 0, 0),
                EventEnd = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour + 1, 0, 0),
                EventTitle = eventTitle,
                EventLocation = string.Empty
            });

            button.SetValue(Grid.RowProperty, hour);
            button.SetValue(Grid.RowSpanProperty, 1);
            button.SetValue(Grid.ColumnProperty, 1);
            grid.Children.Add(button);
        }

        private void EditEvent(object sender, RoutedEventArgs e)
        {
            EditAddDeleteHelpers.AddEditView(sender, _lastItem);
        }
        

    }
}
