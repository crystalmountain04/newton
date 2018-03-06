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
        private Point myP1Start = new Point(10, 10);
        public Point P1Start
        {
            get { return myP1Start; }
            set { myP1Start = value; RaisePropertyChanged(); }
        }

        private Point myP1End = new Point(50, 50);
        public Point P1End
        {
            get { return myP1End; }
            set { myP1End = value; RaisePropertyChanged(); }
        }

        private Point myP2Start = new Point(10, 60);
        public Point P2Start
        {
            get { return myP2Start; }
            set { myP2Start = value; RaisePropertyChanged(); }
        }

        private Point myP2End = new Point(150, 60);
        public Point P2End
        {
            get { return myP2End; }
            set { myP2End = value; RaisePropertyChanged(); }
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
