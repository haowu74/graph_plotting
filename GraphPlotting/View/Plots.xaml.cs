﻿using GraphPlotting.Model;
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

        private DispatcherTimer _waveRenderTimer;

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

                _waveRenderTimer = new DispatcherTimer();
                _waveRenderTimer.Interval = TimeSpan.FromMilliseconds(100);
                _waveRenderTimer.Tick += WaveRender;
                _waveRenderTimer.Start();

                Channel4.Waveform.Visibility = Visibility.Collapsed;
                Channel4.SignalPlot.Visibility = Visibility.Collapsed;
                Channel4.MainPlot.SetValue(Grid.RowSpanProperty, 3);
                Channel4.LoveHeartGrid.SetValue(Grid.ColumnSpanProperty, 3);

                DeviceHelper.OpenFile();
            };

            Unloaded += (sender, args) =>
            {
                _renderTimer.Stop();
                _renderTimer = null;
                _waveRenderTimer.Stop();
                _waveRenderTimer = null;
                DeviceHelper.Disconnect();
                DeviceHelper.CloseFile();
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
            bar.ShowValuesAboveBars = true;
            bar = Channel2.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[1]);
            bar.BarWidth = 1000;
            bar.ShowValuesAboveBars = true;
            bar = Channel3.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[2]);
            bar.BarWidth = 1000;
            bar.ShowValuesAboveBars = true;
            bar = Channel4.SignalPlot.Plot.AddBar(ViewModel.SignalPlotValues[3]);
            bar.BarWidth = 1000;
            bar.ShowValuesAboveBars = true;

            var plot = Channel1.Waveform.Plot.AddScatter(ViewModel.WaveXAxial[0], ViewModel.Waveforms[0]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;
            plot = Channel2.Waveform.Plot.AddScatter(ViewModel.WaveXAxial[1], ViewModel.Waveforms[1]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;
            plot = Channel3.Waveform.Plot.AddScatter(ViewModel.WaveXAxial[2], ViewModel.Waveforms[2]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;
            plot = Channel4.Waveform.Plot.AddScatter(ViewModel.WaveXAxial[3], ViewModel.Waveforms[3]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;

            Channel1.Waveform.Plot.AddScatter(ViewModel.PrevWaveXAxial[0], ViewModel.PrevWaveforms[0]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;
            plot = Channel2.Waveform.Plot.AddScatter(ViewModel.PrevWaveXAxial[1], ViewModel.PrevWaveforms[1]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;
            plot = Channel3.Waveform.Plot.AddScatter(ViewModel.PrevWaveXAxial[2], ViewModel.PrevWaveforms[2]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;
            plot = Channel4.Waveform.Plot.AddScatter(ViewModel.PrevWaveXAxial[3], ViewModel.PrevWaveforms[3]);
            plot.Color = System.Drawing.Color.FromArgb(0xe7, 0x73, 0x23);
            plot.LineWidth = 2;

            plot = Channel1.MainPlot.Plot.AddScatter(ViewModel.XAxial[0], ViewModel.Spo2s[0]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 2;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);
            plot = Channel2.MainPlot.Plot.AddScatter(ViewModel.XAxial[1], ViewModel.Spo2s[1]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 2;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);
            plot = Channel3.MainPlot.Plot.AddScatter(ViewModel.XAxial[2], ViewModel.Spo2s[2]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 2;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);
            plot = Channel4.MainPlot.Plot.AddScatter(ViewModel.XAxial[3], ViewModel.Spo2s[3]);
            plot.YAxisIndex = 0;
            plot.LineWidth = 2;
            plot.Color = System.Drawing.Color.FromArgb(0xb2, 0x00, 0x00);

            plot = Channel1.MainPlot.Plot.AddScatter(ViewModel.XAxial[0], ViewModel.Pulses[0]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 2;
            plot.Color = System.Drawing.Color.FromArgb(0xef, 0xde, 0x00);
            plot = Channel2.MainPlot.Plot.AddScatter(ViewModel.XAxial[1], ViewModel.Pulses[1]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 2;
            plot.Color = System.Drawing.Color.FromArgb(0xef, 0xde, 0x00);
            plot = Channel3.MainPlot.Plot.AddScatter(ViewModel.XAxial[2], ViewModel.Pulses[2]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 2;
            plot.Color = System.Drawing.Color.FromArgb(0xef, 0xde, 0x00);
            plot = Channel4.MainPlot.Plot.AddScatter(ViewModel.XAxial[3], ViewModel.Pulses[3]);
            plot.YAxisIndex = 1;
            plot.LineWidth = 2;
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

        private void WaveRender(object sender, EventArgs e)
        {
            ViewModel.UpdateWave();
            Channel1.WaveRender();
            Channel2.WaveRender();
            Channel3.WaveRender();
            Channel4.WaveRender();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Closing the application?",
                    "Close application",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
