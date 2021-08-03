using GraphPlotting.Model;
using GraphPlotting.ViewModel;
using GraphPlotting.ViewModel.Helpers;
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

            InitPlots();

            Loaded += (sender, args) =>
            {
                _renderTimer = new DispatcherTimer();
                _renderTimer.Interval = TimeSpan.FromMilliseconds(1000);
                _renderTimer.Tick += Render;
                _renderTimer.Start();
            };

            Closing += (sender, args) =>
            {
                _renderTimer.Stop();
                _renderTimer = null;
                DeviceHelper.Disconnect();
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
            
            MessageBox.Show("SpO2 Integrator V1.0 \nAll rights reserved.", "About", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        }

        private void InitPlots()
        {
            var bar = Channel1.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[0]);
            bar.BarWidth = 1000;
            bar = Channel2.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[1]);
            bar.BarWidth = 1000;
            bar = Channel3.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[2]);
            bar.BarWidth = 1000;
            bar = Channel4.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[3]);
            bar.BarWidth = 1000;

            //double[] WaveformX = Enumerable.Repeat(40.0, 600).ToArray();

            //var plot_straight = Channel1.Waveform.Plot.AddSignal(WaveformX);
            //plot_straight.Color = System.Drawing.Color.FromArgb(0xff, 0xff, 0x00);
            //plot_straight.LineWidth = 5;
            //plot_straight = Channel2.Waveform.Plot.AddSignal(WaveformX);
            //plot_straight.Color = System.Drawing.Color.FromArgb(0xff, 0xff, 0x00);
            //plot_straight.LineWidth = 5;
            //plot_straight = Channel3.Waveform.Plot.AddSignal(WaveformX);
            //plot_straight.Color = System.Drawing.Color.FromArgb(0xff, 0xff, 0x00);
            //plot_straight.LineWidth = 5;
            //plot_straight = Channel4.Waveform.Plot.AddSignal(WaveformX);
            //plot_straight.Color = System.Drawing.Color.FromArgb(0xff, 0xff, 0x00);
            //plot_straight.LineWidth = 5;

            var plot = Channel1.Waveform.Plot.AddScatter(ViewModel.XAxial[0], ViewModel.Waveforms[0]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 5;
            plot = Channel2.Waveform.Plot.AddScatter(ViewModel.XAxial[1], ViewModel.Waveforms[1]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 5;
            plot = Channel3.Waveform.Plot.AddScatter(ViewModel.XAxial[2], ViewModel.Waveforms[2]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 5;
            plot = Channel4.Waveform.Plot.AddScatter(ViewModel.XAxial[3], ViewModel.Waveforms[3]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 5;

            plot = Channel1.MainPlot.Plot.AddScatter(ViewModel.XAxial[0], ViewModel.Spo2s[0]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);
            plot = Channel2.MainPlot.Plot.AddScatter(ViewModel.XAxial[1], ViewModel.Spo2s[1]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);
            plot = Channel3.MainPlot.Plot.AddScatter(ViewModel.XAxial[2], ViewModel.Spo2s[2]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);
            plot = Channel4.MainPlot.Plot.AddScatter(ViewModel.XAxial[3], ViewModel.Spo2s[3]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);

            plot = Channel1.MainPlot.Plot.AddScatter(ViewModel.XAxial[0], ViewModel.Pulses[0]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xef, 0xde, 0x00);
            plot = Channel2.MainPlot.Plot.AddScatter(ViewModel.XAxial[1], ViewModel.Pulses[1]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xef, 0xde, 0x00);
            plot = Channel3.MainPlot.Plot.AddScatter(ViewModel.XAxial[2], ViewModel.Pulses[2]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xef, 0xde, 0x00);
            plot = Channel4.MainPlot.Plot.AddScatter(ViewModel.XAxial[3], ViewModel.Pulses[3]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 5;
            plot.Color = System.Drawing.Color.FromArgb(0xef, 0xde, 0x00);
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
