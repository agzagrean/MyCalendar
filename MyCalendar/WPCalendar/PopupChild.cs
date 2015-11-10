using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPCalendar.Resources;
using WPCalendar.Models;
using WPCalendar.Helpers;
using System.Windows.Data;

namespace WPCalendar
{
   public class PopupChild:Control
   {


       #region Private
       Button cancelBtn, saveBtn;
       CheckBox cbAllDayEvent;

       DatePicker startDatePicker,endDatePicker;
       TimePicker startTimePicker, endTimePicker;

       Border selectedEventColor;

       Grid eventColoursGrid;

       TextBox tbName, tbLocation;
       #endregion


       #region Properties
       
       public string EventTitle
       {
           get { return (string)GetValue(EventTitleProperty); }
           internal set { SetValue(EventTitleProperty, value); }
       }

       public static readonly DependencyProperty EventTitleProperty =
           DependencyProperty.Register("EventTitle", typeof(string), typeof(PopupChild), new PropertyMetadata(""));

       public string EventLocation
       {
           get { return (string)GetValue(EventLocationProperty); }
           internal set { SetValue(EventLocationProperty, value); }
       }

       public static readonly DependencyProperty EventLocationProperty =
           DependencyProperty.Register("EventLocation", typeof(string), typeof(PopupChild), new PropertyMetadata(""));


       public Brush EventColor
       {
           get;
           internal set;
       }
    
       public DateTime EventStart
       {
           get;
           internal set;
       }
      
       public DateTime EventEnd
       {
           get;
           internal set;
       }
    


       #endregion

       public PopupChild()
       {
           DefaultStyleKey = typeof(PopupChild);
           EventTitle = ApplicationResources.NewEventTitle;
           EventLocation = ApplicationResources.NewEventLocation;
          
           EventColor = CustomColor.Aquamarine;
       }

       public PopupChild(DateTime start, DateTime end)
       {
           DefaultStyleKey = typeof(PopupChild);
           EventTitle = ApplicationResources.NewEventTitle;
           EventLocation = ApplicationResources.NewEventLocation;
           EventStart = start;
           EventEnd = end;
           EventColor = CustomColor.Aquamarine;
       }

       public PopupChild(EventItem item)
       {
           DefaultStyleKey = typeof(PopupChild);
           EventTitle = item.EventTitle;
           EventLocation = item.EventLocation;
           EventColor = item.EventColor;

           EventStart = item.EventStart;
           EventEnd = item.EventEnd;
       }

       public override void OnApplyTemplate()
       {
           base.OnApplyTemplate();

            Initialize();

            selectedEventColor.Background = EventColor;
            startDatePicker.Value = EventStart;
            endDatePicker.Value = EventEnd;
            startTimePicker.Value = EventStart;
            endTimePicker.Value = EventEnd;
       }

       private void Initialize()
       {
           this.Height = (this.Parent as Popup).Height;
           this.Width = (this.Parent as Popup).Width;


           cancelBtn = GetTemplateChild("CancelButton") as Button;
           if (cancelBtn != null)
               cancelBtn.Click += CancelButtonClick;

           saveBtn = GetTemplateChild("SaveButton") as Button;
           if (saveBtn != null)
               saveBtn.Click += SaveButtonClick;

           tbName = GetTemplateChild("TbName") as TextBox;
           tbName.Loaded += TextBoxLoaded;
           tbName.GotFocus += TextBoxGotFocus;
           tbName.LostFocus += TextBoxLostFocus;

           tbLocation = GetTemplateChild("TbLocation") as TextBox;
           tbLocation.Loaded += TextBoxLoaded;
           tbLocation.GotFocus += TextBoxGotFocus;
           tbLocation.LostFocus += TextBoxLostFocus;

           cbAllDayEvent = GetTemplateChild("CBAllDayEvents") as CheckBox;
           cbAllDayEvent.Checked += AllDayEventChecked;
           cbAllDayEvent.Unchecked += AllDayEventUnChecked;

           startDatePicker = GetTemplateChild("startDatePicker") as DatePicker;
           startTimePicker = GetTemplateChild("startTimePicker") as TimePicker;

           endDatePicker = GetTemplateChild("endDatePicker") as DatePicker;
           endTimePicker = GetTemplateChild("endTimePicker") as TimePicker;

           selectedEventColor = GetTemplateChild("SelectedEventColor") as Border;

           eventColoursGrid = GetTemplateChild("EventColoursGrid") as Grid;
           foreach (UIElement child in eventColoursGrid.Children)
           {
               if (child.GetType() == typeof(Rectangle))
                   child.Tap += PickColor;
           }
       }

       private void TextBoxLoaded(object sender, RoutedEventArgs e)
       {
           TextBox tb = sender as TextBox;
           if (tb.Text.Equals(ApplicationResources.NewEventTitle) || tb.Text.Equals(ApplicationResources.NewEventLocation))
               tb.Foreground = new SolidColorBrush(Colors.DarkGray);
           else
               tb.Foreground = new SolidColorBrush(Colors.Black);
       }

       private void TextBoxLostFocus(object sender, RoutedEventArgs e)
       {
           TextBox tb = sender as TextBox;
           if (string.IsNullOrEmpty(tb.Text))
           {
               if (tb == tbName)
                   tb.Text = ApplicationResources.NewEventTitle;
               if (tb == tbLocation)
                   tb.Text = ApplicationResources.NewEventLocation;
               tb.Foreground = new SolidColorBrush(Colors.DarkGray);
           }
           else
               tb.Foreground = new SolidColorBrush(Colors.Black);
           tb.BorderBrush = new SolidColorBrush(Colors.DarkGray);
       }

       void TextBoxGotFocus(object sender, RoutedEventArgs e)
       {
           TextBox tb = sender as TextBox;
           tb.BorderBrush = new SolidColorBrush(Colors.Black);
           if (tb.Text.Equals(ApplicationResources.NewEventTitle) || tb.Text.Equals(ApplicationResources.NewEventLocation))
               tb.Text = string.Empty;
       }

       void PickColor(object sender, System.Windows.Input.GestureEventArgs e)
       {
           Rectangle chosenColor = sender as Rectangle;
           selectedEventColor.Background = chosenColor.Fill;
       }

       void AllDayEventUnChecked(object sender, RoutedEventArgs e)
       {
           startDatePicker.SetValue(Grid.RowSpanProperty, 1);
           endDatePicker.SetValue(Grid.RowSpanProperty, 1);
           startTimePicker.Visibility = endTimePicker.Visibility = Visibility.Visible;
       }
      
       void AllDayEventChecked(object sender, RoutedEventArgs e)
       {
           startDatePicker.SetValue(Grid.RowSpanProperty, 2);
           endDatePicker.SetValue(Grid.RowSpanProperty, 2);
           startTimePicker.Visibility = endTimePicker.Visibility = Visibility.Collapsed;
       }
     
       void CancelButtonClick(object sender, RoutedEventArgs e)
       {
           (this.Parent as Popup).IsOpen = false;
       }
      
       void SaveButtonClick(object sender, RoutedEventArgs e)
       {
           if (EventTitle != ApplicationResources.NewEventTitle)
           {
               EventItem ev = new EventItem()
               {
                   EventTitle = EventTitle,
                   EventLocation = EventLocation,
               //    EventStart = EventStart,
               //    EventEnd = EventEnd,
               //    EventColor = EventColor,
               };

               (this.Parent as Popup).IsOpen = false;
           }
           else
               tbName.BorderBrush = new SolidColorBrush(Colors.Red);
       }
       

    }
}
