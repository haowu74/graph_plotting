using GraphPlotting.ViewModel;
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

            SignalPlot.Plot.AxisAutoX(margin: 0);
            SignalPlot.Plot.SetAxisLimits(yMin: 0, yMax: 100);
            string[] empty = { "" };
            SignalPlot.Plot.YTicks(empty);
            SignalPlot.Plot.XTicks(empty);
            //SignalPlot.Plot.Style(figureBackground: Color.Black, dataBackground: Color.Black);

            Waveform.Plot.SetAxisLimits(yMin: 0, yMax: 200);
            Waveform.Plot.Style(figureBackground: Color.Black, dataBackground: Color.Black);

            MainPlot.Plot.SetAxisLimits(yMin: 0, yMax: 20, yAxisIndex: 0);
            MainPlot.Plot.SetAxisLimits(yMin: 0, yMax: 200, yAxisIndex: 1);
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

        public int SignalStrength { get; set; }
    }
}
