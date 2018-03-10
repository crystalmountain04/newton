using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace newton
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var aView = new MainWindow();
            var aViewModel = new MainWindowViewModel();
            var aController = new MainWindowController();
            aView.DataContext = aViewModel;
            aController.ViewModel = aViewModel;
            aController.Initialize(new Configuration(50, 500, -0.01, 5));
            aView.Show();
        }
    }
}
