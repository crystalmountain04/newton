﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newton.Simulation
{
    public class SimulationSzenario
    {
        public SimulationSzenario()
        {
        }

        public SimulationSzenario(double theTimeStep_ms, int theSandBoxSize_px, double theGravitationConstant, double theCollisionThreshold, double theEventHorizon)
        {
            TimeStep_ms = theTimeStep_ms;
            SandboxSize_px = theSandBoxSize_px;
            GravitationConstant = theGravitationConstant;
            CollisionThreshold = theCollisionThreshold;
            EventHorizon = theEventHorizon;
        }

        public double EventHorizon
        {
            get;
            set;
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
