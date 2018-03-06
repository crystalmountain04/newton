using System;
using System.Collections.Generic;
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
            initPlanets();
            launchTimer();
        }

        Configuration myConfig;
        Planet myPlanet1;
        Planet myPlanet2;

        private void initPlanets()
        {
            myPlanet1 = new Planet();
            myPlanet1.Mass = 80;
            myPlanet1.Location = new System.Windows.Point(100, 100);
            myPlanet1.Acceleration = new System.Windows.Point(0, 0);

            myPlanet2 = new Planet();
            myPlanet2.Mass = 10;
            myPlanet2.Location = new System.Windows.Point(myConfig.SandboxSize_px - myPlanet2.Mass, myConfig.SandboxSize_px - myPlanet2.Mass);
            myPlanet2.Acceleration = new System.Windows.Point(-10, 0);

            ViewModel.Planet1 = myPlanet1;
            ViewModel.Planet2 = myPlanet2;
        }

        private void launchTimer()
        {
            m_Timer = new Timer(myConfig.TimeStep_ms);
            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
        }

        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            updateAcceleration(myPlanet1, myPlanet2);
            updateAcceleration(myPlanet2, myPlanet1);

            applyAcceleration(myPlanet1);
            applyAcceleration(myPlanet2);
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
