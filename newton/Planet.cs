using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace newton
{
    public class Planet : INotifyPropertyChanged
    {
        public Planet()
        {
        }

        public Planet(double theMass, Point theInitialLocation, Point theInitialAcceleration, string theColor)
        {
            myMass = theMass;
            myLocation = theInitialLocation;
            myAcceleration = theInitialAcceleration;
            myColor = theColor;
        }

        private Point myLocation;
        public Point Location
        {
            get { return myLocation; }
            set { myLocation = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(DisplayedLocation)); }
        }

        public Point DisplayedLocation
        {
            get { return new Point(myLocation.X - (DisplayedSize / 2.0), myLocation.Y - (DisplayedSize / 2.0)); }
        }

        private Point myAcceleration;
        public Point Acceleration
        {
            get { return myAcceleration; }
            set { myAcceleration = value; RaisePropertyChanged(); }
        }

        private double myMass;
        public double Mass
        {
            get { return myMass; }
            set { myMass = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(DisplayedSize)); }
        }

        public double DisplayedSize
        {
            get { return myMass > 1000 ? 10 : 2; }// myMass / 10.0; }
        }

        private string myColor;
        public string Color
        {
            get { return myColor; }
            set { myColor = value; RaisePropertyChanged(); }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
