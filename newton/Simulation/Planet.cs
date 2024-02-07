using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace newton.Simulation
{
    public partial class Planet : ObservableObject
    {
        public Planet()
        {
        }

        public Planet(double theMass, Point theInitialLocation, Point theInitialAcceleration, string theColor)
        {
            Mass = theMass;
            Location = theInitialLocation;
            Acceleration = theInitialAcceleration;
            Color = theColor;
        }

        public Planet(Planet theCopy)
        {
            Mass = theCopy.Mass;
            Location = new Point(theCopy.Location.X, theCopy.Location.Y);
            Acceleration = new Point(theCopy.Acceleration.X, theCopy.Acceleration.Y);
            Color = theCopy.Color;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayedLocation))]
        private Point location;

        public Point DisplayedLocation
        {
            get { return new Point(Location.X - (DisplayedSize / 2.0), Location.Y - (DisplayedSize / 2.0)); }
        }

        public Point Acceleration
        {
            get;
            set;
        }

        public double Mass
        {
            get;
            set;
        }

        public double DisplayedSize
        {
            get => Mass > 1000 ? 30 : 10;
        }

        public string Color
        {
            get;
            set;
        }

        public bool IsDoomed
        {
            get;
            set;
        }
    }
}
