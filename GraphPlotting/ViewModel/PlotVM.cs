using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.ViewModel
{
    public class PlotVM: INotifyPropertyChanged
    {
        private int id;
        public object Id 
        { 
            get { return (object)id; } 
            set 
            {
                id = (int)value;
                OnPropertyChanged("Id");
            } 
        }

        public PlotVM()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
