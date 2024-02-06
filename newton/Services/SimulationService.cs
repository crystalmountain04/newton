﻿using newton.Simulation;
using newton.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace newton.Services
{
    public class SimulationService : ISimulationService
    {
        public void Initialize(Universe theUniverseToSimulate)
        {
            IsRunning = false;
            if (null != myCalculateTimer)
            {
                myCalculateTimer.Elapsed -= MyCalculateTimer_Elapsed;
                myCalculateTimer = null;
            }
            myCalculateTimer = new System.Timers.Timer(theUniverseToSimulate.Configuration.TimeStep_ms);
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
            IsRunning = true;
            myCalculateTimer.Start();
        }

        public void StopSimulation()
        {
            IsRunning = false;
            myCalculateTimer.Stop();
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public void ApplyGravitationConstant(double theConstant)
        {
            Universe.Configuration.GravitationConstant = theConstant;
        }

        private static object myLock = new object();

        private void MyCalculateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (myLock)
            {
                var aPlanetsToRemove = new List<Planet>();
                foreach (var aPlanet in Universe.Planets)
                {
                    if (aPlanet.IsDoomed)
                    {
                        aPlanetsToRemove.Add(aPlanet);
                        continue;
                    }

                    calculateInteractions(aPlanet);
                }

                foreach (var aPlanet in aPlanetsToRemove)
                {
                    Universe.Planets.Remove(aPlanet);
                }

                foreach (var aPlanet in Universe.Planets)
                {
                    if (!aPlanet.IsDoomed)
                    {
                        applyAcceleration(aPlanet);
                    }
                }
            }
        }

        private void calculateInteractions(Planet thePlanet)
        {
            foreach (var aOtherPlanet in Universe.Planets)
            {
                if (aOtherPlanet.IsDoomed || aOtherPlanet == thePlanet)
                {
                    continue;
                }

                updateAcceleration(thePlanet, aOtherPlanet);
            }
        }

        private void applyAcceleration(Planet thePlanetToMove)
        {
            var aAccWithMass = new Point(thePlanetToMove.Acceleration.X / thePlanetToMove.Mass, thePlanetToMove.Acceleration.Y / thePlanetToMove.Mass);
            thePlanetToMove.Location = MathHelper.AddPoints(thePlanetToMove.Location, aAccWithMass);
            thePlanetToMove.IsDoomed = hasPlanetCrossedEventHorizon(thePlanetToMove.Location);
        }

        private bool hasPlanetCrossedEventHorizon(Point thePlanetLocation)
        {
            return thePlanetLocation.X > Universe.Configuration.EventHorizon ||
                    thePlanetLocation.X < -Universe.Configuration.EventHorizon ||
                    thePlanetLocation.Y > Universe.Configuration.EventHorizon ||
                    thePlanetLocation.Y < -Universe.Configuration.EventHorizon;
        }

        private void updateAcceleration(Planet thePlanetToUpdate, Planet theOtherPlanet)
        {
            var aDiff = MathHelper.GetDiffVector(thePlanetToUpdate.Location, theOtherPlanet.Location);
            var aDistance = MathHelper.GetDistance(thePlanetToUpdate.Location, theOtherPlanet.Location);
            var aNorm = aDistance * aDistance * aDistance;
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

        private System.Timers.Timer myCalculateTimer;
    }
}
