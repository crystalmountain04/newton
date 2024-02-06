using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public class MainWindowViewModel : ObservableObject
    {
        private int mySandBoxSize;
        public int SandBoxSize
        {
            get => mySandBoxSize;
            set => SetProperty(ref mySandBoxSize, value);
        }

        private List<Planet> myPlanets = new List<Planet>();
        public List<Planet> Planets
        {
            get => myPlanets;
            set => SetProperty(ref myPlanets, value);
        }

        private double myGravitationalConstant;
        public double GravitationalConstant
        {
            get => myGravitationalConstant;
            set => SetProperty(ref myGravitationalConstant, value);
        }

        public bool IsSimulationRunning
        {
            get;
            set;
        }

        public RelayCommand SyncToSimulation
        {
            get;
            set;
        }

        public RelayCommand ApplyConstant
        {
            get;
            set;
        }

        public RelayCommand Start
        {
            get;
            set;
        }

        public RelayCommand Stop
        {
            get;
            set;
        }

        public RelayCommand Reset
        {
            get;
            set;
        }

        public RelayCommand Save
        {
            get;
            set;
        }

        public RelayCommand Load
        {
            get;
            set;
        }
    }
}
