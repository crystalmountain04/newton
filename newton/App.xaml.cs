﻿using newton.Services;
using newton.Simulation;
using newton.Utility;
using System.Data;
using System.Windows;

namespace newton
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var aView = new MainWindow();
            var aViewModel = new MainWindowViewModel(
                new SimulationService( new RegularTimer() ),
                new UniverseService(),
                new IOService() );
            aView.DataContext = aViewModel;
            aViewModel.Initialize(new SimulationSzenario(0.005, 500, -0.01, 5, 1000));
            aView.Show();
        }
    }

}
