using Microsoft.Win32;
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
        private Simulation mySimulation;

        public MainWindowController()
        {
            myRenderTimer = new DispatcherTimer();
            myRenderTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            myRenderTimer.Tick += MyRenderTimer_Tick;
        }

        public void Initialize(Configuration theConfiguration)
        {
            mySimulation = new Simulation();
            mySimulation.Initialize(UniverseFactory.CreateUniverse(theConfiguration));

            applyConfigurationToViewModel(theConfiguration);

            ViewModel.ApplyConstant = new CommandHandler(p => mySimulation.Universe.Configuration.GravitationConstant = ViewModel.GravitationalConstant, p => true);
            ViewModel.Start = new CommandHandler(p => startSimulation(), p => !m_IsRunning);
            ViewModel.Stop = new CommandHandler(p => stopSimulation(), p => m_IsRunning);
            ViewModel.Reset = new CommandHandler(p => mySimulation.Initialize(UniverseFactory.CreateUniverse(mySimulation.Universe.Configuration)), p => true);
            ViewModel.Save = new CommandHandler(p => saveUniverse(), p => true);
            ViewModel.Load = new CommandHandler(p => loadUniverse(), p => true);

            displayUniverse(mySimulation.Universe);
        }

        private void loadUniverse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var aUniverse = new Universe(openFileDialog.FileName);
                if(null != aUniverse)
                {
                    mySimulation.Universe.Planets = aUniverse.Planets;
                    mySimulation.Universe.Configuration = aUniverse.Configuration;
                    applyConfigurationToViewModel(mySimulation.Universe.Configuration);
                }
            }
        }

        private void applyConfigurationToViewModel(Configuration theConfiguration)
        {
            ViewModel.SandBoxSize = theConfiguration.SandboxSize_px;
            ViewModel.GravitationalConstant = theConfiguration.GravitationConstant;
        }

        private void startSimulation()
        {
            m_IsRunning = true;
            mySimulation.StartSimulation();
            myRenderTimer.Start();
            ViewModel.Start.RaiseCanExecuteChanged();
            ViewModel.Stop.RaiseCanExecuteChanged();
        }

        private void saveUniverse()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                var aUniverse = new Universe(mySimulation.Universe.Planets.ToList(), mySimulation.Universe.Configuration);
                aUniverse.Save(saveFileDialog.FileName);
            }
        }

        private void stopSimulation()
        {
            m_IsRunning = false;
            mySimulation.StopSimulation();
            myRenderTimer.Stop();
            ViewModel.Start.RaiseCanExecuteChanged();
            ViewModel.Stop.RaiseCanExecuteChanged();
        }

        private bool m_IsRunning = false;

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
