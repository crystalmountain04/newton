using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newton
{
    public class Configuration
    {
        public Configuration(int theTimeStep_ms, int theSandBoxSize_px)
        {
            TimeStep_ms = theTimeStep_ms;
            SandboxSize_px = theSandBoxSize_px;
        }

        public int TimeStep_ms
        {
            get;
            private set;
        }

        public int SandboxSize_px
        {
            get;
            private set;
        }
    }
}
