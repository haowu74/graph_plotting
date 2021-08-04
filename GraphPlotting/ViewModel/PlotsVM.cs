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
            ClearCommand = new ClearCommand(this);
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
                Waveforms[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                Spo2s[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                Pulses[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                XAxial[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                WaveXAxial[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();

                PrevWaveforms[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                PrevSpo2s[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                PrevPulses[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                PrevXAxial[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
                PrevWaveXAxial[i] = Enumerable.Repeat<double>(-1000, Configuration.MainPlotWidth).ToArray();
            }
        }

        public ObservableCollection<DeviceReading> DeviceReadings { get; set; }

        public ConnectCommand ConnectCommand { get; set; }

        public ClearCommand ClearCommand { get; set; }

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
            long startTime = DateTime.Now.Ticks / Configuration.Factor - Configuration.TimeSpan;

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

                if(Updating)
                {
                    SignalPlotValues[0][0] = (double)(DeviceReadings[0]?.Reading?.SignalStrength ?? 0);
                    SignalPlotValues[1][0] = (double)(DeviceReadings[1]?.Reading?.SignalStrength ?? 0);
                    SignalPlotValues[2][0] = (double)(DeviceReadings[2]?.Reading?.SignalStrength ?? 0);
                    SignalPlotValues[3][0] = (double)(DeviceReadings[3]?.Reading?.SignalStrength ?? 0);

                    for(var i = 0; i < 3; i++)
                    {
                        Process(i, DeviceReadings[i].Readings, ref Waveforms[i], ref Spo2s[i], ref Pulses[i], ref XAxial[i], ref WaveXAxial[i],
                            ref PrevWaveforms[i], ref PrevSpo2s[i], ref PrevPulses[i], ref PrevXAxial[i], ref PrevWaveXAxial[i]);
                    }

                    OnPropertyChanged("DeviceReadings");

                    Updating = false;
                }
            }
        }

        public double[][] SignalPlotValues { get; set; } = new double[4][];
        public double[][] PrevSignalPlotValues { get; set; } = new double[4][];

        public double[][] Waveforms { get; set; } = new double[4][];
        public double[][] PrevWaveforms { get; set; } = new double[4][];

        public double[][] Spo2s { get; set; } = new double[4][];
        public double[][] PrevSpo2s { get; set; } = new double[4][];

        public double[][] Pulses { get; set; } = new double[4][];
        public double[][] PrevPulses { get; set; } = new double[4][];

        private void Process(int index, List<Reading> readings, ref double[] waveforms, ref double[] spo2s, ref double[] pulses, ref double[] xAxial, ref double[] waveXAxial,
            ref double[] prevWaveforms, ref double[] prevSpo2s, ref double[] prevPulses, ref double[] prevXAxial, ref double[] prevWaveXAxial)
        {
            var current = DateTime.Now.Ticks / Configuration.Factor - 1;
            Debug.WriteLine(current);
            var reduced = readings.Where(r => r.TimeStamp > LastTime[index]).GroupBy(r => r.TimeStamp)
                .Select(g => new Reading(g.Select(g => g.DeviceId).FirstOrDefault(), (int)g.Average(g => g.Spo2), (int)g.Average(g => g.Pulse), (int)g.Average(g => g.PulseWaveform), (int)g.Average(g => g.SignalStrength), g.Key)).OrderBy(r => r.TimeStamp).ToList();
            readings.RemoveAll(r => r.TimeStamp < LastTime[index]);
            
            // Update MainPlot
            foreach (var reading in reduced)
            {
                if (reading.TimeStamp - StartTime[index] >= Configuration.MainPlotWidth)
                {
                    MainPlotPointer[index] = 0;
                    StartTime[index] = current;
                    Array.Copy(spo2s, prevSpo2s, Configuration.MainPlotWidth);
                    Array.Copy(pulses, prevPulses, Configuration.MainPlotWidth);
                    Array.Copy(xAxial, prevXAxial, Configuration.MainPlotWidth);
                    for (int i = 0; i < Configuration.MainPlotWidth; i++)
                    {
                        spo2s[i] = -1000;
                        pulses[i] = -1000;
                        xAxial[i] = -1000;
                    }
                    break;
                }
                else
                {
                    MainPlotPointer[index] = (int)(reading.TimeStamp - StartTime[index]);
                }
            }

            // Update WaveformPlot
            foreach (var reading in reduced)
            { 
                waveforms[WaveformPlotPointer[index]] = reading.PulseWaveform;

                if (reading.TimeStamp - WaveStartTime[index] >= Configuration.WaveformPlotWidth)
                {
                    WaveformPlotPointer[index] = 0;
                    WaveStartTime[index] = current;
                    Array.Copy(waveforms, prevWaveforms, Configuration.WaveformPlotWidth);
                    Array.Copy(waveXAxial, prevWaveXAxial, Configuration.WaveformPlotWidth);
                    for (int i = 0; i < Configuration.WaveformPlotWidth; i++)
                    {
                        waveforms[i] = -1000;
                        xAxial[i] = -1000;
                    }
                    break;
                }
                else
                {
                    WaveformPlotPointer[index] = (int)(reading.TimeStamp - WaveStartTime[index]);
                }

                waveforms[WaveformPlotPointer[index]] = reading.PulseWaveform;
                waveXAxial[WaveformPlotPointer[index]] = reading.TimeStamp - WaveStartTime[index];
            }

            if (MainPlotPointer[index] > 0)
            {
                for (var i = MainPlotPointer[index]; i < Configuration.MainPlotWidth; i++)
                {
                    spo2s[i] = spo2s[i-1];
                    pulses[i] = pulses[i-1];
                    waveforms[i] = waveforms[i - 1];
                    xAxial[i] = xAxial[i - 1];
                }
            }

            if (WaveformPlotPointer[index] > 0)
            {
                for (var i = WaveformPlotPointer[index]; i < Configuration.WaveformPlotWidth; i++)
                {
                    waveforms[i] = waveforms[i - 1];
                    waveXAxial[i] = waveXAxial[i - 1];
                }
            }

            for (var i = Configuration.MainPlotWidth - 2; i >= 0; i--)
            {
                if (prevXAxial[i] < xAxial[MainPlotPointer[index]])
                {
                    prevXAxial[i] = prevXAxial[i + 1];
                    prevSpo2s[i] = prevSpo2s[i + 1];
                    prevPulses[i] = prevPulses[i + 1];
                }
            }

            for (var i = Configuration.WaveformPlotWidth - 2; i >= 0; i--)
            {
                if (prevWaveXAxial[i] < xAxial[WaveformPlotPointer[index]])
                {
                    prevWaveXAxial[i] = prevWaveXAxial[i + 1];
                    prevWaveforms[i] = prevWaveforms[i + 1];
                }
            }

            LastTime[index] = current;
        }

        public void UpdateView()
        {
            Updating = true;
        }

        public bool Updating { get; set; }

        public double[][] XAxial { get; set; } = new double[4][];
        public double[][] PrevXAxial { get; set; } = new double[4][];

        public double[][] WaveXAxial { get; set; } = new double[4][];
        public double[][] PrevWaveXAxial { get; set; } = new double[4][];

        public void ClearPlots()
        {
            ClearCurrentPlots();
            ClearPrevPlots();
            ClearSignalPlots();
            OnPropertyChanged("DeviceReadings");
        }

        private void ClearCurrentPlots()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Configuration.MainPlotWidth; j++)
                {
                    Spo2s[i][j] = -1000;
                    Pulses[i][j] = -1000;
                    Waveforms[i][j] = -1000;
                    XAxial[i][j] = -1000;
                }
            }
        }

        private void ClearPrevPlots()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Configuration.MainPlotWidth; j++)
                {
                    PrevSpo2s[i][j] = -1000;
                    PrevPulses[i][j] = -1000;
                    PrevWaveXAxial[i][j] = -1000;
                    PrevXAxial[i][j] = -1000;
                }
            }
        }

        private void ClearSignalPlots()
        {
            for (int i = 0; i < 4; i++)
            {
                SignalPlotValues[i][0] = 0;
                DeviceReadings[i].Readings.Clear();
            }
        }

        public int[] MainPlotPointer { get; set; } = new int[4] { 0, 0, 0, 0 };
        public int[] WaveformPlotPointer { get; set; } = new int[4] { 0, 0, 0, 0 };
        public long[] StartTime { get; set; } = new long[4] { 0, 0, 0, 0 };
        public long[] WaveStartTime { get; set; } = new long[4] { 0, 0, 0, 0 };

        private long[] LastTime = new long[4] { 0, 0, 0, 0 };
    }
}
