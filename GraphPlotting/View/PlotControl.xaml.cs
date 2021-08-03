using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GraphPlotting.Model;

namespace GraphPlotting.View
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl, INotifyPropertyChanged
    {
        public DeviceReading DeviceReadings
        {
            get 
            { 
                return (DeviceReading)this.GetValue(DeviceReadingsProperty); 
            }
            set 
            { 
                this.SetValue(DeviceReadingsProperty, value); 
            }
        }

        public static readonly DependencyProperty DeviceReadingsProperty =
            DependencyProperty.Register("DeviceReadings", typeof(DeviceReading),
            typeof(PlotControl));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotControl plotControl = d as PlotControl;

            if (plotControl != null)
            {
                plotControl.DataContext = plotControl.DeviceReadings;
            }
        }
        public PlotControl()
        {
            InitializeComponent();

            SignalPlot.Plot.SetAxisLimits(xMin: 0, xMax: 0);
            SignalPlot.Plot.SetAxisLimits(yMin: 0, yMax: 10);
            SignalPlot.Plot.XAxis.Ticks(false);
            SignalPlot.Plot.XAxis.Color(Color.Black);
            SignalPlot.Plot.XAxis2.Color(Color.Black);
            SignalPlot.Plot.YAxis.Ticks(false);
            SignalPlot.Plot.YAxis.Color(Color.Black);
            SignalPlot.Plot.YAxis2.Color(Color.Black);
            SignalPlot.Plot.Style(figureBackground: Color.Black, dataBackground: Color.White);

            Waveform.Plot.SetAxisLimits(xMin: 0, xMax: Configuration.WaveformPlotWidth);
            Waveform.Plot.SetAxisLimits(yMin: 0, yMax: Configuration.WaveformPlotHeight);
            Waveform.Plot.XAxis.Ticks(false);
            Waveform.Plot.XAxis.Color(Color.White);
            Waveform.Plot.XAxis2.Color(Color.White);
            Waveform.Plot.YAxis.Ticks(false);
            Waveform.Plot.YAxis.Color(Color.White);
            Waveform.Plot.YAxis2.Color(Color.White);
            Waveform.Plot.Grid(false);
            Waveform.Plot.Style(figureBackground: Color.Black, dataBackground: Color.Black);

            MainPlot.Plot.SetAxisLimits(xMin: 0, xMax: Configuration.MainPlotWidth);
            MainPlot.Plot.SetAxisLimits(yMin: 40, yMax: Configuration.Spo2PlotHeight, yAxisIndex: 0);
            MainPlot.Plot.SetAxisLimits(yMin: 40, yMax: Configuration.PulsePlotHeight, yAxisIndex: 1);
            MainPlot.Plot.XAxis.Ticks(true);
            MainPlot.Plot.XAxis.TickLabelStyle(fontSize: 20);
            double[] xScaling = { 0, 60, 120, 180, 240, 300, 360, 420, 480, 540, 600 };
            string[] xScaled = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            MainPlot.Plot.XAxis.ManualTickPositions(xScaling, xScaled);
            MainPlot.Plot.XAxis.Color(Color.White);
            MainPlot.Plot.XAxis2.Color(Color.White);
            MainPlot.Plot.YAxis.Color(Color.FromArgb(0xb2, 0x00, 0x00));
            MainPlot.Plot.YAxis2.Ticks(true);
            MainPlot.Plot.YAxis.TickLabelStyle(fontSize: 20);
            MainPlot.Plot.YAxis2.TickLabelStyle(fontSize: 20);
            double[] yScaling = { 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180 };
            string[] yScaled = { "40", "50", "60", "70", "80", "90", "100", "110", "120", "130", "140", "150", "160", "170", "180" };
            MainPlot.Plot.YAxis2.ManualTickPositions(yScaling, yScaled);
            MainPlot.Plot.YAxis2.Color(Color.FromArgb(0xef, 0xde, 0x00));
            MainPlot.Plot.YAxis.Label("SPO2(%)", Color.FromArgb(0xb2, 0x00, 0x00), bold: true, size:20);
            MainPlot.Plot.YAxis2.Label("Pulse(bpm)", Color.FromArgb(0xef, 0xde, 0x00), bold: true, size: 20);
            MainPlot.Plot.Grid(false);
            MainPlot.Plot.Style(figureBackground: Color.Black, dataBackground: Color.Black);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Render()
        {
            SignalPlot.Render();
            Waveform.Render();
            MainPlot.Render();
        }
    }
}
