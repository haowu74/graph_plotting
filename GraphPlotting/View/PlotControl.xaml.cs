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

        private double[] ss = { 10 }; 
        public PlotControl()
        {
            InitializeComponent();

            SignalPlot.Plot.AddBar(ss);
            SignalPlot.Plot.AxisAutoX(margin: 0);
            SignalPlot.Plot.SetAxisLimits(yMin: 0, yMax: 50);

            MainPlot.Plot.Style(figureBackground: Color.Black, dataBackground: Color.Black);

            Loaded += (sender, args) =>
            {
                //Plot = new PlotVM(Id);
                //double[] dataX = new double[] { 1, 2, 3, 4, 5 };
                //double[] dataY = new double[] { 1, 4, 9, 16, 25 };
                //MainPlot.Plot.AddScatter(dataX, dataY);
                
                //_timer = new Timer(state =>
                //{
                //    ss[0] = Plot.SignalStrength;
                //    OnPropertyChanged("Plot");
                //}, null, 1000, 1000);

                //_renderTimer = new DispatcherTimer();
                //_renderTimer.Interval = TimeSpan.FromMilliseconds(1000);
                //_renderTimer.Tick += Render;
                //_renderTimer.Start();
            };

            Unloaded += (sender, args) => {
                //_timer?.Dispose();
                //_renderTimer?.Stop();
            };
        }

        private Timer _timer;

        private DispatcherTimer _renderTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Render(object sender, EventArgs e)
        {
            SignalPlot.Render();
        }
    }
}
