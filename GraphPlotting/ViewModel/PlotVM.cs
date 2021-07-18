using GraphPlotting.Model;
using GraphPlotting.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GraphPlotting.ViewModel
{
    public class PlotVM
    {
        // ten minutes data will be displayed
        private readonly TimeSpan timeSpan = new TimeSpan(0, 0, 0, 10, 0);

        // The id of the signal channels: 1 - 4
        public int Id { get; set; }

        public List<Reading> Readings
        {
            get
            {
                return DeviceReadings.Instance.Readings.Where(r => r.Id == Id).ToList();
            }
        }

        public List<int> Spo2s
        {
            get
            {
                return Readings.Where(r => r.TimeStamp > DateTime.Now - timeSpan).Select(r => r.Spo2).ToList();
            }
        }

        public List<int> Pulses
        {
            get
            {
                return Readings.Where(r => r.TimeStamp > DateTime.Now - timeSpan).Select(r => r.Pulse).ToList(); ;
            }
        }

        public List<int> PulseWaveforms
        {
            get
            {
                return Readings.Where(r => r.TimeStamp > DateTime.Now - timeSpan).Select(r => r.PulseWaveform).ToList(); ;
            }
        }

        public int SignalStrength 
        { 
            get 
            { 
                return Readings?.Select(r => r.SignalStrength)?.FirstOrDefault() ?? 0; 
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

        public PlotVM(int id)
        {
            Id = id;
        }
    }
}
