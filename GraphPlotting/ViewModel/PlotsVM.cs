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
            MeanCommand = new MeanCommand(this);
            Filter1Command = new Filter1Command(this);
            Filter2Command = new Filter2Command(this);

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
                Waveforms[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                Spo2s[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                Pulses[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                XAxial[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                WaveXAxial[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();

                PrevWaveforms[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                PrevSpo2s[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                PrevPulses[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                PrevXAxial[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
                PrevWaveXAxial[i] = Enumerable.Repeat<double>(Configuration.DummyValue, Configuration.MainPlotWidth).ToArray();
            }
        }

        public ObservableCollection<DeviceReading> DeviceReadings { get; set; }

        public ConnectCommand ConnectCommand { get; set; }

        public ClearCommand ClearCommand { get; set; }
        public MeanCommand MeanCommand { get; set; }
        public Filter1Command Filter1Command { get; set; }
        public Filter2Command Filter2Command { get; set; }

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

        public void DispatchReadings(List<Reading> readings)
        {
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
            }

            if(Updating || UpdatingWave)
            {
                SignalPlotValues[0][0] = (double)(DeviceReadings[0]?.Reading?.SignalStrength ?? 0);
                SignalPlotValues[1][0] = (double)(DeviceReadings[1]?.Reading?.SignalStrength ?? 0);
                SignalPlotValues[2][0] = (double)(DeviceReadings[2]?.Reading?.SignalStrength ?? 0);
                SignalPlotValues[3][0] = (double)(DeviceReadings[3]?.Reading?.SignalStrength ?? 0);

                for(var i = 0; i < 3; i++)
                {
                    var reading = DeviceReadings[i].Readings.LastOrDefault();
                    if (reading is not null)
                    {
                        DeviceReadings[i].Reading.DeviceId = reading?.DeviceId ?? "";
                        DeviceReadings[i].Reading.Pulse = reading.Pulse;
                        DeviceReadings[i].Reading.Spo2 = reading?.Spo2 ?? 0;
                        DeviceReadings[i].Reading.PulseWaveform = reading?.PulseWaveform ?? 0;
                        DeviceReadings[i].Reading.SignalStrength = reading?.SignalStrength ?? 0;
                    }

                    Process(i, DeviceReadings[i].Readings, ref Waveforms[i], ref Spo2s[i], ref Pulses[i], ref XAxial[i], ref WaveXAxial[i],
                        ref PrevWaveforms[i], ref PrevSpo2s[i], ref PrevPulses[i], ref PrevXAxial[i], ref PrevWaveXAxial[i]);
                }

                OnPropertyChanged("DeviceReadings");

                Updating = false;
                UpdatingWave = false;
            }
        }

        public double[][] SignalPlotValues { get; set; } = new double[4][];

        public double[][] Waveforms { get; set; } = new double[4][];
        public double[][] PrevWaveforms { get; set; } = new double[4][];

        public double[][] Spo2s { get; set; } = new double[4][];
        public double[][] PrevSpo2s { get; set; } = new double[4][];

        public double[][] Pulses { get; set; } = new double[4][];
        public double[][] PrevPulses { get; set; } = new double[4][];

        private void Process(int index, List<Reading> readings, ref double[] waveforms, ref double[] spo2s, ref double[] pulses, ref double[] xAxial, ref double[] waveXAxial,
            ref double[] prevWaveforms, ref double[] prevSpo2s, ref double[] prevPulses, ref double[] prevXAxial, ref double[] prevWaveXAxial)
        {
            var current = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            var reduced = readings.LastOrDefault();
            if (reduced is not null)
            {
                MainPlotPointer[index] = (int)(current - StartTime[index]);
                if (MainPlotPointer[index] >= Configuration.MainPlotWidth)
                {
                    MainPlotPointer[index] = 0;
                    StartTime[index] = current;
                    Array.Copy(spo2s, prevSpo2s, Configuration.MainPlotWidth);
                    Array.Copy(pulses, prevPulses, Configuration.MainPlotWidth);
                    Array.Copy(xAxial, prevXAxial, Configuration.MainPlotWidth);
                    for (int i = 0; i < Configuration.MainPlotWidth; i++)
                    {
                        spo2s[i] = Configuration.DummyValue;
                        pulses[i] = Configuration.DummyValue;
                        xAxial[i] = Configuration.DummyValue;
                    }
                }
                else
                {
                    if (MainPlotPointer[index] >= Configuration.MainPlotWidth * 0.9)
                    {
                        for (int i = 0; i < Configuration.MainPlotWidth * 0.9; i++)
                        {
                            spo2s[i] = spo2s[i+60];
                            pulses[i] = pulses[i+60];
                        }
                        MainPlotPointer[index] = (int)( Configuration.MainPlotWidth * 0.8);
                        StartTime[index] = StartTime[index] + (int)(Configuration.MainPlotWidth * 0.1);
                    }
                }

                spo2s[MainPlotPointer[index]] = reduced.Spo2;
                pulses[MainPlotPointer[index]] = reduced.Pulse;
                xAxial[MainPlotPointer[index]] = MainPlotPointer[index];
            }

            // Update WaveformPlot
            foreach (var reading in readings)
            { 
                WaveformPlotPointer[index] += 1;
                if (WaveformPlotPointer[index] >= Configuration.WaveformPlotWidth)
                {
                    WaveformPlotPointer[index] = 0;
                    WaveStartTime[index] = current;
                    Array.Copy(waveforms, prevWaveforms, Configuration.WaveformPlotWidth);
                    Array.Copy(waveXAxial, prevWaveXAxial, Configuration.WaveformPlotWidth);
                    for (int i = 0; i < Configuration.WaveformPlotWidth; i++)
                    {
                        waveforms[i] = Configuration.DummyValue;
                        waveXAxial[i] = Configuration.DummyValue;
                    }
                    break;
                }
                waveforms[WaveformPlotPointer[index]] = reading.PulseWaveform;
                waveXAxial[WaveformPlotPointer[index]] = WaveformPlotPointer[index];
            }

            if (MainPlotPointer[index] > 0)
            {
                for (var i = MainPlotPointer[index]+1; i < Configuration.MainPlotWidth; i++)
                {
                    spo2s[i] = spo2s[i - 1];
                    pulses[i] = pulses[i - 1];
                    xAxial[i] = xAxial[i - 1];
                }
            }

            if (WaveformPlotPointer[index] > 0)
            {
                for (var i = WaveformPlotPointer[index]+1; i < Configuration.WaveformPlotWidth; i++)
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
                if (prevWaveXAxial[i] < waveXAxial[WaveformPlotPointer[index]])
                {
                    prevWaveXAxial[i] = prevWaveXAxial[i + 1];
                    prevWaveforms[i] = prevWaveforms[i + 1];
                }
            }
            readings.Clear();
        }

        public void UpdateView()
        {
            Updating = true;
        }

        public void UpdateWave()
        {
            UpdatingWave = true;
        }

        private bool Updating;

        private bool UpdatingWave;

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
                    Spo2s[i][j] = Configuration.DummyValue;
                    Pulses[i][j] = Configuration.DummyValue;
                    Waveforms[i][j] = Configuration.DummyValue;
                    XAxial[i][j] = Configuration.DummyValue;
                }
            }
        }

        private void ClearPrevPlots()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Configuration.MainPlotWidth; j++)
                {
                    PrevSpo2s[i][j] = Configuration.DummyValue;
                    PrevPulses[i][j] = Configuration.DummyValue;
                    PrevWaveXAxial[i][j] = Configuration.DummyValue;
                    PrevXAxial[i][j] = Configuration.DummyValue;
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

        private int[] MainPlotPointer = new int[4] { 0, 0, 0, 0 };

        private int[] WaveformPlotPointer = new int[4] { 0, 0, 0, 0 };

        private long[] StartTime = new long[4] { 0, 0, 0, 0 };

        private long[] WaveStartTime = new long[4] { 0, 0, 0, 0 };
        public FilterMode FilterMode { get; set; }
    }
}
