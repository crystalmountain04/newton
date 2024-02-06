using newton.Simulation;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace newton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel ViewModel
        {
            get { return (this.DataContext as MainWindowViewModel); }
        }

        private bool myIsDragging = false;
        private Planet myDraggedPlanet = null;
        private bool myWasSimulationRunning = false;

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startDragMode(sender as Ellipse);
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            executeMove(e.GetPosition(myHost));
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            stopDragMode();
        }

        private void executeMove(Point theNewPosition)
        {
            if (myIsDragging)
            {
                var aUnflippedPosition = theNewPosition;
                var aFlippedPosition = new Point(aUnflippedPosition.X, ViewModel.SandBoxSize - aUnflippedPosition.Y);

                if (myDraggedPlanet != null)
                {
                    myDraggedPlanet.Location = aFlippedPosition;
                }
                else
                {
                    stopDragMode();
                }
            }
        }

        private void startDragMode(Ellipse theDraggedEllipse)
        {
            myWasSimulationRunning = ViewModel.IsSimulationRunning;
            if (ViewModel.IsSimulationRunning && ViewModel.Stop.CanExecute(null))
            {
                ViewModel.Stop.Execute(null);
            }

            if (!myIsDragging)
            {
                myIsDragging = true;
                myDraggedPlanet = theDraggedEllipse.DataContext as Planet;
                this.PreviewMouseMove += MainWindow_MouseMove;
                this.PreviewMouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
            }
        }

        private void stopDragMode()
        {
            if (myIsDragging)
            {
                myIsDragging = false;
                myDraggedPlanet = null;
                (this.DataContext as MainWindowViewModel).SyncToSimulation.Execute(null);
                this.PreviewMouseMove -= MainWindow_MouseMove;
                this.PreviewMouseLeftButtonUp -= MainWindow_MouseLeftButtonUp;
            }

            if (myWasSimulationRunning && ViewModel.Start.CanExecute(null))
            {
                myWasSimulationRunning = false;
                ViewModel.Start.Execute(null);
            }
        }
    }
}