using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace newton
{
    public class MainWindowController
    {
        public void Initialize(Configuration theConfiguration)
        {
            myUniverse = new Universe();
            myUniverse.Configuration = theConfiguration;
            ViewModel.SandBoxSize = theConfiguration.SandboxSize_px;
            ViewModel.GravitationalConstant = theConfiguration.GravitationConstant;
            ViewModel.ApplyConstant = new CommandHandler(p => myUniverse.Configuration.GravitationConstant = ViewModel.GravitationalConstant, p => true);
            ViewModel.Start = new CommandHandler(p => startSimulation(), p => !m_IsRunning);
            ViewModel.Stop = new CommandHandler(p => stopSimulation(), p => m_IsRunning);
            ViewModel.Reset = new CommandHandler(p => initPlanets(), p => true);
            ViewModel.Save = new CommandHandler(p => saveUniverse(), p => true);
            ViewModel.Load = new CommandHandler(p => loadUniverse(), p => true);
            initPlanets();
            initTimer();
        }

        private void loadUniverse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var aUniverse = new Universe(openFileDialog.FileName);
                if(null != aUniverse)
                {
                    myUniverse.Planets = aUniverse.Planets;
                    myUniverse.Configuration = aUniverse.Configuration;
                    ViewModel.SandBoxSize = myUniverse.Configuration.SandboxSize_px;
                    ViewModel.GravitationalConstant = myUniverse.Configuration.GravitationConstant;
                }
            }
        }

        private Universe myUniverse;

        private void startSimulation()
        {
            m_IsRunning = true;
            myCalculateTimer.Start();
            myRenderTimer.Start();
            ViewModel.Start.RaiseCanExecuteChanged();
            ViewModel.Stop.RaiseCanExecuteChanged();
        }

        private void saveUniverse()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                var aUniverse = new Universe(myUniverse.Planets.ToList(), myUniverse.Configuration);
                aUniverse.Save(saveFileDialog.FileName);
            }
        }

        private void stopSimulation()
        {
            m_IsRunning = false;
            myCalculateTimer.Stop();
            myRenderTimer.Stop();
            ViewModel.Start.RaiseCanExecuteChanged();
            ViewModel.Stop.RaiseCanExecuteChanged();
        }

        private bool m_IsRunning = false;

        private void initPlanets()
        {
            var aPlanets = new List<Planet>();

            aPlanets.Add(new Planet(50000, new Point(myUniverse.Configuration.SandboxSize_px *0.5, myUniverse.Configuration.SandboxSize_px * 0.5), new Point(0, 0), "Yellow"));
            aPlanets.Add(new Planet(30, new Point(myUniverse.Configuration.SandboxSize_px - 200, 100), new Point(20, 30), "Turquoise"));
            aPlanets.Add(new Planet(30, new Point(100, myUniverse.Configuration.SandboxSize_px - 220), new Point(0, -30), "Magenta"));
            aPlanets.Add(new Planet(20, new Point(105, 103), new Point(20, 5), "Green"));
            aPlanets.Add(new Planet(10, new Point(myUniverse.Configuration.SandboxSize_px - 123, myUniverse.Configuration.SandboxSize_px - 142), new Point(-10, 0), "Red"));

            //Random aRandom = new Random();
            //for (int i = 0; i < 1000; i++)
            //{
            //    var aLocX = aRandom.Next(0, myUniverse.Configuration.SandboxSize_px);
            //    var aLocY = aRandom.Next(0, myUniverse.Configuration.SandboxSize_px);
            //    var aMass = aRandom.Next(30, 50);
            //    var aAccX = aRandom.Next(-100, 100);
            //    var aAccY = aRandom.Next(-100, 100);
            //    aPlanets.Add(new Planet(aMass, new Point(aLocX, aLocY), new Point(aAccX, aAccY), "White"));
            //}

            myUniverse.Planets = aPlanets;
            displayUniverse(myUniverse);
        }

        private void initTimer()
        {
            myCalculateTimer = new Timer(myUniverse.Configuration.TimeStep_ms);
            myCalculateTimer.Elapsed += MyCalculateTimer_Elapsed;

            myRenderTimer = new DispatcherTimer();
            myRenderTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            myRenderTimer.Tick += MyRenderTimer_Tick;
        }

        private async void MyRenderTimer_Tick(object sender, EventArgs e)
        {
            await displayUniverse(myUniverse);
        }

        private Task displayUniverse(Universe theUniverseToDisplay)
        {
            return Task.Run(() =>
            {
                ViewModel.Planets = new ObservableCollection<Planet>(theUniverseToDisplay.Planets);
            });
        }

        private void MyCalculateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var aPlanet in myUniverse.Planets)
            {
                foreach (var aOtherPlanet in myUniverse.Planets)
                {
                    if (aOtherPlanet != aPlanet)
                    {
                        updateAcceleration(aPlanet, aOtherPlanet);
                    }
                }
            }

            foreach (var aPlanet in myUniverse.Planets)
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
            var aDistance = getDistance(thePlanetToUpdate.Location, theOtherPlanet.Location);
            var aNorm = (aDistance * aDistance * aDistance);
            var aNewAcc = new Point(myUniverse.Configuration.GravitationConstant * thePlanetToUpdate.Mass * theOtherPlanet.Mass * aDiff.X / aNorm,
                myUniverse.Configuration.GravitationConstant * thePlanetToUpdate.Mass * theOtherPlanet.Mass * aDiff.Y / aNorm);

            if (aDistance > myUniverse.Configuration.CollisionThreshold)
            {
                thePlanetToUpdate.Acceleration = addPoints(thePlanetToUpdate.Acceleration, aNewAcc);
            }
            else
            {
                // nothing
            }
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

        private double getDistance(Point theFirstPoint, Point theSecondPoint)
        {
            var aDiff = getDiffVector(theFirstPoint, theSecondPoint);
            return Math.Sqrt(aDiff.X * aDiff.X + aDiff.Y * aDiff.Y);
        }

        private Timer myCalculateTimer;
        private DispatcherTimer myRenderTimer;

        internal MainWindowViewModel ViewModel
        {
            get;
            set;
        }
    }
}
