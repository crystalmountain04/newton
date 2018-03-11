using newton.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool myIsDragging = false;
        private Planet myDraggedPlanet = null; 

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var aCapturedEllipse = (sender as Ellipse);
            if (null != aCapturedEllipse)
            {
                myDraggedPlanet = aCapturedEllipse.DataContext as Planet;
                myIsDragging = true;
                myHost.MouseMove += ACapturedEllipse_MouseMove;
                myHost.MouseLeftButtonUp += ACapturedEllipse_MouseLeftButtonUp;
            }
        }

        private void ACapturedEllipse_MouseMove(object sender, MouseEventArgs e)
        {
            if ( myIsDragging)
            {
                var aViewModel = this.DataContext as MainWindowViewModel;
                var aUnflippedPosition = e.GetPosition(myHost);
                var aFlippedPosition = new Point(aUnflippedPosition.X, aViewModel.SandBoxSize - aUnflippedPosition.Y);

                myDraggedPlanet.Location = aFlippedPosition;
            }
        }

        private void ACapturedEllipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (myIsDragging)
            {
                myIsDragging = false;
                myDraggedPlanet = null;
                myHost.MouseMove -= ACapturedEllipse_MouseMove;
                myHost.MouseLeftButtonUp -= ACapturedEllipse_MouseLeftButtonUp;
            }
        }
    }
}
