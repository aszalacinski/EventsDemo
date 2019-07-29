using System;

namespace EventsDemo
{

    // https://www.akadia.com/services/dotnet_delegates_and_events.html

    /* ========= Subscriber of the Event ============== */
    // It's now easier and cleaner to merely add instances
    // of the delegate to the event, instead of having to
    // manage things ourselves
    public class Program
    {
        static void Logger(string s)
        {
            Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
            MyClass myClass = new MyClass();

            // Subscribe the Functions Logger 
            myClass.Log += new MyClass.LogHandler(Logger);

            // The Event will now be triggered in the Process() Method
            myClass.Process();
            Console.ReadKey();
        }
    }

    /* ==================== Publisher of the Event ===================== */
    public class MyClass
    {
        // Define a delegate named LogHandler, which will encapsulate
        // any method that takes a string as the parameter and returns no value
        public delegate void LogHandler(string message);

        // Define an Event based on the above Delegate
        public event LogHandler Log;

        // Instead of having the Process() function take a delegate
        // as a parameter, we've declared a Log event. Call the Event,
        // using the OnXXXX Method, where XXXX is the name of the Event.
        public void Process()
        {
            OnLog("Process() begin");
            OnLog("Process() end");
        }

        // By Default, create an OnXXXX Method, to call the Event
        protected void OnLog(string message)
        {
            if(Log != null)
            {
                Log(message);
            }
        }
    }
}
