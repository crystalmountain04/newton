using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace newtonTests.Helper
{
    public class TestTimer : newton.Utility.ITimer
    {
        public double Interval { get; set; }

        public event ElapsedEventHandler Elapsed;

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public bool IsRunning { get; set; }

        public void RaiseElapsed( ElapsedEventArgs theEventArgsToSend )
        {
            Elapsed?.Invoke(this, theEventArgsToSend);
        }
    }
}
