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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphPlotting.View
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl
    {
        private PlotVM viewModel;

        public object Id 
        { 
            get
            {
                return viewModel.Id;
            }
            set 
            {
                viewModel.Id = value;
            } 
        }

        public List<int> Spo2s { get { return viewModel.Spo2s; } }

        public int Spo2 { get { return viewModel.Spo2; } }

        public List<int> Pulses { get { return viewModel.Pulses; } }

        public int Pulse { get { return viewModel.Pulse; } }

        public List<int> PulseWaveforms { get { return viewModel.PulseWaveforms; } }

        public int SignalStrength { get { return viewModel.SignalStrength; } }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string),
            typeof(PlotControl), new PropertyMetadata(""));

        public PlotControl()
        {
            viewModel = new PlotVM();
            InitializeComponent();
        }
    }
}
