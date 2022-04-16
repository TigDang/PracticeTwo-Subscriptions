using System;
using System.Threading;

namespace PracticeTwo
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("----------------------");
            int ms;
            Console.WriteLine("Enter delay in milliseconds:");

            //Initialize class with event that subscribers needs to know
            var Timer = new CountDown();

            //Initialize subscribers
            var firstSubscriber = new Subscriber(new CountDown[] { Timer });
            var secondSubscriber = new Subscriber(new CountDown[] { Timer });

            //Trying parsing inputs until success
            while (!int.TryParse(Console.ReadLine(), out ms))
            {}

            Timer.Timer(ms, $"User event after {ms}");

            //Raising event in Timer class
            Timer.RaiseCountDownEvent();

            Console.ReadKey();
            
            /*
            CountDown ctnDown = new CountDown();
            ctnDown.SampleEvent += CtnDownOnSampleEvent;

            ctnDown.RaiseSampleEvent();
            Console.ReadKey();
            */
        }
    }

    public class CountDown
    {
        public delegate void CountDownDelegate(object sender, string data);
        // Declare the event.
        public event CountDownDelegate CountDownEvent;

        public void Timer(int ms, string massage)
        {
            //Simulating threading before starts notifying
            Console.WriteLine(massage);
            Thread.Sleep(ms);
        }

        public virtual void RaiseCountDownEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            CountDownEvent?.Invoke(this, CountDownEvent.ToString() + " event is happened");
        }
    }

    public class Subscriber
    {
        //Contructor needs CountDown parameter to subscribe to him.
        public Subscriber(CountDown[] cds)
        {
            //Adding event handlers for each of classes
            foreach (var cd in cds)
            {
                cd.CountDownEvent += (sender, args) => { Display(args); };
            }
        }


        //Implement a logic of notifying. In that example - simply writeline in console.
        private void Display(string eventText)
        {
            if (eventText == null)
            {
                throw new ArgumentNullException("message argument is null");
            }

            Console.WriteLine("Subscriber({0}): Message", eventText);
        }
    }
}
