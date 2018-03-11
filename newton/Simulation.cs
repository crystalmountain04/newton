using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace newton
{
    public class Simulation
    {
        public void Initialize(Universe theUniverseToSimulate)
        {
            if (null != myCalculateTimer)
            {
                myCalculateTimer.Elapsed -= MyCalculateTimer_Elapsed;
                myCalculateTimer = null;
            }
            myCalculateTimer = new Timer(theUniverseToSimulate.Configuration.TimeStep_ms);
            myCalculateTimer.Elapsed += MyCalculateTimer_Elapsed;

            Universe = theUniverseToSimulate;
        }

        public Universe Universe
        {
            get;
            private set;
        }

        public void StartSimulation()
        {
            myCalculateTimer.Start();
        }

        public void StopSimulation()
        {
            myCalculateTimer.Stop();
        }

        private void MyCalculateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var aPlanet in Universe.Planets)
            {
                foreach (var aOtherPlanet in Universe.Planets)
                {
                    if (aOtherPlanet != aPlanet)
                    {
                        updateAcceleration(aPlanet, aOtherPlanet);
                    }
                }
            }

            foreach (var aPlanet in Universe.Planets)
            {
                applyAcceleration(aPlanet);
            }
        }

        private void applyAcceleration(Planet thePlanetToMove)
        {
            var aAccWithMass = new Point(thePlanetToMove.Acceleration.X / thePlanetToMove.Mass, thePlanetToMove.Acceleration.Y / thePlanetToMove.Mass);
            thePlanetToMove.Location = MathHelper.AddPoints(thePlanetToMove.Location, aAccWithMass);
        }

        private void updateAcceleration(Planet thePlanetToUpdate, Planet theOtherPlanet)
        {
            var aDiff = MathHelper.GetDiffVector(thePlanetToUpdate.Location, theOtherPlanet.Location);
            var aDistance = MathHelper.GetDistance(thePlanetToUpdate.Location, theOtherPlanet.Location);
            var aNorm = (aDistance * aDistance * aDistance);
            var aNewAcc = new Point(Universe.Configuration.GravitationConstant * thePlanetToUpdate.Mass * theOtherPlanet.Mass * aDiff.X / aNorm,
                Universe.Configuration.GravitationConstant * thePlanetToUpdate.Mass * theOtherPlanet.Mass * aDiff.Y / aNorm);

            if (aDistance > Universe.Configuration.CollisionThreshold)
            {
                thePlanetToUpdate.Acceleration = MathHelper.AddPoints(thePlanetToUpdate.Acceleration, aNewAcc);
            }
            else
            {
                // nothing
            }
        }

        private Timer myCalculateTimer;
    }
}
