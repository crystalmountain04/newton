using newton.Simulation;
using newton.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace newton
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int mySandBoxSize;
        public int SandBoxSize
        {
            get { return mySandBoxSize; }
            set { mySandBoxSize = value; RaisePropertyChanged(); }
        }

        private List<Planet> myPlanets;
        public List<Planet> Planets
        {
            get { return myPlanets; }
            set { myPlanets = value; RaisePropertyChanged(); }
        }

        private double myGravitationalConstant;
        public double GravitationalConstant
        {
            get { return myGravitationalConstant; }
            set { myGravitationalConstant = value; RaisePropertyChanged(); }
        }

        public bool IsSimulationRunning
        {
            get;
            set;
        }

        public CommandHandler SyncToSimulation
        {
            get;
            set;
        }

        public CommandHandler ApplyConstant
        {
            get;
            set;
        }

        public CommandHandler Start
        {
            get;
            set;
        }

        public CommandHandler Stop
        {
            get;
            set;
        }

        public CommandHandler Reset
        {
            get;
            set;
        }

        public CommandHandler Save
        {
            get;
            set;
        }

        public CommandHandler Load
        {
            get;
            set;
        }
    }
}
