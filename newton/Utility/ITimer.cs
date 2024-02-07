using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace newton.Utility
{
    public interface ITimer
    {
        void Start();
        void Stop();
        double Interval { get; set; }
        event ElapsedEventHandler Elapsed;
    }
}
