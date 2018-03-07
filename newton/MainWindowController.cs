using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace newton
{
    public class MainWindowController
    {
        public void Initialize(Configuration theConfiguration)
        {
            myConfig = theConfiguration;
            ViewModel.SandBoxSize = theConfiguration.SandboxSize_px;
            ViewModel.GravitationalConstant = theConfiguration.GravitationConstant;
            ViewModel.ApplyConstant = new CommandHandler(p => myConfig.GravitationConstant = ViewModel.GravitationalConstant, p => true);
            ViewModel.Start = new CommandHandler(p => startSimulation(), p => !m_IsRunning);
            ViewModel.Stop = new CommandHandler(p => stopSimulation(), p => m_IsRunning);
            ViewModel.Reset = new CommandHandler(p => initPlanets(), p => true);
            initPlanets();
            initTimer();
        }

        private void startSimulation()
        {
            m_IsRunning = true;
            m_Timer.Start();
            ViewModel.Start.RaiseCanExecuteChanged();
            ViewModel.Stop.RaiseCanExecuteChanged();
        }

        private void stopSimulation()
        {
            m_IsRunning = false;
            m_Timer.Stop();
            ViewModel.Start.RaiseCanExecuteChanged();
            ViewModel.Stop.RaiseCanExecuteChanged();
        }

        private bool m_IsRunning = false;

        Configuration myConfig;
        private void initPlanets()
        {
            var aPlanets = new List<Planet>();

            aPlanets.Add(new Planet(80, new Point(100, 100), new Point(0, 0), "Blue"));
            aPlanets.Add(new Planet(30, new Point(myConfig.SandboxSize_px - 30, 0), new Point(0, 30), "Green"));
            aPlanets.Add(new Planet(30, new Point(0, myConfig.SandboxSize_px - 30), new Point(0, -30), "Magenta"));
            aPlanets.Add(new Planet(20, new Point(0, 0), new Point(20, 5), "Olive"));
            aPlanets.Add(new Planet(10, new Point(myConfig.SandboxSize_px - 10, myConfig.SandboxSize_px - 10), new Point(-10, 0), "Red"));

            ViewModel.Planets = new ObservableCollection<Planet>(aPlanets);
        }

        private void initTimer()
        {
            m_Timer = new Timer(myConfig.TimeStep_ms);
            m_Timer.Elapsed += M_Timer_Elapsed;
        }

        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var aPlanet in ViewModel.Planets)
            {
                foreach (var aOtherPlanet in ViewModel.Planets)
                {
                    if (aOtherPlanet != aPlanet)
                    {
                        updateAcceleration(aPlanet, aOtherPlanet);
                    }
                }
            }

            foreach (var aPlanet in ViewModel.Planets)
            {
                applyAcceleration(aPlanet);
            }
        }

        private void applyAcceleration(Planet thePlanetToMove)
        {
            var aAccWithMass = new Point(thePlanetToMove.Acceleration.X / thePlanetToMove.Mass, thePlanetToMove.Acceleration.Y / thePlanetToMove.Mass);
            thePlanetToMove.Location = addPoints(thePlanetToMove.Location, aAccWithMass);
        }

        private void updateAcceleration(Planet thePlanetToUpdate, Planet theOtherPlanet)
        {
            var aDiff = getDiffVector(thePlanetToUpdate.Location, theOtherPlanet.Location);
            var aLength = getLength(thePlanetToUpdate.Location, theOtherPlanet.Location);
            var aNorm = (aLength * aLength * aLength);
            var aNewAcc = new Point(myConfig.GravitationConstant * thePlanetToUpdate.Mass * theOtherPlanet.Mass * aDiff.X / aNorm,
                myConfig.GravitationConstant * thePlanetToUpdate.Mass * theOtherPlanet.Mass * aDiff.Y / aNorm);

            thePlanetToUpdate.Acceleration = addPoints(thePlanetToUpdate.Acceleration, aNewAcc);
        }

        private Point addPoints(Point theFirstPoint, Point theSecondPoint)
        {
            return new Point(theFirstPoint.X + theSecondPoint.X, theFirstPoint.Y + theSecondPoint.Y);
        }

        private Point subPoints(Point theFirstPoint, Point theSecondPoint)
        {
            return new Point(theFirstPoint.X - theSecondPoint.X, theFirstPoint.Y - theSecondPoint.Y);
        }

        private Point getDiffVector(Point theFirstPoint, Point theSecondPoint)
        {
            var aDeltaX = theFirstPoint.X - theSecondPoint.X;
            var aDeltaY = theFirstPoint.Y - theSecondPoint.Y;
            return new Point(aDeltaX, aDeltaY);
        }

        private double getLength(Point theFirstPoint, Point theSecondPoint)
        {
            var aDiff = getDiffVector(theFirstPoint, theSecondPoint);
            return Math.Sqrt(aDiff.X * aDiff.X + aDiff.Y * aDiff.Y);
        }

        private Timer m_Timer;

        internal MainWindowViewModel ViewModel
        {
            get;
            set;
        }
    }
}
