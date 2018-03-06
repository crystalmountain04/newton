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

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
