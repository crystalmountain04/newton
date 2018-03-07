using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace newton
{
    public class Planet : INotifyPropertyChanged
    {
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
            get
            {
                return new Point(myLocation.X - (myMass / 2.0), myLocation.Y - (myMass / 2.0));
            }
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
            set { myMass = value; RaisePropertyChanged(); }
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
