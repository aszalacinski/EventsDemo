﻿using System;
using System.Threading;

namespace EventsDemo2
{
    // https://www.akadia.com/services/dotnet_delegates_and_events.html

    /* ======================= Event Publisher =============================== */

    // Our subject -- it is this class that other classes
    // will observe. This class publishes one event:
    // SecondChange. The observers subscribe to that event.
    public class Clock
    {
        // Private fields holding the hour, minute and second
        private int _hour;
        private int _minute;
        private int _second;

        // The delegate named SecondChangeHandler, which will encapsulate
        // any method that takes a clock object and a TimeInfoEventArgs
        // object as the parameter and returns no value. It's the
        // delegate the subscribers must implement.
        public delegate void SecondChangeHandler(object clock, TimeInfoEventArgs timeInformation);

        // The event we publish
        public event SecondChangeHandler SecondChange;

        // The method which fires the Event
        protected void OnSecondChange(object clock, TimeInfoEventArgs timeInformation)
        {
            // check if there are any subscribers
            if(SecondChange != null)
            {
                // call the event
                SecondChange(clock, timeInformation);
            }
        }

        // Set the clock running, it will raise an
        // event for each new second
        public void Run()
        {
            for(; ;)
            {
                // Sleep 1 second
                Thread.Sleep(1000);

                // Get the current time
                System.DateTime dt = System.DateTime.Now;

                // if the second has changed
                // notify the subscribers
                if(dt.Second != _second)
                {
                    // create the TimeInfoEventArgs object
                    // to pass to the subscribers
                    TimeInfoEventArgs timeInformation = new TimeInfoEventArgs(dt.Hour, dt.Minute, dt.Second);

                    // If anyone has subscribed, notify them
                    OnSecondChange(this, timeInformation);
                }

                // update the state
                this._second = dt.Second;
                this._minute = dt.Minute;
                this._hour = dt.Hour;
            }
        }
    }

    // The class to hold the information about the event
    // in this case it will hold only information
    // available int he clock class, but could hold
    // additional state information
    public class TimeInfoEventArgs : EventArgs
    {
        public TimeInfoEventArgs(int hour, int minute, int second)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public readonly int hour;
        public readonly int minute;
        public readonly int second;
    }

    /* ======================= Event Subscribers =============================== */

    // an observer. DisplayClock subscribes to the
    // clock's events. The job of DisplayClock is
    // to display the current time
    public class DisplayClock
    {
        // given the clock, to subscribe to
        // its SecondChangeHandler event
        public void Subscribe(Clock theClock)
        {
            theClock.SecondChange += new Clock.SecondChangeHandler(TimeHasChanged);
        }

        // the method that implements the delegated functionality
        public void TimeHasChanged(object theClock, TimeInfoEventArgs ti)
        {
            Console.WriteLine($"Current Time: {ti.hour.ToString()}:{ti.minute.ToString()}:{ti.second.ToString()}");
        }
    }

    // A second subscriber whose job is to write to a file
    public class LogClock
    {
        public void Subscribe(Clock theClock)
        {
            theClock.SecondChange += new Clock.SecondChangeHandler(WriteLogEntry);
        }

        // This method should write to a file
        // we write to the console to see the effect
        // this object keeps no state
        public void WriteLogEntry(object theClock, TimeInfoEventArgs ti)
        {
            Console.WriteLine($"Logging to file: {ti.hour.ToString()}:{ti.minute.ToString()}:{ti.second.ToString()}");
        }
    }

    /* ======================= Test Application =============================== */
    // Test Application which implements the
    // Clock Notifier - Subscriber sample
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new Clock
            Clock theClock = new Clock();

            // Create the display and tell it to
            // subscribe to the clock just created
            DisplayClock dc = new DisplayClock();
            dc.Subscribe(theClock);

            // Create a Log object and tell it
            // to subscribe to the clock
            LogClock lc = new LogClock();
            lc.Subscribe(theClock);

            // Get the clock started
            theClock.Run();
        }
    }

}
