using GraphPlotting.Model;
using GraphPlotting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GraphPlotting.View
{
    /// <summary>
    /// Interaction logic for Plots.xaml
    /// </summary>
    public partial class Plots : Window
    {
        public PlotsVM ViewModel { get; set; }

        private DispatcherTimer _renderTimer;

        public Plots()
        {
            InitializeComponent();
            ViewModel = (PlotsVM)(this.DataContext);
            ViewModel.PlotsView = this;

            InitPlots();

            Loaded += (sender, args) =>
            {
                _renderTimer = new DispatcherTimer();
                _renderTimer.Interval = TimeSpan.FromMilliseconds(1000);
                _renderTimer.Tick += Render;
                _renderTimer.Start();
            };
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Closing the application?",
                    "Close application",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Plots V0.0.1 \nAll right reserved.", "About", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        }

        private void InitPlots()
        {
            Channel1.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[0]);
            Channel2.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[1]);
            Channel3.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[2]);
            Channel4.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[3]);

            double[] X = Enumerable.Range(0, Configuration.TimeSpan).Select(i => (double)i).ToArray();

            //Channel1.Waveform.Plot.AddScatter(X, ViewModel.Waveforms[0]);
            //Channel2.Waveform.Plot.AddScatter(X, ViewModel.Waveforms[1]);
            //Channel3.Waveform.Plot.AddScatter(X, ViewModel.Waveforms[2]);
            //Channel4.Waveform.Plot.AddScatter(X, ViewModel.Waveforms[3]);
            Channel1.Waveform.Plot.AddSignal(ViewModel.Waveforms[0]);
            Channel2.Waveform.Plot.AddSignal(ViewModel.Waveforms[1]);
            Channel3.Waveform.Plot.AddSignal(ViewModel.Waveforms[2]);
            Channel4.Waveform.Plot.AddSignal(ViewModel.Waveforms[3]);

            Channel1.MainPlot.Plot.AddScatter(X, ViewModel.Spo2s[0]);
            Channel2.MainPlot.Plot.AddScatter(X, ViewModel.Spo2s[1]);
            Channel3.MainPlot.Plot.AddScatter(X, ViewModel.Spo2s[2]);
            Channel4.MainPlot.Plot.AddScatter(X, ViewModel.Spo2s[3]);
        }

        private void Render(object sender, EventArgs e)
        {
            ViewModel.UpdateView();
            Channel1.Render();
            Channel2.Render();
            Channel3.Render();
            Channel4.Render();
        }
    }
}
