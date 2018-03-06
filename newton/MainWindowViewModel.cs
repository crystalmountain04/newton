using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace newton
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private int mySandBoxSize;
        public int SandBoxSize
        {
            get { return mySandBoxSize; }
            set { mySandBoxSize = value; RaisePropertyChanged(); }
        }

        private Planet myPlanet1;
        public Planet Planet1
        {
            get { return myPlanet1; }
            set { myPlanet1 = value; RaisePropertyChanged(); }
        }

        private Planet myPlanet2;
        public Planet Planet2
        {
            get { return myPlanet2; }
            set { myPlanet2 = value; RaisePropertyChanged(); }
        }

        private double myGravitationalConstant;
        public double GravitationalConstant
        {
            get { return myGravitationalConstant; }
            set { myGravitationalConstant = value; RaisePropertyChanged(); }
        }

        public CommandHandler ApplyConstant
        {
            get;
            set;
        }

        public CommandHandler Start
        {
            get;
            set;
        }

        public CommandHandler Stop
        {
            get;
            set;
        }

        public CommandHandler Reset
        {
            get;
            set;
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
