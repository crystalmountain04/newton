using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using newton.Simulation;
using newton.Utility;
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
        private SimulationEngine mySimulation;

        public MainWindowController()
        {
            myRenderTimer = new DispatcherTimer();
            myRenderTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            myRenderTimer.Tick += MyRenderTimer_Tick;
        }

        public void Initialize(Configuration theConfiguration)
        {
            initCommands();

            mySimulation = new SimulationEngine();
            mySimulation.Initialize(UniverseFactory.CreateUniverse(theConfiguration));

            configureVisualization(theConfiguration);
            displayUniverse(mySimulation.Universe);
        }

        private void initCommands()
        {
            ViewModel.ApplyConstant = new RelayCommand(() => mySimulation.ApplyGravitationConstant(ViewModel.GravitationalConstant), () => true);
            ViewModel.Start = new RelayCommand(() => startSimulation(), () => !mySimulation.IsRunning);
            ViewModel.Stop = new RelayCommand(() => stopSimulation(), () => mySimulation.IsRunning);
            ViewModel.Reset = new RelayCommand(() => resetSimulation(), () => true);
            ViewModel.Save = new RelayCommand(() => saveUniverseToFile(), () => true);
            ViewModel.Load = new RelayCommand(() => openUniverseFromFile(), () => true);
            ViewModel.SyncToSimulation = new RelayCommand(() => syncToSimulation(), () => true);
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
                if(null != aUniverse)
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
            mySimulation.Universe.Planets = Helper.DeepCopy(ViewModel.Planets);
        }

        private void configureVisualization(Configuration theConfiguration)
        {
            ViewModel.SandBoxSize = theConfiguration.SandboxSize_px;
            ViewModel.GravitationalConstant = theConfiguration.GravitationConstant;
        }

        private void startSimulation()
        {
            mySimulation.StartSimulation();
            myRenderTimer.Start();
            updateCommandAvailability();
            ViewModel.IsSimulationRunning = true;
        }

        private void stopSimulation()
        {
            mySimulation.StopSimulation();
            myRenderTimer.Stop();
            updateCommandAvailability();
            ViewModel.IsSimulationRunning = false;
        }

        private void updateCommandAvailability()
        {
            ViewModel.Start.NotifyCanExecuteChanged();
            ViewModel.Stop.NotifyCanExecuteChanged();
        }

        private void MyRenderTimer_Tick(object sender, EventArgs e)
        {
            displayUniverse(mySimulation.Universe);
        }

        private void displayUniverse(Universe theUniverseToDisplay)
        {
            ViewModel.Planets = theUniverseToDisplay.Planets;
        }

        private DispatcherTimer myRenderTimer;

        internal MainWindowViewModel ViewModel
        {
            get;
            set;
        }
    }
}
