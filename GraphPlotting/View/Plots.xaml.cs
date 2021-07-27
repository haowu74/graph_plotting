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

namespace GraphPlotting.View
{
    /// <summary>
    /// Interaction logic for Plots.xaml
    /// </summary>
    public partial class Plots : Window
    {
        public PlotsVM ViewModel { get; set; }

        public Plots()
        {
            InitializeComponent();
            ViewModel = (PlotsVM)(this.DataContext);
            ViewModel.PlotsView = this;

            InitPlots();
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
        }
    }
}
