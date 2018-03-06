using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace newton
{
    public class MainWindowController
    {
        public void Initialize()
        {
            initPlanets();
            launchTimer();
        }

        Planet myPlanet1;
        Planet myPlanet2;

        private void initPlanets()
        {
            myPlanet1 = new Planet();
            myPlanet1.Location = new System.Windows.Point(0, 0);

            myPlanet2 = new Planet();
            myPlanet2.Location = new System.Windows.Point(250, 250);

            ViewModel.Planet1 = myPlanet1;
            ViewModel.Planet2 = myPlanet2;
        }

        private void launchTimer()
        {
            m_Timer = new Timer(500);
            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
        }

        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            moveRelativ(myPlanet1, 1, 1);
            moveRelativ(myPlanet2, -1, -1);
        }

        private void moveRelativ(Planet thePlanetToMove, double theDeltaX, double theDeltaY)
        {
            thePlanetToMove.Location = new System.Windows.Point(thePlanetToMove.Location.X + theDeltaX, thePlanetToMove.Location.Y + theDeltaY);
        }

        private Timer m_Timer;

        internal MainWindowViewModel ViewModel
        {
            get;
            set;
        }
    }
}
