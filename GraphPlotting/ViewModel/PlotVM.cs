using GraphPlotting.Model;
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
        // ten minutes data will be displayed
        private readonly TimeSpan timeSpan = new TimeSpan(0, 0, 0, 10, 0);
        
        // The id of the signal channels: 1 - 4
        private int id;

        private List<Reading> readings = new List<Reading>();

        public object Id 
        {
            get { return (object)id; } 
            set 
            {
                id = (int)value;
                OnPropertyChanged("Id");
            } 
        }

        public List<int> Spo2s
        {
            get
            {
                return readings.Where(r => r.TimeStamp > DateTime.Now - timeSpan).Select(r => r.Spo2).ToList();
            }
        }

        public List<int> Pulses
        {
            get
            {
                return readings.Where(r => r.TimeStamp > DateTime.Now - timeSpan).Select(r => r.Pulse).ToList(); ;
            }
        }

        public List<int> PulseWaveforms
        {
            get
            {
                return readings.Where(r => r.TimeStamp > DateTime.Now - timeSpan).Select(r => r.PulseWaveform).ToList(); ;
            }
        }

        public int SignalStrength 
        { 
            get 
            { 
                return readings?.Select(r => r.SignalStrength)?.FirstOrDefault() ?? 0; 
            } 
        }

        public int Spo2
        {
            get => Spo2s?.FirstOrDefault() ?? 0;
        }

        public int Pulse
        {
            get => Pulses?.FirstOrDefault() ?? 0;
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
