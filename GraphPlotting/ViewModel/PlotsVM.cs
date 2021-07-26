using GraphPlotting.Model;
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
                Readings = new List<Reading>() { new Reading("M1",1,1,1,1,1)}
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
            // DeviceReadings[0].Reading = DeviceReadings[0].Readings.LastOrDefault();

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

        public string SelectedSerialPort { get; set; }

        public List<string> SerialPorts { get; set; }

        public void RefreshSelectPortMenuItem()
        {
            OnPropertyChanged("IsSelected");
        }

        public SerialPort SelectedPort { get; set; }

        public void DispatchReadings(List<Reading> readings)
        {
            foreach (var r in readings)
            {
                //Debug.WriteLine($"{r.DeviceId} {r.Spo2} {r.SignalStrength} {r.PulseWaveform} {r.Pulse} {r.TimeStamp}");
                if (r.DeviceId == "M1")
                {
                    DeviceReadings[0].Readings.Add(r);
                    // DeviceReadings[0].Reading = r;
                }
                else if (r.DeviceId == "M2")
                {
                    DeviceReadings[1].Readings.Add(r);
                    // DeviceReadings[1].Reading = r;
                }
                else if (r.DeviceId == "M3")
                {
                    DeviceReadings[2].Readings.Add(r);
                    // DeviceReadings[2].Reading = r;
                }
                else if (r.DeviceId == "M4")
                {
                    DeviceReadings[3].Readings.Add(r);
                    // DeviceReadings[3].Reading = r;
                }
                // DeviceReadings[0].Reading = DeviceReadings[0].Readings.LastOrDefault();
                OnPropertyChanged("DeviceReadings");

            }
        }
    }
}
