using GraphPlotting.Model;
using GraphPlotting.View;
using GraphPlotting.ViewModel.Commands;
using GraphPlotting.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.ViewModel
{
    public class PlotsVM : INotifyPropertyChanged
    {
        public PlotsVM()
        {
            //DeviceHelper.Run();
            DeviceReadings = new ObservableCollection<DeviceReading>();
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 0,
                Readings = new List<Reading>()
            });
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 1,
                Readings = new List<Reading>()
            });
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 2,
                Readings = new List<Reading>()
            });
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 3,
                Readings = new List<Reading>()
            });

            ConnectCommand = new ConnectCommand(this);
            SelectPortCommand = new SelectPortCommand(this);

            SerialPorts = DeviceHelper.GetSerialPorts();
            SelectPortCommands = new ObservableCollection<SelectPort>();

            foreach (var portName in SerialPorts)
            {
                var port = new SelectPort()
                {
                    PortName = portName,
                    SelectPortCommand = SelectPortCommand,
                    IsSelected = false
                };
                SelectPortCommands.Add(port);
            }

            for (int i = 0; i < 4; i++)
            {
                SignalPlotValues[i] = new double[1] { 0 };
                Waveforms[i] = Enumerable.Repeat<double>(-1000, Configuration.TimeSpan).ToArray();
                Spo2s[i] = Enumerable.Repeat<double>(-1000, Configuration.TimeSpan).ToArray();
                Pulses[i] = Enumerable.Repeat<double>(-1000, Configuration.TimeSpan).ToArray();
            }
        }

        public ObservableCollection<DeviceReading> DeviceReadings { get; set; }

        public ConnectCommand ConnectCommand { get; set; }

        public ObservableCollection<SelectPort> SelectPortCommands { get; set; }

        public SelectPortCommand SelectPortCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isConnected;

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        private string selectedSerialPort;
        public string SelectedSerialPort 
        { 
            get { return selectedSerialPort; }
            set
            {
                selectedSerialPort = value;
                OnPropertyChanged("SelectedSerialPort");
            }
        }

        public List<string> SerialPorts { get; set; }

        public void RefreshSelectPortMenuItem()
        {
            OnPropertyChanged("IsSelected");
        }

        public SerialPort SelectedPort { get; set; }

        public void DispatchReadings(List<Reading> readings)
        {
            long startTime = DateTime.Now.Ticks / 10000000 - Configuration.TimeSpan; ;

            foreach (var r in readings)
            {
                if (r.DeviceId == "M1")
                {
                    DeviceReadings[0].Readings.Add(r);
                }
                else if (r.DeviceId == "M2")
                {
                    DeviceReadings[1].Readings.Add(r);
                }
                else if (r.DeviceId == "M3")
                {
                    DeviceReadings[2].Readings.Add(r);
                }
                else if (r.DeviceId == "M4")
                {
                    DeviceReadings[3].Readings.Add(r);
                }

                DeviceReadings[0].Readings.RemoveAll(r => r.TimeStamp < startTime);
                DeviceReadings[1].Readings.RemoveAll(r => r.TimeStamp < startTime);
                DeviceReadings[2].Readings.RemoveAll(r => r.TimeStamp < startTime);
                DeviceReadings[3].Readings.RemoveAll(r => r.TimeStamp < startTime);

                if(Updating)
                {
                    SignalPlotValues[0][0] = (double)(DeviceReadings[0]?.Reading?.SignalStrength ?? 0);
                    SignalPlotValues[1][0] = (double)(DeviceReadings[1]?.Reading?.SignalStrength ?? 0);
                    SignalPlotValues[2][0] = (double)(DeviceReadings[2]?.Reading?.SignalStrength ?? 0);
                    SignalPlotValues[3][0] = (double)(DeviceReadings[3]?.Reading?.SignalStrength ?? 0);

                    Process(DeviceReadings[0].Readings, ref Waveforms[0], ref Spo2s[0], ref Pulses[0], startTime);
                    Process(DeviceReadings[1].Readings, ref Waveforms[1], ref Spo2s[1], ref Pulses[1], startTime);
                    Process(DeviceReadings[2].Readings, ref Waveforms[2], ref Spo2s[2], ref Pulses[2], startTime);
                    Process(DeviceReadings[3].Readings, ref Waveforms[3], ref Spo2s[3], ref Pulses[3], startTime);

                    OnPropertyChanged("DeviceReadings");

                    Updating = false;
                }
            }
        }

        public Plots PlotsView { get; set; }

        public double[][] SignalPlotValues { get; set; } = new double[4][];

        public double[][] Waveforms { get; set; } = new double[4][];

        public double[][] Spo2s { get; set; } = new double[4][];

        public double[][] Pulses { get; set; } = new double[4][];

        private void Process(List<Reading> readings, ref double[] waveforms, ref double[] spo2s, ref double[] pulses, long startTime)
        {
             var reduced = readings.Where(r => r.TimeStamp >= startTime).GroupBy(r => r.TimeStamp)
               .Select(g => new Reading(g.Select(g => g.DeviceId).FirstOrDefault(), (int)g.Average(g => g.Spo2), (int)g.Average(g => g.Pulse), (int)g.Average(g => g.PulseWaveform), (int)g.Average(g => g.SignalStrength), g.Key)).OrderBy(r => r.TimeStamp).ToList();
            
            for(var i = 0; i < Configuration.TimeSpan; i++)
            {
                var reading = reduced.FirstOrDefault(r => r.TimeStamp == startTime + i);
                if (reading is not null)
                {
                    waveforms[i] = reading.PulseWaveform;
                    spo2s[i] = reading.Spo2;
                    pulses[i] = reading.Pulse;
                }
            }
        }

        public void UpdateView()
        {
            Updating = true;
        }

        public bool Updating { get; set; }
    }
}
