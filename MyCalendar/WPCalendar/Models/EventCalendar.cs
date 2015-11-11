using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WPCalendar;
using WPCalendar.Helpers;

namespace WPCalendar.Models
{
    [DataContract]
    public class EventCalendar : INotifyPropertyChanged
    {

        #region Serializable
        [DataMember]
        public List<EventItem> AllEvents
        {
            get
            {
                return allEvents;
            }
            set
            {
                if (value != null)
                    allEvents = value;
                else
                    allEvents = new List<EventItem>();
              
                OnPropertyChanged("AllEvents");
            }
        }


        #endregion

        #region private
        private EventItem currentEvent;
        private List<EventItem> allEvents = new List<EventItem>();
        private List<EventItem> futureEvents;
        private List<EventItem> pastEvents;
        #endregion

        #region const

        private const int defaultAveragePeriod = 6;
        private const int defaultAverageCycle = 28;

        #endregion

        #region Properties
        public EventItem CurrentEvent
        {
            get
            {
                List<EventItem> events = new List<EventItem>();
                if (currentEvent != null) return currentEvent;

                currentEvent = new EventItem();
                if (AllEvents != null && AllEvents.Count > 0)
                {
                    events = AllEvents.OrderByDescending(x => x.EventStart).ToList();
                    currentEvent = events.Where(x => x.EventStart <= DateTime.Today && x.EventEnd >= DateTime.Today).FirstOrDefault();
                }

                return currentEvent;
            }
            set
            {
                if (value != null)
                {
                    currentEvent = value;
                    OnPropertyChanged("CurrentEvent");
                }
            }
        }

        public List<EventItem> FutureEvents
        {
            get
            {
                if (futureEvents == null || futureEvents.Count == 0)
                {
                //    futureEvents = EventsAfter(DateTime.Today);
                }
                return futureEvents;
            }
            private set
            {
                if (futureEvents == null)
                    futureEvents = new List<EventItem>();
                if (futureEvents != value)
                    futureEvents = value;
            }
        }

        public List<EventItem> PastEvents
        {
            get
            {
                if (pastEvents == null || pastEvents.Count == 0)
                {
                //    pastEvents = EventsBefore(DateTime.Today);
                }
                return pastEvents;
            }
            private set
            {
                if (pastEvents == null)
                    pastEvents = new List<EventItem>();
                if (pastEvents != value)
                    pastEvents = value;
            }
        }

        #endregion

        #region  Private Methods
      /*  private List<EventItem> EventsAfter(DateTime dateTime)
        {
            List<EventItem> future = new List<EventItem>();
            List<EventItem> allEvents = new List<EventItem>();

            if (AllEvents != null)
            {
                allEvents = AllEvents.OrderByDescending(x => x.EventStart).ToList();
            }

            future = allEvents.Where(x => x.EventStart > dateTime).ToList();
            return future;
        }

        private List<EventItem> EventsBefore(DateTime dateTime)
        {
            List<EventItem> past = new List<EventItem>();
            List<EventItem> events = new List<EventItem>();

            if (AllEvents != null)
                events = AllEvents.OrderByDescending(x => x.EventStart).ToList();

            past = events.Where(x => x.EventStart < dateTime).ToList();
            return past;
        }
       * 
       * */
        #endregion

        #region event handlers

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

       


        #endregion


        public EventCalendar()
        {
        }
     

       
    }
}
