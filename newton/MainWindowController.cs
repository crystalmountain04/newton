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
            myRenderTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
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
            ViewModel.ApplyConstant = new CommandHandler(p => mySimulation.ApplyGravitationConstant(ViewModel.GravitationalConstant), p => true);
            ViewModel.Start = new CommandHandler(p => startSimulation(), p => !mySimulation.IsRunning);
            ViewModel.Stop = new CommandHandler(p => stopSimulation(), p => mySimulation.IsRunning);
            ViewModel.Reset = new CommandHandler(p => resetSimulation(), p => true);
            ViewModel.Save = new CommandHandler(p => saveUniverseToFile(), p => true);
            ViewModel.Load = new CommandHandler(p => openUniverseFromFile(), p => true);
        }

        private void resetSimulation()
        {
            mySimulation.Initialize(UniverseFactory.CreateUniverse(mySimulation.Universe.Configuration));
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
        }

        private void stopSimulation()
        {
            mySimulation.StopSimulation();
            myRenderTimer.Stop();
            updateCommandAvailability();
        }

        private void updateCommandAvailability()
        {
            ViewModel.Start.RaiseCanExecuteChanged();
            ViewModel.Stop.RaiseCanExecuteChanged();
        }

        private async void MyRenderTimer_Tick(object sender, EventArgs e)
        {
            await displayUniverse(mySimulation.Universe);
        }

        private Task displayUniverse(Universe theUniverseToDisplay)
        {
            return Task.Run(() =>
            {
                ViewModel.Planets = new ObservableCollection<Planet>(theUniverseToDisplay.Planets);
            });
        }

        private DispatcherTimer myRenderTimer;

        internal MainWindowViewModel ViewModel
        {
            get;
            set;
        }
    }
}
