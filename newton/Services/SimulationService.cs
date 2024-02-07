using newton.Simulation;
using newton.Utility;
using System.Diagnostics;
using System.Windows;

namespace newton.Services
{
    public class SimulationService : ISimulationService
    {
        public SimulationService(Utility.ITimer theTimer)
        {
            myCalculateTimer = theTimer;
        }

        public void Initialize(Universe theUniverseToSimulate)
        {
            myCalculateTimer.Stop();
            IsRunning = false;
            myCalculateTimer.Interval = theUniverseToSimulate.Configuration.TimeStep_ms;
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
            myCalculateTimer.Elapsed += MyCalculateTimer_Elapsed;
            myCalculateTimer.Start();
        }

        public void StopSimulation()
        {
            if(!IsRunning)
            {
                return;
            }
            IsRunning = false;
            myCalculateTimer.Stop();
            myCalculateTimer.Elapsed -= MyCalculateTimer_Elapsed;
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

        private static SemaphoreSlim myCalculationSemaphore = new SemaphoreSlim(1);

        private async void MyCalculateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                await myCalculationSemaphore.WaitAsync().ConfigureAwait(false);
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
                myCalculationSemaphore.Release();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
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

        private Utility.ITimer myCalculateTimer;
    }
}
