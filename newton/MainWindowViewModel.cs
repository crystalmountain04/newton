using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using newton.Services;
using newton.Simulation;
using System.Windows.Threading;

namespace newton
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly DispatcherTimer myRenderTimer;
        private readonly ISimulationService mySimulationService;
        private readonly IUniverseService myUniverseService;
        private readonly IIOService myIOService;

        public MainWindowViewModel( ISimulationService theSimulationEngine, IUniverseService theUniverseService, IIOService theIOService )
        {
            mySimulationService = theSimulationEngine;
            myUniverseService = theUniverseService;
            myIOService = theIOService;
            ApplyConstant = new RelayCommand(() => mySimulationService?.ApplyGravitationConstant(GravitationalConstant));
            Start = new RelayCommand(startSimulation, () => mySimulationService != null && !mySimulationService.IsRunning);
            Stop = new RelayCommand(stopSimulation, () => mySimulationService != null && mySimulationService.IsRunning);
            Reset = new RelayCommand(resetSimulation);
            Save = new RelayCommand(saveUniverseToFile);
            Load = new RelayCommand(openUniverseFromFile);
            SyncToSimulation = new RelayCommand(syncToSimulation);

            myRenderTimer = new DispatcherTimer();
            myRenderTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            myRenderTimer.Tick += MyRenderTimer_Tick;
        }

        public void Initialize(SimulationSzenario theConfiguration)
        {
            mySimulationService.Initialize(myUniverseService.CreateUniverse(theConfiguration));

            configureVisualization(theConfiguration);
            displayUniverse(mySimulationService.Universe);
        }

        private void resetSimulation()
        {
            mySimulationService.Initialize(myUniverseService.CreateUniverse(mySimulationService.Universe.Configuration));
            updateCommandAvailability();
            displayUniverse(mySimulationService.Universe);
        }

        private void openUniverseFromFile()
        {
            var aFileName = myIOService.OpenFile();
            if (string.IsNullOrEmpty(aFileName))
            {
                return;
            }

            var aUniverse = myUniverseService.LoadUniverse(aFileName);
            if (null != aUniverse)
            {
                mySimulationService.Initialize(aUniverse);
                updateCommandAvailability();
                configureVisualization(aUniverse.Configuration);
                displayUniverse(mySimulationService.Universe);
            }
        }

        private void saveUniverseToFile()
        {
            var aFileName = myIOService.SaveFile();
            if (string.IsNullOrEmpty(aFileName))
            {
                return;
            }

            myUniverseService.SaveUniverse(aFileName, mySimulationService.Universe);
        }

        private void syncToSimulation()
        {
            mySimulationService.Universe.Planets = Planets.ConvertAll(p => new Planet(p));
        }

        private void configureVisualization(SimulationSzenario theConfiguration)
        {
            SandBoxSize = theConfiguration.SandboxSize_px;
            GravitationalConstant = theConfiguration.GravitationConstant;
        }

        private void startSimulation()
        {
            mySimulationService.StartSimulation();
            myRenderTimer.Start();
            updateCommandAvailability();
            IsSimulationRunning = true;
        }

        private void stopSimulation()
        {
            mySimulationService.StopSimulation();
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
            displayUniverse(mySimulationService.Universe);
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
