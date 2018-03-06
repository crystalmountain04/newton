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
        private Point myLocation;
        public Point Location
        {
            get { return myLocation; }
            set { myLocation = value; RaisePropertyChanged(); }
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

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
