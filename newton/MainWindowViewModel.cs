using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
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
using System.Windows.Threading;

namespace newton
{
    public class MainWindowViewModel : ObservableObject
    {
        private DispatcherTimer myRenderTimer;

        public MainWindowViewModel()
        {
            mySimulation = new SimulationEngine();
            ApplyConstant = new RelayCommand(() => mySimulation?.ApplyGravitationConstant(GravitationalConstant), () => true);
            Start = new RelayCommand(() => startSimulation(), () => mySimulation != null && !mySimulation.IsRunning);
            Stop = new RelayCommand(() => stopSimulation(), () => mySimulation != null && mySimulation.IsRunning);
            Reset = new RelayCommand(() => resetSimulation(), () => true);
            Save = new RelayCommand(() => saveUniverseToFile(), () => true);
            Load = new RelayCommand(() => openUniverseFromFile(), () => true);
            SyncToSimulation = new RelayCommand(() => syncToSimulation(), () => true);

            myRenderTimer = new DispatcherTimer();
            myRenderTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            myRenderTimer.Tick += MyRenderTimer_Tick;
        }

        private SimulationEngine mySimulation;

        public void Initialize(SimulationSzenario theConfiguration)
        {
            mySimulation.Initialize(UniverseFactory.CreateUniverse(theConfiguration));

            configureVisualization(theConfiguration);
            displayUniverse(mySimulation.Universe);
        }

        private void resetSimulation()
        {
            mySimulation.Initialize(UniverseFactory.CreateUniverse(mySimulation.Universe.Configuration));
            updateCommandAvailability();
            displayUniverse(mySimulation.Universe);
        }

        private void openUniverseFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var aUniverse = new Universe(openFileDialog.FileName);
                if (null != aUniverse)
                {
                    mySimulation.Initialize(aUniverse);
                    updateCommandAvailability();
                    configureVisualization(aUniverse.Configuration);
                    displayUniverse(mySimulation.Universe);
                }
            }
        }

        private void saveUniverseToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                mySimulation.Universe.Save(saveFileDialog.FileName);
            }
        }

        private void syncToSimulation()
        {
            mySimulation.Universe.Planets = Helper.DeepCopy(Planets);
        }

        private void configureVisualization(SimulationSzenario theConfiguration)
        {
            SandBoxSize = theConfiguration.SandboxSize_px;
            GravitationalConstant = theConfiguration.GravitationConstant;
        }

        private void startSimulation()
        {
            mySimulation.StartSimulation();
            myRenderTimer.Start();
            updateCommandAvailability();
            IsSimulationRunning = true;
        }

        private void stopSimulation()
        {
            mySimulation.StopSimulation();
            myRenderTimer.Stop();
            updateCommandAvailability();
            IsSimulationRunning = false;
        }

        private void updateCommandAvailability()
        {
            Start.NotifyCanExecuteChanged();
            Stop.NotifyCanExecuteChanged();
        }

        private void MyRenderTimer_Tick(object sender, EventArgs e)
        {
            displayUniverse(mySimulation.Universe);
        }

        private void displayUniverse(Universe theUniverseToDisplay)
        {
            Planets = theUniverseToDisplay.Planets;
        }

        private int mySandBoxSize;
        public int SandBoxSize
        {
            get => mySandBoxSize;
            set => SetProperty(ref mySandBoxSize, value);
        }

        private List<Planet> myPlanets = [];
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

        public bool IsSimulationRunning { get; set; }

        public RelayCommand SyncToSimulation { get; set; }

        public RelayCommand ApplyConstant { get; set; }

        public RelayCommand Start { get; set; }

        public RelayCommand Stop { get; set; }

        public RelayCommand Reset { get; set; }

        public RelayCommand Save { get; set; }

        public RelayCommand Load { get; set; }
    }
}
