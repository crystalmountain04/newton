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
        public MainWindowController()
        {
            m_Timer = new Timer(500);
            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
        }

        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ViewModel.P1Start = new System.Windows.Point(ViewModel.P1Start.X + 1, ViewModel.P1Start.Y + 1);
            ViewModel.P2Start = new System.Windows.Point(ViewModel.P2Start.X + 1, ViewModel.P2Start.Y + 1);
        }

        private Timer m_Timer;

        internal MainWindowViewModel ViewModel
        {
            get;
            set;
        }
    }
}
