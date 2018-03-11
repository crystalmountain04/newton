using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newton
{
    public class Configuration
    {
        public Configuration()
        {
        }

        public Configuration(double theTimeStep_ms, int theSandBoxSize_px, double theGravitationConstant, double theCollisionThreshold)
        {
            TimeStep_ms = theTimeStep_ms;
            SandboxSize_px = theSandBoxSize_px;
            GravitationConstant = theGravitationConstant;
            CollisionThreshold = theCollisionThreshold;
        }

        public double CollisionThreshold
        {
            get;
            set;
        }

        public double TimeStep_ms
        {
            get;
            set;
        }

        public int SandboxSize_px
        {
            get;
            set;
        }

        public double GravitationConstant
        {
            get;
            set;
        }
    }
}
